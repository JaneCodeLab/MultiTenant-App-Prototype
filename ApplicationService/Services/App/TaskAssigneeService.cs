
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class TaskAssigneeService : BaseService<TaskAssignee, Guid, TenantDbContext>, ITaskAssigneeService
{
    public TaskAssigneeService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ServiceResult> CustomUpdateAsync(TaskAssignee inputModel, SysCustomUser user)
    {
        try
        {
            var model = new TaskAssignee
            {
                Id = inputModel.Id,
                SprintTaskId = inputModel.SprintTaskId,
                UserId = inputModel.UserId,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<TaskAssignee, object>>>() { c => c.SprintTaskId, c => c.UserId };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public override async Task<DuplicationResult> IsDuplicated(TaskAssignee model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.TaskAssignee
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (!string.IsNullOrEmpty(model.UserId))
        {
            predicate = predicate.And(c => c.SprintTaskId == model.SprintTaskId && c.UserId == model.UserId);
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(ProjectExpression.Title.ToInt());
        }
        return result;
    }

    public ExpressionStarter<TaskAssignee> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<TaskAssignee>(c => c.Id != id) : PredicateBuilder.New<TaskAssignee>();
}