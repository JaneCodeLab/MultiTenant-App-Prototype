
namespace ApplicationService;

public interface IBaseApiService : IDisposable
{
    bool VaildateTenantUnitOfWork(int tenantId);
}