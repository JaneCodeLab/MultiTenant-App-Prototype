
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysTenantService : IBaseService<SysTenant, int, ApplicationDbContext>
{
    void FetchHelperTenants();
    Task<List<SysTenant>> GetActiveTenantsAsync();
}