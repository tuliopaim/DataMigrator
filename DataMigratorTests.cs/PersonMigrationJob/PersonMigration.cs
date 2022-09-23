using DataMigrator;
using DataMigratorTests.cs.Person;

namespace DataMigratorTests.cs.PersonMigrationJob;

public class PersonMigration : IMigrationService<PersonDto>
{
    private readonly PersonRepository _personRepository;

    public PersonMigration(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task AddData(List<PersonDto> dataToAdd)
    {
        foreach (var data in dataToAdd) _personRepository.Add(data);

        return Task.CompletedTask;
    }

    public async Task EditData(List<(PersonDto OldData, PersonDto NewData)> dataToEdit)
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

    public Task RemoveData(List<PersonDto> dataToRemove)
    {
        foreach (var data in dataToRemove) _personRepository.Remove(data);

        return Task.CompletedTask;
    }
}
