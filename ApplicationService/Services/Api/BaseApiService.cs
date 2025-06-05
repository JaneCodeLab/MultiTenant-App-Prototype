
using ApplicationCore.DomainModel;
using Infrastructure;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class BaseApiService : IBaseApiService
{
    public UnitOfWork<TenantDbContext> unitOfWork;

    public bool VaildateTenantUnitOfWork(int tenantId)
    {
        if (unitOfWork != null)
            return true;

        var tenantProvider = new TenantProvider(tenantId);
        if (tenantProvider.IsTenantValid)
        {
            unitOfWork = new UnitOfWork<TenantDbContext>(new TenantDbContext(tenantProvider));
            return true;
        }
        else
            return false;
    }

    public async Task<bool> SendEmail(SysSmtp smtp, string email, string subject, string body)
    {
        var _emailSender = new EmailAdapter(smtp.Host, smtp.Port, smtp.EnableSSL, smtp.Username, smtp.Password);
        if (email.IsNullOrEmpty())
            return false;

        await _emailSender.SendEmailAsync(email, subject, body);
        return true;
    }

    public void Dispose()
    {
        if (unitOfWork != null)
            unitOfWork.Dispose();
    }
}