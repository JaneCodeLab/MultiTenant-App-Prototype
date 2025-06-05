
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationService.CustomModels.ApiModels;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysApiUserService : BaseService<SysApiUser, int, ApplicationDbContext>, ISysApiUserService
{
    public SysApiUserService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ApiResponse<SysApiUser>> GetAsync(string email, string password, string userName, Language language)
    {
        if (email.IsNullOrEmpty() || password.IsNullOrEmpty() || userName.IsNullOrEmpty())
            return new ApiResponse<SysApiUser>(false, "Invalid Credentials!");

        var result = await _repository.FirstAsync(c => c.Email == email && c.Password == password && c.UserName == userName);
        if (result == null)
            return new ApiResponse<SysApiUser>(false, "Invalid Credentials!");
        else
            return new ApiResponse<SysApiUser>(true, String.Empty, result);
    }
}