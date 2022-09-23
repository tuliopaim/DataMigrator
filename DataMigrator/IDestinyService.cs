namespace DataMigrator;

public interface IDestinyService<T> where T : class, IDto<T>
{
    Task SaveChangesAsync();
    Task<List<T>> Get();
    void Add(T data);
    void Remove(T data);
}