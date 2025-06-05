
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IDepartmentMemberService : IBaseService<DepartmentMember, Guid, TenantDbContext>
{
    Task<ICollection<DepartmentMember>> GetDepartmentMembers(int departmentId);
    Task<ServiceResult> CustomUpdateAsync(DepartmentMember inputModel, SysCustomUser user);
}