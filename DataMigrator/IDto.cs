namespace DataMigrator;

public interface IDto<T> : IEquatable<T> where T : class, IDto<T>
{
    public abstract bool SemanticEquals(T? other);
}