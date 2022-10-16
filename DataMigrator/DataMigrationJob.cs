namespace DataMigrator;

public abstract class DataMigrationJob<T, TJobDto>
    where T : class, IDto<T>
    where TJobDto : class, IJobDto<TJobDto>
{
    private readonly ISourceService<T, TJobDto> _sourceService;
    private readonly IDestinyService<T, TJobDto> _destinyService;

    public DataMigrationJob(
        ISourceService<T, TJobDto> sourceService,
        IDestinyService<T, TJobDto> destinyService)
    {
        _sourceService = sourceService;
        _destinyService = destinyService;
    }

    public async Task Migrate(TJobDto jobDto)
    {
        var sourceData = await _sourceService.Get(jobDto);
        var destinyData = await _destinyService.Get(jobDto);

        await ProcessChanges(sourceData, destinyData, jobDto);

        await _destinyService.SaveChangesAsync();
    }

    private async Task ProcessChanges(List<T> sourceData, List<T> destinyData, TJobDto jobDto)
    {
        List<T> dataToAdd = sourceData.Where(id => destinyData.All(cd => !cd.SemanticEquals(id))).ToList();

        List<T> dataToRemove = destinyData.Where(cd => sourceData.All(id => !id.SemanticEquals(cd))).ToList();

        List<(T OldData, T NewData)> dataToEdit = GetDataToEdit(sourceData, destinyData).ToList();

        if (!dataToAdd.Any() && !dataToRemove.Any() && !dataToEdit.Any()) return;

        await AddData(dataToAdd, jobDto);
        await RemoveData(dataToRemove, jobDto);
        await EditData(dataToEdit, jobDto);
    }

    private static IEnumerable<(T OldData, T NewData)> GetDataToEdit(List<T> sourceData, List<T> destinyData)
    {
        foreach (var oldData in destinyData)
        {
            var newData = sourceData
                .FirstOrDefault(sd => sd.SemanticEquals(oldData) && !sd.Equals(oldData));

            if (newData is null) continue;

            yield return (oldData, newData);
        }
    }

    protected abstract Task AddData(List<T> dataToAdd, TJobDto jobDto);
    protected abstract Task EditData(List<(T OldData, T NewData)> dataToEdit, TJobDto jobDto);
    protected abstract Task RemoveData(List<T> dataToRemove, TJobDto jobDto);
}
