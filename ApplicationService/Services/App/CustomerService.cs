
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class CustomerService : BaseService<Customer, int, TenantDbContext>, ICustomerService
{
    public CustomerService(IUnitOfWork<TenantDbContext> unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<ServiceResult> CustomUpdateAsync(Customer inputModel, SysCustomUser user)
    {
        var model = new Customer
        {
            Id = inputModel.Id,
            Title = inputModel.Title,
        };

        var duplicationResult = await IsDuplicated(model, CrudType.Update);
        if (duplicationResult.IsDuplicated)
            return GetDuplicateMessage(user.Language, duplicationResult);

        if (await IsLockedAsync(model.Id))
            return GetLockedResult(user.Language);

        var UpdatableFields = new List<Expression<Func<Customer, object>>>() { c => c.Title };
        _repository.Update(model, UpdatableFields, user);
        return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
    }

    public override async Task<ServiceResult> ChangeActivateAsync(int id, bool active, SysCustomUser user)
    {
        if (!active)
        {
            var projectRepository = _unitOfWork.GetRepository<Project, Guid>();
            (await projectRepository.GetListAsync(c => c.CustomerId == id)).ForEach(c => c.Active = false);
        }
        return await base.ChangeActivateAsync(id, active, user);
    }

    public override async Task<DuplicationResult> IsDuplicated(Customer model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.Customer
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (!string.IsNullOrEmpty(model.Title))
        {
            predicate = predicate.And(c => c.Title.ToLower() == model.Title.ToLower());
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(CustomerExpression.Title.ToInt());
        }
        return result;
    }

    public ExpressionStarter<Customer> GetPredicate(CrudType ctype, int id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<Customer>(c => c.Id != id) : PredicateBuilder.New<Customer>();
}