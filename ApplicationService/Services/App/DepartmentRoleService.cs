
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class DepartmentRoleService : BaseService<DepartmentRole, Guid, TenantDbContext>, IDepartmentRoleService
{
    public DepartmentRoleService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }
    public async Task<ServiceResult> CustomUpdateAsync(DepartmentRole inputModel, SysCustomUser user)
    {
        try
        {
            var model = new DepartmentRole
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<DepartmentRole, object>>>() { c => c.Title };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public async Task<ICollection<DepartmentRole>> GetDepartmentRoles(int departmentId) =>
        await _repository.GetListAsync(c => c.DepartmentId == departmentId);

    public override async Task<DuplicationResult> IsDuplicated(DepartmentRole model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.Project
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (model.DepartmentId > 0)
        {
            predicate = predicate.And(c => c.DepartmentId == model.DepartmentId && c.Title == model.Title);
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(ProjectExpression.Title.ToInt());
        }
        return result;
    }

    public ExpressionStarter<DepartmentRole> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<DepartmentRole>(c => c.Id != id) : PredicateBuilder.New<DepartmentRole>();

}