
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using System.Linq.Expressions;

namespace ApplicationService;

public interface IDepartmentService : IBaseService<Department, int, TenantDbContext>
{
    Task<ICollection<DepartmentMinimal>> GetUsersDepartmentForSelect(Expression<Func<Department, object>> orderBy, string userId);
    Task<ICollection<DepartmentMinimal>> GetDepartmentForSelect();
    Task<ServiceResult> CustomUpdateAsync(Department inputModel, SysCustomUser user);
}