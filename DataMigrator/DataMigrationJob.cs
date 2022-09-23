namespace DataMigrator;

public class DataMigrationJob<T> where T : class, IDto<T>
{
    private readonly IMigrationService<T> _migrationService;
    private readonly ISourceService<T> _sourceService;
    private readonly IDestinyService<T> _destinyService;

    public DataMigrationJob(
        IMigrationService<T> migrationService,
        ISourceService<T> sourceService,
        IDestinyService<T> destinyService)
    {
        _migrationService = migrationService;
        _sourceService = sourceService;
        _destinyService = destinyService;
    }

    public async Task Migrate()
    {
        var sourceData = await _sourceService.Get();
        var destinyData = await _destinyService.Get();

        await ProcessChanges(sourceData, destinyData);

        await _destinyService.SaveChangesAsync();
    }

    private async Task ProcessChanges(List<T> sourceData, List<T> destinyData)
    {
        List<T> dataToAdd = sourceData.Where(id => destinyData.All(cd => !cd.SemanticEquals(id))).ToList();

        List<T> dataToRemove = destinyData.Where(cd => sourceData.All(id => !id.SemanticEquals(cd))).ToList();

        List<(T OldData, T NewData)> dataToEdit = GetDataToEdit(sourceData, destinyData).ToList();

        if (!dataToAdd.Any() && !dataToRemove.Any() && !dataToEdit.Any()) return;

        await AddData(dataToAdd);
        await RemoveData(dataToRemove);
        await EditData(dataToEdit);
    }

    private static IEnumerable<(T OldData, T NewData)> GetDataToEdit(List<T> sourceData, List<T> destinyData)
    {
        foreach (var oldData in destinyData)
        {
            var newData = sourceData
                .FirstOrDefault(id => id.SemanticEquals(oldData) && !id.Equals(oldData));

            if (newData is null) continue;

            yield return (oldData, newData);
        }
    }

    private async Task AddData(List<T> dataToAdd)
    {
        await _migrationService.AddData(dataToAdd);
    }

    private async Task RemoveData(List<T> dataToRemove)
    {
        await _migrationService.RemoveData(dataToRemove);
    }

    private async Task EditData(List<(T OldData, T NewData)> dataToEdit)
    {
        await _migrationService.EditData(dataToEdit);
    }
}
