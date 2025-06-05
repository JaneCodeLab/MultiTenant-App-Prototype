
namespace ApplicationCore.DomainModel;

public class TaskAssignee : BaseEntity<Guid>
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ProfileImage { get; set; } = string.Empty;

    public Guid SprintTaskId { get; set; }
}