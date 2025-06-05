
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IDepartmentCustomerService : IBaseService<DepartmentCustomer, int, TenantDbContext>
{
    Task<ServiceResult> CustomUpdateAsync(DepartmentCustomer inputModel, SysCustomUser user);
    Task<ICollection<DepartmentCustomer>> GetDepartmentCustomer(int departmentId);
}