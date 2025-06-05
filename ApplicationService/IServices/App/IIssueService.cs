
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IIssueService : IBaseService<Issue, Guid, TenantDbContext>
{
    Task<ICollection<Issue>> GetBacklogItemsAsync(int backlogId);
    Task<ICollection<Issue>> GetSprintPlanItemsAsync(int sprintId);
    Task<ServiceResult> CustomUpdateAsync(Issue inputModel, SysCustomUser user);
}