using DataMigrator;

namespace DataMigratorTests.cs.Person;

public class PersonDto : IDto<PersonDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public bool SemanticEquals(PersonDto other)
    {
        if (other == null) return false;

        return Id == other.Id;
    }

    public bool Equals(PersonDto other)
    {
        if (other == null) return false;
        return
            Id == other.Id &&
            Name == other.Name &&
            Age == other.Age;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as PersonDto);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Age);
    }
}