
using ApplicationCore.DomainModel;
using ApplicationService.CustomModels.ApiModels;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysApiUserService : IBaseService<SysApiUser, int, ApplicationDbContext>
{
    Task<ApiResponse<SysApiUser>> GetAsync(string Email, string Password, string UserName, Language language);
}