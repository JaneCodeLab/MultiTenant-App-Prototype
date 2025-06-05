
using ApplicationCore.DomainModel;

namespace ApplicationService;

public interface ISysCustomUserService
{
    SysCustomUser Deactivate(SysCustomUser model, string user);
    Task Update(SysCustomUser model, string user);
    Task Delete(string id);
    Task ChangeState(string id, string user);
    Task<SysCustomUser> FindAsync(string id, bool detach = false);
    Task<ICollection<SysCustomUser>> GetListAsync(CustomUserFilter filter);
    Task<ICollection<SysCustomUser>> GetListAsync(List<string> idList);
    Task<ICollection<SysCustomUser>> GetEmployeeUserListAsync();
    Task<ICollection<SysCustomUser>> GetTenantEmployeeUserListAsync(int tenantId);

}