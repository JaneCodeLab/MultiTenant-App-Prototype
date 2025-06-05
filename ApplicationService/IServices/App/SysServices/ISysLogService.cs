
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysLogService : IBaseService<SysLog, Guid, ApplicationDbContext>
{
}