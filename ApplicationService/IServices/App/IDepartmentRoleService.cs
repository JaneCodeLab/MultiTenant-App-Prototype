
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IDepartmentRoleService : IBaseService<DepartmentRole, Guid, TenantDbContext>
{
    Task<ICollection<DepartmentRole>> GetDepartmentRoles(int departmentId);
    Task<ServiceResult> CustomUpdateAsync(DepartmentRole inputModel, SysCustomUser user);
}