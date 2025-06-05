
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;

namespace ApplicationService;

public interface IActivityService : IBaseService<Activity, Guid, TenantDbContext>
{
    Task<ServiceResult> CustomUpdateAsync(Activity inputModel, SysCustomUser user);

    Task<ICollection<Activity>> GetUsersActivitiesWithParentAsync(DateTime date, string userId);
}