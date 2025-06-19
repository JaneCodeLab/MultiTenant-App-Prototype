using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysApiUserService : IBaseService<SysApiUser, int, ApplicationDbContext>
{
    /// <summary>
    /// Retrieves a SysApiUser by credentials and language.
    /// </summary>
    /// <returns>An ApiResponse containing the SysApiUser.</returns>
    Task<ApiResponse<SysApiUser>> GetAsync(string Email, string Password, string UserName, Language language);
}
