
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IApiLogService : IBaseService<SysApiLog, Guid, TenantDbContext>
{
}