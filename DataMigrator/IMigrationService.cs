namespace DataMigrator;
public interface IMigrationService<T> where T : class, IDto<T>
{
    Task AddData(List<T> dataToAdd);
    Task EditData(List<(T OldData, T NewData)> dataToEdit);
    Task RemoveData(List<T> dataToRemove);
}
