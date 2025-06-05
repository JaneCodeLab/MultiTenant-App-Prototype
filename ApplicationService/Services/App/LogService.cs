
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class LogService : BaseService<SysLog, Guid, TenantDbContext>, ILogService
{
    public LogService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }
}