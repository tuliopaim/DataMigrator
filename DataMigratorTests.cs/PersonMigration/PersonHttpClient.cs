using DataMigrator;
using DataMigratorTests.cs.Dtos;

namespace DataMigratorTests.cs.PersonMigration;

public class PersonHttpClient : ISourceService<PersonDto, PersonJobDto>
{
    public List<PersonDto> People { get; set; } = new();

    public Task<List<PersonDto>> Get(PersonJobDto jobDto)
    {
        return Task.FromResult(People);
    }
}
