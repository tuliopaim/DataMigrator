using Bogus;
using DataMigrator;
using DataMigratorTests.cs.Dtos;
using DataMigratorTests.cs.PersonMigration;

namespace DataMigratorTests.cs;
public partial class DataMigrationTests
{
    private PersonRepository _personRepository;
    private PersonHttpClient _personHttpClient;
    private DataMigrationJob<PersonDto, PersonJobDto> _job;

    public DataMigrationTests()
    {
        _personRepository = new PersonRepository();
        _personHttpClient = new PersonHttpClient();

        _job = new PersonMigrationJob(
            _personRepository,
            _personHttpClient,
            _personRepository);
    }

    public List<PersonDto> GetRandomPerson()
    {
        return new Faker<PersonDto>()
            .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
            .RuleFor(x => x.Age, f => f.Random.Int(1, 50))
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .Generate(10);
    }

    public PersonJobDto GetPersonJobDto()
    {
        return new Faker<PersonJobDto>()
            .RuleFor(x => x.CompanyId, f => f.Random.Int(1, 1000))
            .Generate();
    }
}