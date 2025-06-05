
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysApiLogService : IBaseService<SysApiLog, Guid, ApplicationDbContext>
{
}