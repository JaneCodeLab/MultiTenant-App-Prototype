
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysHelpService : BaseService<SysHelp, Guid, ApplicationDbContext>, ISysHelpService
{
    public SysHelpService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<string> GetHelpAsync(string controller, string action, Language language)
    {
        var help = await _repository.FirstAsync(c => c.ControlerName == controller && c.ViewName == action && c.Language == language);
        if (help != null)
            return help.Content;
        else
            return string.Empty;
    }
}