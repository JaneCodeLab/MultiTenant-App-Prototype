
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISprintTaskService : IBaseService<SprintTask, Guid, TenantDbContext>
{
    Task<ServiceResult> AddToBacklog(Guid id, SysCustomUser user);
    Task<ServiceResult> AddToCurrentSprint(Guid id, SysCustomUser user);
    Task<ICollection<SprintTask>> GetBacklogItemsAsync(int backlogId);
    Task<ICollection<SprintTask>> GetSprintPlanItemsAsync(int sprintId);
    Task<ICollection<SprintTask>> GetUsersTasksWithParentAsync(string userId);
    Task<ICollection<SprintTask>> GetUsersCurrentTasksAsync(string userId, int departmentId);
    Task<ServiceResult> ChangeProressStatus(Guid id, ProgressStatus progressStatus, SysCustomUser user);
    Task<ServiceResult> CustomUpdateAsync(SprintTask inputModel, SysCustomUser user);
    Task<ServiceResult> UpdateTimeAsync(SprintTask inputModel, SysCustomUser user);
    Task<ICollection<SprintTask>> GetUsersSprintsTasksAsync(string userId, int sprintId);
}