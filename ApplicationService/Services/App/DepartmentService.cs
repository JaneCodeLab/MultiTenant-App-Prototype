
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class DepartmentService : BaseService<Department, int, TenantDbContext>, IDepartmentService
{
    private ISprintService _sprintService;
    private IRepository<DepartmentMember, Guid> _departmentMemberRepository;
    public DepartmentService(IUnitOfWork<TenantDbContext> unitOfWork, ISprintService sprintService) : base(unitOfWork)
    {
        _sprintService = sprintService;
        _departmentMemberRepository = unitOfWork.GetRepository<DepartmentMember, Guid>();
    }

    public async Task<ICollection<DepartmentMinimal>> GetUsersDepartmentForSelect(Expression<Func<Department, object>> orderBy, string userId)
    {
        var order = new OrderBy<Department> { Orders = new List<Expression<Func<Department, object>>> { orderBy }, OrderType = OrderType.Asc };

        var departments = (await _departmentMemberRepository.GetListAsync(c => c.UserId == userId && c.Active)).Select(s => s.DepartmentId);
        return await _repository.GetListAsync(c => c.Active && departments.Contains(c.Id), order, s => new DepartmentMinimal { Id = s.Id, Title = s.Title });
    }
    public async Task<ICollection<DepartmentMinimal>> GetDepartmentForSelect()
    {
        var order = new OrderBy<Department> { Orders = new List<Expression<Func<Department, object>>> { o => o.Title }, OrderType = OrderType.Asc };
        return await _repository.GetListAsync(c => c.Active, order, s => new DepartmentMinimal { Id = s.Id, Title = s.Title });
    }
    public async Task<ServiceResult> CustomUpdateAsync(Department inputModel, SysCustomUser user)
    {
        try
        {
            var model = new Department
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<Department, object>>>() { c => c.Title };
            _repository.Update(model, UpdatableFields, user);
            return await SaveAsync(model, user, CrudType.CustomUpdate, UpdatableFields);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }
    public override async Task<ServiceResult> CreateAsync(Department model, SysCustomUser user)
    {
        var createResult = await base.CreateAsync(model, user);
        if (createResult.Type == ServiceResultType.Succeed)
            await _sprintService.CreateBacklogAsync(model.Id);

        return createResult;
    }
    public override async Task<DuplicationResult> IsDuplicated(Department model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.Department
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (!string.IsNullOrEmpty(model.Title))
        {
            predicate = predicate.And(c => c.Title.ToLower() == model.Title.ToLower());
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(DepartmentExpression.Title.ToInt());
        }
        return result;
    }
    public ExpressionStarter<Department> GetPredicate(CrudType ctype, int id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<Department>(c => c.Id != id) : PredicateBuilder.New<Department>();
}