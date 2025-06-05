
namespace ApplicationCore.DomainModel;

public class SysLog : BaseEntity<Guid>
{
    public SysLog() => Id = Guid.NewGuid();

    public string? EntityName { get; set; }
    public string? EntityId { get; set; }
    public CrudType CrudType { get; set; }
    public string? Entity { get; set; }
}