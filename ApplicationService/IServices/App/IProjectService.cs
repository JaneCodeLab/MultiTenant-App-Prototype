
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IProjectService : IBaseService<Project, Guid, TenantDbContext>
{
    Task<ServiceResult> CustomUpdateAsync(Project inputModel, SysCustomUser user);
    Task<ICollection<Project>> GetCustomerProject(int customerId);
    Task<ICollection<Project>> GetDepartmentProjectsAsync(int departmentId);
}