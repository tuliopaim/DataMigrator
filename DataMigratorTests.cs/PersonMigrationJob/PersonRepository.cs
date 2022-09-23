using DataMigrator;
using DataMigratorTests.cs.Person;

namespace DataMigratorTests.cs.PersonMigrationJob;

public class PersonRepository : IDestinyService<PersonDto>
{
    public List<PersonDto> People { get; set; } = new();

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }

    public Task<List<PersonDto>> Get()
    {
        return Task.FromResult(People);
    }

    public void Add(PersonDto data)
    {
        People.Add(data);
    }

    public Task<List<PersonDto>> GetByIds(List<int> idsToGet)
    {
        var people = People.Where(p => idsToGet.Contains(p.Id)).ToList();

        return Task.FromResult(people);
    }

    public void Remove(PersonDto data)
    {
        People.Remove(data);
    }
}
