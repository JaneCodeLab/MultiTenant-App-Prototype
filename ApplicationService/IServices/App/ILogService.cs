
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ILogService : IBaseService<SysLog, Guid, TenantDbContext>
{
}