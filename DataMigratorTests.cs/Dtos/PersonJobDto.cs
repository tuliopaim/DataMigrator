using DataMigrator;

namespace DataMigratorTests.cs.Dtos;
public class PersonJobDto : IJobDto<PersonJobDto>
{
    public int CompanyId { get; set; }
}
