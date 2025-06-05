
namespace ApplicationService;

public class BacklogService : IBacklogService
{
    private ISprintTaskService _sprintTaskService;
    private IIssueService _issueService;
    private ISprintService _sprintService;
    public BacklogService(ISprintTaskService sprintTaskService, ISprintService sprintService, IIssueService issueService)
    {
        _sprintTaskService = sprintTaskService;
        _issueService = issueService;
        _sprintService = sprintService;
    }

    public async Task<ICollection<BacklogMinimalListItem>> GetBacklogItems(int departmentId)
    {
        var backlogId = (await _sprintService.GetBacklogIdAsync(departmentId)) ?? 0;
        if (backlogId == 0)
            return default;

        var tasks = (await _sprintTaskService.GetBacklogItemsAsync(backlogId)).Select(s => new BacklogMinimalListItem
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
            Estimate = s.Estimate,
            TaskType = s.TaskType,
            IsIssue = false,
            Assignees = string.Join(" , ", s.Assignees.Select(a => a.FullName)),
        }).ToList();

        var issues = (await _issueService.GetBacklogItemsAsync(backlogId)).Select(s => new BacklogMinimalListItem
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
        }).ToList();

        tasks.AddRange(issues);
        return tasks;
    }
}