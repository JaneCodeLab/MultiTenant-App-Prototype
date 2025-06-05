
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;

namespace ApplicationService;

public class SprintPlanMinimalListItem : BaseMinimalListItem<Guid>
{
    private readonly string _userTimeZone;
    public SprintPlanMinimalListItem()
    {
        _userTimeZone = GeneralVariables.DefaultTimeZone;
    }
    public SprintPlanMinimalListItem(string userTimeZone)
    {
        _userTimeZone = userTimeZone;
    }

    public bool IsIssue { get; set; }
    public int Code { get; set; }
    public string Title { get; set; } = string.Empty;
    public Priority Priority { get; set; } = Priority.Medium;
    public ProgressStatus Status { get; set; } = ProgressStatus.ToDo;
    public TaskType TaskType { get; set; }
    public DateOnly Deadline { get; set; }
    public decimal Estimate { get; set; }
    public Guid? ProjectId { get; set; }
    public string? ProjectTitle { get; set; }
    public string? Assignees { get; set; } = string.Empty;
    [MainColumn(OrderBy = nameof(SysLog.CreatedAt))]
    public string CreatedAtText => CreatedAt.GetLocalDateTime(_userTimeZone).ToDateTimeStyle();
    public string? UpdatedAtText => UpdatedAt?.GetLocalDateTime(_userTimeZone).ToDateTimeStyle();
    public string? CreatedBy { get; set; } = string.Empty;
}