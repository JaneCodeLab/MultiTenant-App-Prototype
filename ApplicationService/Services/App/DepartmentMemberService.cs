
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationCore.DomainService;
using Infrastructure.Helpers;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class DepartmentMemberService : BaseService<DepartmentMember, Guid, TenantDbContext>, IDepartmentMemberService
{
    private readonly ISysCustomUserService _sysCustomUserService;
    public DepartmentMemberService(IUnitOfWork<TenantDbContext> unitOfWork, ISysCustomUserService sysCustomUserService) : base(unitOfWork)
    {
        _sysCustomUserService = sysCustomUserService;
    }

    public async Task<ICollection<DepartmentMember>> GetDepartmentMembers(int departmentId) =>
        await _repository.GetListAsync(c => c.DepartmentId == departmentId, GetIncludes());

    public async Task<ServiceResult> CustomUpdateAsync(DepartmentMember inputModel, SysCustomUser user)
    {
        try
        {
            var newMemberUser = await _sysCustomUserService.FindAsync(inputModel.UserId, true);
            var model = new DepartmentMember
            {
                Id = inputModel.Id,
                DepartmentRoleId = inputModel.DepartmentRoleId,
                SupervisorMemberId = inputModel.SupervisorMemberId,
                UserId = inputModel.UserId,
                FullName = newMemberUser.GetFullName(),
                ProfileImage = newMemberUser.ProfileImage ?? (newMemberUser.Gender == Gender.Female ? "default_female.png" : "default_male.png"),
            };

            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            var UpdatableFields = new List<Expression<Func<DepartmentMember, object>>>() {
                                                                                       c => c.DepartmentRoleId,
                                                                                       c => c.SupervisorMemberId,
                                                                                       c => c.UserId,
                                                                                       c => c.FullName,
                                                                                       c => c.ProfileImage,
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
        nameof(DepartmentMember.DepartmentRole),
    };

    public override async Task<ServiceResult> CreateAsync(DepartmentMember model, SysCustomUser user)
    {
        var newMemberUser = await _sysCustomUserService.FindAsync(model.UserId, true);
        model.FullName = newMemberUser.GetFullName();
        model.ProfileImage = newMemberUser.ProfileImage ?? (newMemberUser.Gender == Gender.Female ? "default_female.png" : "default_male.png");
        return await base.CreateAsync(model, user);
    }

    public override async Task<DuplicationResult> IsDuplicated(DepartmentMember model, CrudType ctype)
    {
        var result = new DuplicationResult
        {
            ExpressionType = ExpressionTypes.DepartmentMember
        };

        var predicate = GetPredicate(ctype, model.Id);
        if (model.DepartmentId > 0)
        {
            predicate = predicate.And(c => c.DepartmentId == model.DepartmentId && c.UserId == model.UserId);
            if (await _repository.Any(predicate))
                result.DuplicatedFields.Add(DepartmentMemberExpression.MemberId.ToInt());
        }
        return result;
    }

    public ExpressionStarter<DepartmentMember> GetPredicate(CrudType ctype, Guid id) => (ctype == CrudType.Update || ctype == CrudType.CustomUpdate) ? PredicateBuilder.New<DepartmentMember>(c => c.Id != id) : PredicateBuilder.New<DepartmentMember>();

}