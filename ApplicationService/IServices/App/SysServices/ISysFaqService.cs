
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ISysFaqService : IBaseService<SysFaq, Guid, ApplicationDbContext>
{
    Task<ICollection<SysFaq>> GetFaqsAsync(string controller, string action, Language language);
}