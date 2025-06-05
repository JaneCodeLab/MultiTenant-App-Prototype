
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysTenantUserService : BaseService<SysTenantUser, int, ApplicationDbContext>, ISysTenantUserService
{
    public SysTenantUserService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<SysTenantUser>> GetUsersTenants(string userId)
    {
        return await _repository.GetListAsync(c => c.UserId == userId, GetIncludes());
    }
    public async Task<ICollection<SysTenantUser>> GetTenantsUsers(int tenantId)
    {
        return await _repository.GetListAsync(c => c.TenantId == tenantId, GetIncludes());
    }

    public async Task AddTenantUser(string userId, int tenantId)
    {
        var tenantUser = new SysTenantUser { TenantId = tenantId, UserId = userId };
        await base.CreateAsync(tenantUser, GeneralVariables.SystemUser);
    }

    public async Task Assign(string userId, int[] selectedTenants, SysCustomUser user)
    {
        var usersTenants = await GetUsersTenants(userId);

        var removableUserTenants = usersTenants.Where(c => !selectedTenants.Contains(c.TenantId)).ToList();
        var newTenants = selectedTenants.Where(c => !usersTenants.Select(s => s.TenantId).Contains(c));

        _repository.DeleteRange(removableUserTenants);
        foreach (var tenantId in newTenants)
        {
            SysTenantUser tenantUser = new SysTenantUser
            {
                UserId = userId,
                TenantId = tenantId
            };
            await base.CreateAsync(tenantUser, user);
        }

        _unitOfWork.SaveChanges();
    }

    public override List<string> GetIncludes() => new() { nameof(SysTenantUser.Tenant) };
}