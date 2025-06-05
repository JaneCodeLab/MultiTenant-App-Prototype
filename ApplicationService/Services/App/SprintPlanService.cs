
namespace ApplicationService;

public class SprintPlanService : ISprintPlanService
{
    private ISprintTaskService _sprintTaskService;
    private IIssueService _issueService;
    private ISprintService _sprintService;
    public SprintPlanService(ISprintTaskService sprintTaskService, ISprintService sprintService, IIssueService issueService)
    {
        _sprintTaskService = sprintTaskService;
        _issueService = issueService;
        _sprintService = sprintService;
    }

    public async Task<ICollection<SprintPlanMinimalListItem>> GetSprintPlanItems(int sprintId)
    {
        var tasks = (await _sprintTaskService.GetSprintPlanItemsAsync(sprintId)).Select(s => new SprintPlanMinimalListItem
        {
            Id = s.Id,
            Code = s.Code,
            Title = s.Title,
            Deadline = s.Deadline,
            Active = s.Active,
            CreatedAt = s.CreatedAt,
            CreatedBy = s.CreatedBy,
            Priority = s.Priority,
            ProjectId = s.ProjectId,
            ProjectTitle = s.Project?.Title,
            Status = s.Status,
            TaskType = s.TaskType,
            IsIssue = false,
            Estimate = s.Estimate,
            Assignees = string.Join(" , ", s.Assignees.Select(a => a.FullName)),
        }).ToList();

        var issues = (await _issueService.GetSprintPlanItemsAsync(sprintId)).Select(s => new SprintPlanMinimalListItem
        {
            Id = s.Id,
            Code = s.Code,
            Title = s.Title,
            Deadline = s.Deadline,
            Active = s.Active,
            CreatedAt = s.CreatedAt,
            Priority = s.Priority,
            ProjectId = s.ProjectId,
            ProjectTitle = s.Project?.Title,
            Status = s.Status,
            IsIssue = true,
            Estimate = 0,
            CreatedBy = s.CreatedBy,
        }).ToList();

        tasks.AddRange(issues);
        return tasks;
    }
}