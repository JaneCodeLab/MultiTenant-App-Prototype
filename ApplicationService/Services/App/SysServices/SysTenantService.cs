
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysTenantService : BaseService<SysTenant, int, ApplicationDbContext>, ISysTenantService
{
    public SysTenantService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {

    }
    public void FetchHelperTenants()
    {
        TenantHelper.Tenants = _repository.GetList(s => new TenantConfig
        {
            DbIp = s.DbIp,
            DbPassword = s.DbPassword,
            DbUsername = s.DbUsername,
            TenantId = s.Id,
            Logo = s.Logo,
            Title = s.Title

        }).ToList();
    }

    public async Task<List<SysTenant>> GetActiveTenantsAsync() => (await _repository.GetListAsync(c => c.Active == true, s => new SysTenant { Id = s.Id, Title = s.Title })).ToList();
}