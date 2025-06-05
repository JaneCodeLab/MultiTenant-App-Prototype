
using ApplicationCore;
using ApplicationCore.DomainModel;
using Infrastructure.SqlServerAdapter;
using LinqKit;
using System.Linq.Expressions;

namespace ApplicationService;

public class SysCustomUserService : ISysCustomUserService
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    public SysCustomUserService(IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetUserRepository();
    }

    public async Task<SysCustomUser> FindAsync(string id, bool detach = false)
    {
        return await _repository.FindAsync(id, detach);
    }

    public async Task<ICollection<SysCustomUser>> GetListAsync(CustomUserFilter filter)
    {
        var predicate = MakePredicate(filter);
        return await _repository.GetListAsync(predicate);
    }

    public async Task<ICollection<SysCustomUser>> GetEmployeeUserListAsync()
    {
        var order = new OrderBy<SysCustomUser> { Orders = new List<Expression<Func<SysCustomUser, object>>> { c => c.Email }, OrderType = OrderType.Asc };
        return await _repository.GetListAsync(c => c.IsEmployeeUser == true, s => new SysCustomUser
        {
            Id = s.Id,
            Email = s.Email,
            CreatedBy = s.CreatedBy,
            CreatedAt = s.CreatedAt,
            Language = s.Language,
            FirstName = s.FirstName,
            LastName = s.LastName
        }, order);
    }
    public async Task<ICollection<SysCustomUser>> GetTenantEmployeeUserListAsync(int tenantId)
    {
        var order = new OrderBy<SysCustomUser> { Orders = new List<Expression<Func<SysCustomUser, object>>> { c => c.Email }, OrderType = OrderType.Asc };
        var tenantUser = _unitOfWork.GetRepository<SysTenantUser, int>();
        var userList = await tenantUser.GetListAsync(c => c.TenantId == tenantId);
        var userIdList = userList.Select(s => s.UserId);
        return await _repository.GetListAsync(c => userIdList.Contains(c.Id) && c.IsEmployeeUser == false,
                                             s => new SysCustomUser
                                             {
                                                 Id = s.Id,
                                                 Email = s.Email,
                                                 CreatedBy = s.CreatedBy,
                                                 CreatedAt = s.CreatedAt,
                                                 Language = s.Language,
                                                 FirstName = s.FirstName,
                                                 LastName = s.LastName
                                             },
                                             order);
    }

    public async Task<ICollection<SysCustomUser>> GetListAsync(List<string> idList) => await _repository.GetListAsync(c => idList.Contains(c.Id));

    public async Task Delete(string id)
    {
        await _repository.Delete(id);
        _unitOfWork.SaveChanges();
    }

    public SysCustomUser Deactivate(SysCustomUser model, string user)
    {
        model.Active = false;
        var result = _repository.Update(model, user);
        _unitOfWork.SaveChanges();
        return result;
    }

    public async Task Update(SysCustomUser model, string user)
    {
        await _repository.UpdateAsync(model, user);
    }

    public async Task ChangeState(string id, string user)
    {
        var model = await FindAsync(id);
        model.EmailConfirmed = !model.EmailConfirmed;
        _repository.Update(model, user);
        _unitOfWork.SaveChanges();
    }

    private Expression<Func<SysCustomUser, bool>> MakePredicate(CustomUserFilter filter)
    {
        var predicate = PredicateBuilder.New<SysCustomUser>(true);
        predicate = predicate.And(x => x.Id != String.Empty);

        if (filter.Active != null)
            predicate = predicate.And(c => c.Active == filter.Active);

        if (filter.IsEmployeeUser != null)
            predicate = predicate.And(c => c.IsEmployeeUser == filter.IsEmployeeUser);

        if (filter.CreatedAtStart != null && filter.CreatedAtStart != new DateTime())
            predicate = predicate.And(c => c.CreatedAt >= filter.CreatedAtStart);
        if (filter.CreatedAtEnd != null && filter.CreatedAtEnd != new DateTime())
            predicate = predicate.And(c => c.CreatedAt < filter.CreatedAtEnd.Value.AddDays(1));

        if (filter.UpdatedAtStart != null && filter.UpdatedAtStart != new DateTime())
            predicate = predicate.And(c => c.CreatedAt >= filter.UpdatedAtStart);
        if (filter.UpdatedAtEnd != null && filter.UpdatedAtEnd != new DateTime())
            predicate = predicate.And(c => c.CreatedAt < filter.UpdatedAtEnd.Value.AddDays(1));

        return predicate;
    }
}