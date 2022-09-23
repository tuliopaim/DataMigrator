namespace DataMigrator;

public interface ISourceService<T> where T : class, IDto<T>
{
    Task<List<T>> Get();
}