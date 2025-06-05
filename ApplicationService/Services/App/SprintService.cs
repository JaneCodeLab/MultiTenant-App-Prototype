
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class SprintService : BaseService<Sprint, int, TenantDbContext>, ISprintService
{
    public SprintService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ServiceResult> MakeCurrent(int id, int departmentId, SysCustomUser user)
    {
        var UpdatableFields = new List<Expression<Func<Sprint, object>>>() { c => c.Current };
        var model = new Sprint
        {
            Id = id,
            Current = true,
        };
        _repository.Update(model, UpdatableFields, user);

        return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
    }

    public async Task<ICollection<Sprint>> GetDepartmentSprintsAsync(int departmentId)
    {
        var predicate = PredicateBuilder.New<Sprint>(true);
        predicate = predicate.And(c => c.DepartmentId == departmentId && !c.Backlog);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }

    public async Task<ICollection<Sprint>> GetAllSprintsAsync(BaseFilter filter, string sourceTimeZoneId)
    {
        var predicate = MakePredicate(filter, sourceTimeZoneId);
        predicate = predicate.And(c => !c.Backlog);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }

    public async Task<ServiceResult> CreateBacklogAsync(int departmentId)
    {
        var model = new Sprint
        {
            DepartmentId = departmentId,
            Title = nameof(Sprint.Backlog),
            Backlog = true,
            Locked = true,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1).AddDays(-1)),
        };
        return await base.CreateAsync(model, GeneralVariables.SystemUser);
    }

    public async Task<int?> GetBacklogIdAsync(int departmentId) => (await _repository.FirstAsync(c => c.DepartmentId == departmentId && c.Backlog))?.Id;
    public async Task<int?> GetCurrentSprintIdAsync(int departmentId) => (await _repository.FirstAsync(c => c.DepartmentId == departmentId && c.Current))?.Id;

    public async Task<ServiceResult> CustomUpdateAsync(Sprint inputModel, SysCustomUser user)
    {
        try
        {
            var model = new Sprint
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                StartDate = inputModel.StartDate,
                EndDate = inputModel.EndDate,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<Sprint, object>>>() {
                                                                                       c => c.Title,
                                                                                       c => c.StartDate,
                                                                                       c => c.EndDate,
                                                                                     };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public override async Task<DuplicationResult> IsDuplicated(Sprint model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.Project
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (!string.IsNullOrEmpty(model.Title))
        {
            predicate = predicate.And(c => c.Title.ToLower() == model.Title.ToLower());
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(ProjectExpression.Title.ToInt());
        }
        return result;
    }

    public ExpressionStarter<Sprint> GetPredicate(CrudType ctype, int id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<Sprint>(c => c.Id != id) : PredicateBuilder.New<Sprint>();
}