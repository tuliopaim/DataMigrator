namespace DataMigrator;

public interface IDestinyService<T, TJobDto>
    where T : class, IDto<T>
    where TJobDto : class, IJobDto<TJobDto>
{
    Task SaveChangesAsync();
    Task<List<T>> Get(TJobDto jobDto);
}