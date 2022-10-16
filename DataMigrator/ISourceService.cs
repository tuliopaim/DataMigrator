namespace DataMigrator;

public interface ISourceService<T, TJobDto> 
    where T : class, IDto<T>
    where TJobDto : class, IJobDto<TJobDto>
{
    Task<List<T>> Get(TJobDto jobDto);
}