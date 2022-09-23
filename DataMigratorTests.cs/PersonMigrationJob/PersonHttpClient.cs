using DataMigrator;
using DataMigratorTests.cs.Person;

namespace DataMigratorTests.cs.PersonMigrationJob;

public class PersonHttpClient : ISourceService<PersonDto>
{
    public List<PersonDto> People { get; set; } = new();

    public Task<List<PersonDto>> Get()
    {
        return Task.FromResult(People);
    }
}
