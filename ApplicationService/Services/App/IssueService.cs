
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class IssueService : BaseService<Issue, Guid, TenantDbContext>, IIssueService
{
    public IssueService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<Issue>> GetBacklogItemsAsync(int backlogId)
    {
        var predicate = PredicateBuilder.New<Issue>(true);
        predicate.And(c => c.SprintId == backlogId);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }

    public async Task<ICollection<Issue>> GetSprintPlanItemsAsync(int sprintId)
    {
        var predicate = PredicateBuilder.New<Issue>(true);
        predicate.And(c => c.SprintId == sprintId);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }

    public async Task<ServiceResult> CustomUpdateAsync(Issue inputModel, SysCustomUser user)
    {
        try
        {
            var model = new Issue
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<Issue, object>>>() { c => c.Title, };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public override async Task<DuplicationResult> IsDuplicated(Issue model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.SprintTask
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

    public ExpressionStarter<Issue> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<Issue>(c => c.Id != id) : PredicateBuilder.New<Issue>();
}