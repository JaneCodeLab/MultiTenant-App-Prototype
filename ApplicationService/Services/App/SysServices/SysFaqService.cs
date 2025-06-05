
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public class SysFaqService : BaseService<SysFaq, Guid, ApplicationDbContext>, ISysFaqService
{
    public SysFaqService(IUnitOfWork<ApplicationDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<SysFaq>> GetFaqsAsync(string controller, string action, Language language)
    {
        return await _repository.GetListAsync(c => c.ControllerName == controller && c.ViewName == action && c.Language == language);
    }
}