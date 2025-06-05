
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class DepartmentCustomerService : BaseService<DepartmentCustomer, int, TenantDbContext>, IDepartmentCustomerService
{
    public DepartmentCustomerService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ICollection<DepartmentCustomer>> GetDepartmentCustomer(int departmentId) => await _repository.GetListAsync(c => c.DepartmentId == departmentId, GetIncludes());

    public async Task<ServiceResult> CustomUpdateAsync(DepartmentCustomer inputModel, SysCustomUser user)
    {
        try
        {
            var model = new DepartmentCustomer
            {
                Id = inputModel.Id,
                CustomerId = inputModel.CustomerId,
                DepartmentId = inputModel.DepartmentId,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<DepartmentCustomer, object>>>() {
                                                                                       c => c.DepartmentId,
                                                                                       c => c.CustomerId,
                                                                                     };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }

    public override List<string> GetIncludes() => new()
    {
        nameof(DepartmentCustomer.Customer),
        nameof(DepartmentCustomer.Department),
    };

    public override async Task<DuplicationResult> IsDuplicated(DepartmentCustomer model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.Project
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (model.DepartmentId > 0)
        {
            predicate = predicate.And(c => c.DepartmentId == model.DepartmentId && c.CustomerId == model.CustomerId);
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(ProjectExpression.Title.ToInt());
        }
        return result;
    }

    public ExpressionStarter<DepartmentCustomer> GetPredicate(CrudType ctype, int id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<DepartmentCustomer>(c => c.Id != id) : PredicateBuilder.New<DepartmentCustomer>();
}