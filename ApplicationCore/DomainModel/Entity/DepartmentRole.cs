
namespace ApplicationCore.DomainModel;

public class DepartmentRole : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public bool BacklogRead { get; set; } = false;
    public bool BacklogWrite { get; set; } = false;
    public int DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
}