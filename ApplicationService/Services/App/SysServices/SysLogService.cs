
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysLogService : BaseService<SysLog, Guid, ApplicationDbContext>, ISysLogService
{
    public SysLogService(IUnitOfWork<ApplicationDbContext> unitOfWork)
                        : base(unitOfWork)
    {
    }
}