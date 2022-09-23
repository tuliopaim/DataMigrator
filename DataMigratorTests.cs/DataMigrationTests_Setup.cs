using Bogus;
using DataMigrator;
using DataMigratorTests.cs.Person;
using DataMigratorTests.cs.PersonMigrationJob;

namespace DataMigratorTests.cs;
public partial class DataMigrationTests
{
    private PersonRepository _personRepository;
    private PersonHttpClient _personHttpClient;
    private PersonMigration _personMigration;
    private DataMigrationJob<PersonDto> _job;

    public DataMigrationTests()
    {
        _personRepository = new PersonRepository();
        _personHttpClient = new PersonHttpClient();
        _personMigration = new PersonMigration(_personRepository);

        _job = new DataMigrationJob<PersonDto>(
            _personMigration,
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
}