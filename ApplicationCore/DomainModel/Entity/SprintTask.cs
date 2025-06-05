
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.DomainModel;

public class SprintTask : BaseEntity<Guid>
{
    public SprintTask() => Assignees = new HashSet<TaskAssignee>();

    public int Code { get; set; }
    public int SequenceNo { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;
    public ProgressStatus Status { get; set; } = ProgressStatus.ToDo;
    public TaskType TaskType { get; set; }
    public DateOnly Deadline { get; set; }
    public DateOnly? FinishDate { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal Remained { get; set; }
    [Column(TypeName = "decimal(5, 2)")]
    public decimal Estimate { get; set; }
    [Column(TypeName = "decimal(5, 2)")]
    public decimal Completed { get; set; } = 0;

    public int DepartmentId { get; set; }
    public int SprintId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? IssueId { get; set; }

    public virtual Sprint? Sprint { get; set; }
    public virtual Project? Project { get; set; }
    public virtual Issue? Issue { get; set; }
    public ICollection<TaskAssignee> Assignees { get; set; }
}