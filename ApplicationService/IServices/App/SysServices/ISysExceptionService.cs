
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysExceptionService : IBaseService<SysException, Guid, ApplicationDbContext>
{
}