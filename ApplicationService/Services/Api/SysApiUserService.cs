
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationService.Constants;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysApiUserService : BaseService<SysApiUser, int, ApplicationDbContext>, ISysApiUserService
{
    public SysApiUserService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }
    /// <summary>
    /// Retrieves a SysApiUser by credentials and language.
    /// </summary>
    public async Task<ApiResponse<SysApiUser>> GetAsync(string email, string password, string userName, Language language)
    {
        if (email.IsNullOrEmpty() || password.IsNullOrEmpty() || userName.IsNullOrEmpty())
            return ApiResponse<SysApiUser>.Fail(ApiCommonMessages.AuthenticationFailed);

        var trimmedEmail = email.Trim();
        var trimmedPassword = password.Trim();
        var trimmedUserName = userName.Trim();

        var result = await _repository.FirstAsync(c => c.Email == trimmedEmail && c.Password == trimmedPassword && c.UserName == trimmedUserName);
        return result == null
            ? ApiResponse<SysApiUser>.Fail(ApiCommonMessages.AuthenticationFailed)
            : ApiResponse<SysApiUser>.Ok(result, string.Empty);
    }
}