using DataMigrator;
using DataMigratorTests.cs.Dtos;

namespace DataMigratorTests.cs.PersonMigration;

public class PersonMigrationJob : DataMigrationJob<PersonDto, PersonJobDto>
{
    private readonly PersonRepository _personRepository;

    public PersonMigrationJob(
        PersonRepository personRepository,
        ISourceService<PersonDto, PersonJobDto> sourceService,
        IDestinyService<PersonDto, PersonJobDto> destinyService) : base(sourceService, destinyService)
    {
        _personRepository = personRepository;
    }

    protected override Task AddData(List<PersonDto> dataToAdd, PersonJobDto jobDto)
    {
        foreach (var data in dataToAdd) _personRepository.Add(data);

        return Task.CompletedTask;
    }

    protected override async Task EditData(
        List<(PersonDto OldData, PersonDto NewData)> dataToEdit,
        PersonJobDto jobDto)
    {
        var idsToGet = dataToEdit.Select(dte => dte.NewData.Id).ToList();

        var peopleToEdit = await _personRepository.GetByIds(idsToGet);

        foreach (var (oldData, newData) in dataToEdit)
        {
            var person = peopleToEdit.First(p => p.SemanticEquals(newData));

            person.Name = newData.Name;
            person.Age = newData.Age;
        }
    }

    protected override Task RemoveData(
        List<PersonDto> dataToRemove,
        PersonJobDto jobDto)
    {
        foreach (var data in dataToRemove) _personRepository.Remove(data);

        return Task.CompletedTask;
    }
}
