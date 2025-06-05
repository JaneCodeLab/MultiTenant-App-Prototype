
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysExceptionService : BaseService<SysException, Guid, ApplicationDbContext>, ISysExceptionService
{
    public SysExceptionService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }
}