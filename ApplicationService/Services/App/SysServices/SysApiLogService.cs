
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class SysApiLogService : BaseService<SysApiLog, Guid, ApplicationDbContext>, ISysApiLogService
{
    public SysApiLogService(IUnitOfWork<ApplicationDbContext> unitOfWork)
                        : base(unitOfWork)
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