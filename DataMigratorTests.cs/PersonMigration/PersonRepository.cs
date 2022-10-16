using DataMigrator;
using DataMigratorTests.cs.Dtos;

namespace DataMigratorTests.cs.PersonMigration;

public class PersonRepository : IDestinyService<PersonDto, PersonJobDto>
{
    public List<PersonDto> People { get; set; } = new();

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }

    public Task<List<PersonDto>> Get(PersonJobDto jobDto)
    {
        return Task.FromResult(People);
    }

    public void Add(PersonDto data)
    {
        People.Add(data);
    }

    public void Remove(PersonDto data)
    {
        People.Remove(data);
    }

    public Task<List<PersonDto>> GetByIds(List<int> idsToGet)
    {
        var people = People.Where(p => idsToGet.Contains(p.Id)).ToList();

        return Task.FromResult(people);
    }
}
