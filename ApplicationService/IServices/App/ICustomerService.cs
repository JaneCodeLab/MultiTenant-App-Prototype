
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface ICustomerService : IBaseService<Customer, int, TenantDbContext>
{
    Task<ServiceResult> CustomUpdateAsync(Customer inputModel, SysCustomUser user);
}