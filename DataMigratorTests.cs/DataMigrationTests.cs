using DataMigratorTests.cs.Dtos;

namespace DataMigratorTests.cs;

public partial class DataMigrationTests
{
    [Fact]
    public async Task ShouldAddData()
    {
        //arrange
        var initialData = GetRandomPerson();
        var jobData = GetPersonJobDto();

        _personHttpClient.People = initialData;

        //act
        await _job.Migrate(jobData);

        //assert
        foreach (var data in initialData)
        {
            var finalData = _personRepository.People.FirstOrDefault(x => x.Id == data.Id);

            Assert.True(data.Equals(finalData));
        }
    }

    [Fact]
    public async Task ShouldRemoveData()
    {
        //arrange
        var repositoryData = GetRandomPerson();
        var clientData = repositoryData.ToList();
        var jobData = GetPersonJobDto();

        var removedData = new List<PersonDto> { clientData[1], clientData[4], clientData[6] };

        clientData.RemoveAll(x => removedData.Contains(x));

        _personHttpClient.People = clientData;

        //act
        await _job.Migrate(jobData);


        //assert
        foreach (var data in removedData)
        {
            var finalData = _personRepository.People.FirstOrDefault(x => x.Id == data.Id);

            Assert.Null(finalData);
        }

        Assert.Equal(7, _personRepository.People.Count);
    }

    [Fact]
    public async Task ShouldEditData()
    {
        //arrange
        var jobData = GetPersonJobDto();
        var clientData = new List<PersonDto>
        {
            new PersonDto{ Id = 1, Name= "Túlio", Age = 24 },
            new PersonDto{ Id = 2, Name= "Andressa", Age = 22 },
        };
        _personHttpClient.People = clientData;
        
        var repositoryData = new List<PersonDto>
        {
            new PersonDto{ Id = 1, Name= "Túlio", Age = 24 },
            new PersonDto{ Id = 2, Name= "Andressa Cardoso", Age = 22 },
        };
        _personRepository.People = repositoryData;

        //act
        await _job.Migrate(jobData);

        //assert
        foreach (var data in clientData)
        {
            var finalData = _personRepository.People.FirstOrDefault(x => x.Id == data.Id);

            Assert.True(data.Equals(finalData));
        }
    }
}

