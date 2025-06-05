
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SmtpService : BaseService<SysSmtp, int, TenantDbContext>, ISmtpService
{
    public SmtpService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<SysSmtp?> GetActiveSmtp(int tenantId)
    {
        return  await _repository.FirstAsync(c => c.Active);
    }
}