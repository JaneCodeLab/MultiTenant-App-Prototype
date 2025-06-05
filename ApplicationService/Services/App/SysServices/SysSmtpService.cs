
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysSmtpService : BaseService<SysSmtp, int, ApplicationDbContext>, ISysSmtpService
{
    public SysSmtpService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<SysSmtp?> GetActiveSmtp(int tenantId)
    {
        return await _repository.FirstAsync(c => c.Active);
    }
}