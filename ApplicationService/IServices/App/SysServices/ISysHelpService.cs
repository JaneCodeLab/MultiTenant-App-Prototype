
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysHelpService : IBaseService<SysHelp, Guid, ApplicationDbContext>
{
    Task<string> GetHelpAsync(string controller, string action, Language language);
}