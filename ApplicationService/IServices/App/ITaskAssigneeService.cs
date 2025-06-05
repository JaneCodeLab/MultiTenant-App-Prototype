
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ITaskAssigneeService : IBaseService<TaskAssignee, Guid, TenantDbContext>
{
    Task<ServiceResult> CustomUpdateAsync(TaskAssignee inputModel, SysCustomUser user);
}