
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysTenantUserService : IBaseService<SysTenantUser, int, ApplicationDbContext>
{
    Task<ICollection<SysTenantUser>> GetUsersTenants(string userId);
    Task<ICollection<SysTenantUser>> GetTenantsUsers(int tenantId);
    Task AddTenantUser(string userId, int tenantId);
    Task Assign(string userId, int[] selectedTenants, SysCustomUser user);
}