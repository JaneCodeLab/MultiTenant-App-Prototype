
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class ProjectService : BaseService<Project, Guid, TenantDbContext>, IProjectService
{
    IDepartmentCustomerService _departmentCustomerService;
    public ProjectService(IUnitOfWork<TenantDbContext> unitOfWork, IDepartmentCustomerService departmentCustomerService) : base(unitOfWork)
    {
        _departmentCustomerService = departmentCustomerService;
    }

    public async Task<ICollection<Project>> GetCustomerProject(int customerId)
    {
        return await _repository.GetListAsync(c => c.CustomerId == customerId);
    }

    public async Task<ICollection<Project>> GetDepartmentProjectsAsync(int departmentId)
    {
        var _departmentCustomerRepository = _unitOfWork.GetRepository<DepartmentCustomer, int>();
        var customerList = await _departmentCustomerRepository.GetListAsync(c => c.DepartmentId == departmentId);

        return await _repository.GetListAsync(c => customerList.Select(s => s.CustomerId).Contains(c.CustomerId));
    }

    public async Task<ServiceResult> CustomUpdateAsync(Project inputModel, SysCustomUser user)
    {
        try
        {
            var model = new Project
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                CustomerId = inputModel.CustomerId,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<Project, object>>>() {
                                                                                       c => c.Title,
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
        nameof(Project.Customer),
    };

    public override async Task<DuplicationResult> IsDuplicated(Project model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.Project
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (!string.IsNullOrEmpty(model.Title))
        {
            predicate = predicate.And(c => c.Title.ToLower() == model.Title.ToLower() && c.CustomerId == model.CustomerId);
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(ProjectExpression.Title.ToInt());
        }
        return result;
    }

    public ExpressionStarter<Project> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<Project>(c => c.Id != id) : PredicateBuilder.New<Project>();
}