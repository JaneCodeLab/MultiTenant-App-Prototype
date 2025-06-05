
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class ApiLogService : BaseService<SysApiLog, Guid, TenantDbContext>, IApiLogService
{
    public ApiLogService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public override Expression<Func<SysApiLog, bool>> MakePredicate(BaseFilter filter, string sourceTimeZoneId)
    {
        var predicate = base.MakePredicate(filter, sourceTimeZoneId);
        var apiLogFilter = filter as ApiLogFilter;

        if (apiLogFilter.ApiRequestTypeParamId != null)
            predicate = predicate.And(c => c.ApiRequestTypeParamId == apiLogFilter.ApiRequestTypeParamId);

        return predicate;
    }
}