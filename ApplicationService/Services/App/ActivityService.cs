
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class ActivityService : BaseService<Activity, Guid, TenantDbContext>, IActivityService
{
    public ActivityService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<Activity>> GetUsersActivitiesWithParentAsync(DateTime date, string userId)
    {
        return await _repository.GetListAsync(c => c.UserId == userId && c.StartDate.Date >= date.Date.AddMonths(-1), GetIncludes());
    }
    public override async Task<Activity?> GetAsync(Guid id) => await _repository.FirstAsync(c => c.Id == id, GetIncludes());
    public override List<string> GetIncludes() => new List<string> { nameof(Activity.SprintTask) };

    public async Task<ServiceResult> CustomUpdateAsync(Activity inputModel, SysCustomUser user)
    {
        try
        {
            var model = new Activity
            {
                Id = inputModel.Id,
                Description = inputModel.Description,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<Activity, object>>>() { c => c.Description, };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public ExpressionStarter<Activity> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<Activity>(c => c.Id != id) : PredicateBuilder.New<Activity>();
}