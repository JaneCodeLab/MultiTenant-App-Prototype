
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationCore.IRepository.Model;
using Infrastructure.Helpers;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace ApplicationService;

public class BaseService<T, BT, CT> : IBaseService<T, BT, CT> where T : BaseEntity<BT> where CT : DbContext
{
    public readonly IRepository<T, BT> _repository;
    public readonly IRepository<SysLog, Guid> _logRepository;
    public readonly IUnitOfWork<CT> _unitOfWork;

    public BaseService(IUnitOfWork<CT> unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetRepository<T, BT>();
        _logRepository = _unitOfWork.GetRepository<SysLog, Guid>();
    }

    public virtual async Task<T?> GetAsync(BT id)
    {
        return await _repository.FindAsync(id);

    }
    public virtual async Task<ICollection<T>> GetAllAsync(BaseFilter filter, string sourceTimeZoneId)
    {
        var predicate = MakePredicate(filter, sourceTimeZoneId);
        return await _repository.GetListAsync(predicate, GetIncludes());
    }
    public virtual async Task<ICollection<T>> GetAllForSelect(Expression<Func<T, object>> orderBy, bool active = true)
    {
        var order = new OrderBy<T> { Orders = new List<Expression<Func<T, object>>> { orderBy }, OrderType = OrderType.Asc };
        if (active)
            return await _repository.GetListAsync(c => c.Active == active, order);
        else
            return await _repository.GetListAsync(order);
    }


    public virtual async Task<ServiceResult> CreateAsync(T model, SysCustomUser user)
    {
        try
        {
            var duplicationResult = await IsDuplicated(model, CrudType.Create);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            await _repository.InsertAsync(model, user);

            return await SaveAsync(model, user, CrudType.Create, false);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }
    public async Task<ServiceResult> UpdateAsync(T model, SysCustomUser user)
    {
        try
        {
            var duplicationResult = await IsDuplicated(model, CrudType.Update);
            if (duplicationResult.IsDuplicated)
                return GetDuplicateMessage(user.Language, duplicationResult);

            if (await IsLockedAsync(model.Id))
                return GetLockedResult(user.Language);

            _repository.Update(model, user);

            return await SaveAsync(model, user, CrudType.Update);
        }
        catch (Exception ex)
        {
            return GetFailedResult(user.Language);
        }
    }
    public async Task<ServiceResult> PermanentDeleteAsync(SysCustomUser user, BT id)
    {
        try
        {
            var model = await GetAsync(id);
            if (model.Locked)
                return GetLockedResult(user.Language);
            _repository.Delete(model);
            return await SaveAsync(model, user, CrudType.Delete);
        }
        catch (DbUpdateException)
        {
            return GetFailedDeleteResult(user.Language);
        }
        catch
        {
            return GetFailedResult(user.Language);
        }
    }
    public async Task<ServiceResult> SoftDeleteAsync(SysCustomUser user, BT id)
    {
        var model = await GetAsync(id);
        if (model.Locked)
            return GetLockedResult(user.Language);

        var UpdatableFields = new List<Expression<Func<T, object>>>() { c => c.IsDeleted };
        _repository.SoftDelete(model, user);
        return await SaveAsync(model, user, CrudType.SoftDelete, UpdatableFields);
    }

    public async Task<ServiceResult> RollbackSoftDeleteAsync(SysCustomUser user, BT id)
    {
        var model = await GetAsync(id);
        if (model.Locked)
            return GetLockedResult(user.Language);

        var UpdatableFields = new List<Expression<Func<T, object>>>() { c => c.IsDeleted };
        _repository.RollbackSoftDelete(model, user);
        return await SaveAsync(model, user, CrudType.RollBackSoftDelete, UpdatableFields);
    }
    public virtual async Task<ServiceResult> ChangeActivateAsync(BT id, bool active, SysCustomUser user)
    {
        var model = Activator.CreateInstance(typeof(T)) as T;
        model.Id = id;
        model.Active = active;

        var UpdatableFields = new List<Expression<Func<T, object>>>() { c => c.Active };
        _repository.Update(model, UpdatableFields, user);
        if (active)
            return await SaveAsync(model, user, CrudType.Active, UpdatableFields);
        else
            return await SaveAsync(model, user, CrudType.Inactive, UpdatableFields);
    }

    public async Task<ServiceResult> LockAsync(SysCustomUser user, BT id) => await ChangeLockAsync(id, true, user);
    public async Task<ServiceResult> UnlockAsync(SysCustomUser user, BT id) => await ChangeLockAsync(id, false, user);
    public async Task<ServiceResult> ChangeLockAsync(BT id, bool locked, SysCustomUser user)
    {
        var model = Activator.CreateInstance(typeof(T)) as T;
        model.Id = id;
        model.Locked = locked;

        var UpdatableFields = new List<Expression<Func<T, object>>>() { c => c.Locked };
        _repository.Update(model, UpdatableFields, user);
        if (locked)
            return await SaveAsync(model, user, CrudType.Lock, UpdatableFields);
        else
            return await SaveAsync(model, user, CrudType.Unlock, UpdatableFields);
    }

    public async Task<bool> Any(Expression<Func<T, bool>> predicate) => await _repository.Any(predicate);
    public async Task<T> FirstAsync(Expression<Func<T, bool>> predicate) => await _repository.FirstAsync(predicate);

    public async Task<bool> IsLockedAsync(BT id)
    {
        return (await _repository.FindAsync(id)).Locked;
    }
    public async Task logActionAsync(T model, SysCustomUser user, CrudType crudType, List<Expression<Func<T, object>>> expressions = null)
    {
        JsonSerializerSettings jsSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        SysLog log = new SysLog();
        log.Id = Guid.NewGuid();
        log.EntityName = typeof(T).Name;
        log.EntityId = model.Id.ToString();
        log.CrudType = crudType;

        if (expressions != null)
        {
            JObject logObject = new JObject
            {
                [nameof(model.Id)] = model.Id.ToString(),
            };
            foreach (var expression in expressions)
            {
                var propName = string.Empty;
                if (expression.Body is MemberExpression)
                    propName = ((MemberExpression)expression.Body).Member.Name;
                else if (expression.Body is UnaryExpression)
                    propName = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member.Name;

                if (!propName.IsNullOrEmpty())
                    logObject[propName] = model.GetType().GetProperty(propName)?.GetValue(model, null)?.ToString();
            }

            log.Entity = logObject.ToString();
        }
        else
            log.Entity = JsonConvert.SerializeObject(model, Formatting.None, jsSettings);


        await _logRepository.InsertAsync(log, user);
        _unitOfWork.SaveChanges();
    }
    public async Task<ServiceResult> SaveAsync(T model, SysCustomUser user, CrudType crudType, bool doLog = true)
    {
        if (_unitOfWork.SaveChanges() > 0)
        {
            try
            {
                if (doLog)
                    await logActionAsync(model, user, crudType);
            }
            catch (Exception ex1)
            {
                //Todo: should handle in someway
            }
            return GetSuccessResult(user.Language);
        }
        else
            return GetFailedResult(user.Language);
    }

    public async Task<ServiceResult> SaveAsync(T model, SysCustomUser user, CrudType crudType, List<Expression<Func<T, object>>> properties, bool doLog = true)
    {
        if (_unitOfWork.SaveChanges() > 0)
        {
            try
            {
                if (doLog)
                    await logActionAsync(model, user, crudType, properties);
            }
            catch (Exception ex1)
            {
                //Todo: should handle in someway
            }
            return GetSuccessResult(user.Language);
        }
        else
            return GetFailedResult(user.Language);
    }

    // CustomActionResult
    public ServiceResult GetFailedWithRefResult(Language language, string refCode)
    {
        return new ServiceResult
        {
            Type = ServiceResultType.Failed,
            ExpressionType = ExpressionTypes.BaseEntity,
            ExpressionItem = BaseEntityExpression.Failed_Message_With_Ref.ToInt(),
            Language = language,
            Parameters = new Dictionary<string, string> { { ServiceResultParameterType.RefrenceCode, refCode } }
        };
    }
    public ServiceResult GetFailedResult(Language language)
    {
        return new ServiceResult
        {
            Type = ServiceResultType.Failed,
            ExpressionType = ExpressionTypes.BaseEntity,
            ExpressionItem = BaseEntityExpression.Failed_Message.ToInt(),
            Language = language,
        };
    }

    public ServiceResult GetFailedDeleteResult(Language language)
    {
        return new ServiceResult
        {
            Type = ServiceResultType.Failed,
            ExpressionType = ExpressionTypes.BaseEntity,
            ExpressionItem = BaseEntityExpression.Failed_Delete_Message.ToInt(),
            Language = language,
        };
    }

    public ServiceResult GetSuccessResult(Language language)
    {
        return new ServiceResult
        {
            Type = ServiceResultType.Succeed,
            ExpressionType = ExpressionTypes.BaseEntity,
            ExpressionItem = BaseEntityExpression.Success_Message.ToInt(),
            Language = language,
        };
    }
    public ServiceResult GetLockedResult(Language language)
    {
        return new ServiceResult
        {
            Type = ServiceResultType.Failed,
            ExpressionType = ExpressionTypes.BaseEntity,
            ExpressionItem = BaseEntityExpression.Locked_Validation_Message.ToInt(),
            Language = language,
        };
    }

    public ServiceResult GetDuplicateMessage(Language language, DuplicationResult duplicationResult)
    {
        List<string> fieldNames = new List<string>();
        foreach (var field in duplicationResult.DuplicatedFields)
            fieldNames.Add(SysExpressionHelper.Get(language, duplicationResult.ExpressionType, field));

        return new ServiceResult
        {
            Type = ServiceResultType.Failed,
            ExpressionType = ExpressionTypes.BaseEntity,
            ExpressionItem = BaseEntityExpression.Duplicate_Validation_Message.ToInt(),
            Language = language,
            Parameters = new Dictionary<string, string>
                         {
                              { ServiceResultParameterType.DuplicatedFieldName , string.Join(", ", fieldNames) },
                         }
        };
    }

    // virtuals
    public virtual Expression<Func<T, bool>> MakePredicate(BaseFilter filter, string sourceTimeZoneId)
    {
        if (string.IsNullOrEmpty(sourceTimeZoneId))
            throw new ArgumentNullException(nameof(sourceTimeZoneId));
        else
        {
            var predicate = PredicateBuilder.New<T>(true);

            if (filter.Active != null)
                predicate = predicate.And(c => c.Active == filter.Active);
            if (filter.Locked != null)
                predicate = predicate.And(c => c.Locked == filter.Locked);

            if (filter.CreatedAtStart != null && filter.CreatedAtStart != new DateTime())
                predicate = predicate.And(c => c.CreatedAt >= filter.CreatedAtStart.GetUtcDateTime(sourceTimeZoneId));
            if (filter.CreatedAtEnd != null && filter.CreatedAtEnd != new DateTime())
                predicate = predicate.And(c => c.CreatedAt < filter.CreatedAtEnd.GetUtcDateTime(sourceTimeZoneId).Value.AddDays(1));

            if (filter.UpdatedAtStart != null && filter.UpdatedAtStart != new DateTime())
                predicate = predicate.And(c => c.CreatedAt >= filter.UpdatedAtStart.GetUtcDateTime(sourceTimeZoneId));
            if (filter.UpdatedAtEnd != null && filter.UpdatedAtEnd != new DateTime())
                predicate = predicate.And(c => c.CreatedAt < filter.UpdatedAtEnd.Value.GetUtcDateTime(sourceTimeZoneId).AddDays(1));

            return predicate;
        }
    }

    public virtual async Task<DuplicationResult> IsDuplicated(T model, CrudType ctype)
    {
        if (ctype == CrudType.Create && await _repository.FindAsync(model.Id) != null)
            return new DuplicationResult(ExpressionTypes.BaseEntity, new() { BaseEntityExpression.Id.ToInt() });

        return new DuplicationResult();
    }

    public virtual List<string> GetIncludes() => new List<string>();

    public async Task<PagedList<DTO>> GetAllPagedAsync<DTO>(
        BaseFilter filter,
        DataTablePagination<T> pagination,
        Expression<Func<T, DTO>> select,
        string sourceTimeZoneId,
        Language language)
        where DTO : class, new()
    {
        var predicate = MakePredicate(filter, sourceTimeZoneId);
        if (pagination.Search is not null)
            predicate = predicate.And(pagination.Search);
        var data = await _repository.GetListAsync(predicate: predicate, orderBy: pagination.OrderBy, pagination: pagination, select);

        return data;
    }
}