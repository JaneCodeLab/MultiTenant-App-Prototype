
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysSmtpService : IBaseService<SysSmtp, int, ApplicationDbContext>
{
    Task<SysSmtp?> GetActiveSmtp(int tenantId);
}