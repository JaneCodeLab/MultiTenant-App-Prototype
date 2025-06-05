
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISmtpService : IBaseService<SysSmtp, int, TenantDbContext>
{
    Task<SysSmtp?> GetActiveSmtp(int tenantId);
}