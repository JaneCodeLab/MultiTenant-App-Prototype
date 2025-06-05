
namespace ApplicationCore.DomainModel;

public class Issue : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public int Code { get; set; }
    public string? Description { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public ProgressStatus Status { get; set; } = ProgressStatus.ToDo;
    public DateOnly Deadline { get; set; }

    public int SprintId { get; set; }
    public Guid? ProjectId { get; set; }
    public int DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
    public virtual Project? Project { get; set; }
}