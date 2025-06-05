
using ApplicationCore;
using ApplicationCore.DomainModel;
using ApplicationCore.DomainService;
using ApplicationCore.IRepository.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.SqlServerAdapter;

public partial class Repository<T, BT> : IRepository<T, BT> where T : BaseEntity<BT>
{
    public readonly DbContext _dbContext;
    public readonly DbSet<T> _dbSet;

    public Repository(DbContext dbContext)
    {
        if (dbContext == null)
            throw new ArgumentNullException("dbContext can not be null");

        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public T Insert(T entity, SysCustomUser user)
    {
        entity.SetCreatedFileds(user);
        return _dbSet.Add(entity).Entity;
    }
    public async Task<T> InsertAsync(T entity, SysCustomUser user)
    {
        entity.SetCreatedFileds(user);
        return (await _dbSet.AddAsync(entity)).Entity;
    }

    public T Update(T entity, SysCustomUser user)
    {
        entity.SetUpdatedFileds(user);
        return _dbSet.Update(entity).Entity;
    }
    public T Update(T entity, List<Expression<Func<T, object>>> properties, SysCustomUser user)
    {
        _dbContext.Entry(entity).State = EntityState.Unchanged;

        entity.SetUpdatedFileds(user);

        _dbContext.Entry(entity).Property(nameof(entity.UpdatedAt)).IsModified = true;
        _dbContext.Entry(entity).Property(nameof(entity.UpdatedBy)).IsModified = true;
        _dbContext.Entry(entity).Property(nameof(entity.UpdatedById)).IsModified = true;

        foreach (var property in properties)
            _dbContext.Entry(entity).Property(property).IsModified = true;

        return entity;
    }
    public void UpdateRange(ICollection<T> entities, SysCustomUser user)
    {
        foreach (var entity in entities)
            entity.SetUpdatedFileds(user);

        _dbSet.UpdateRange(entities);
    }

    public async Task DeleteAsync(BT id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
            Delete(entity);
    }
    public void Delete(T entity)
    {
        var dbEntityEntry = _dbContext.Entry(entity);

        if (dbEntityEntry.State != EntityState.Detached)
            dbEntityEntry.State = EntityState.Deleted;
        else
        {
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
        }
    }
    public void DeleteRange(List<T> entity) => _dbSet.RemoveRange(entity);

    public T SoftDelete(T entity, SysCustomUser user)
    {
        entity.IsDeleted = true;
        return Update(entity, new List<Expression<Func<T, object>>> { c => c.IsDeleted }, user);
    }
    public T RollbackSoftDelete(T entity, SysCustomUser user)
    {
        entity.IsDeleted = false;
        return Update(entity, new List<Expression<Func<T, object>>> { c => c.IsDeleted }, user);
    }
    public async Task SoftDeleteAsync(BT id, SysCustomUser user)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
            SoftDelete(entity, user);
    }

    public async Task<int> CountAsync() => await _dbSet.CountAsync();
    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) => await _dbSet.CountAsync(predicate);

    public async Task<bool> Any(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

    public object? Max(Func<T, object> field) => _dbSet.Max(field);
    public object? Max(Func<T, object> field, Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).Max(field);

    public object? Min(Func<T, object> field) => _dbSet.Min(field);
    public object? Min(Func<T, object> field, Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).Min(field);

    public decimal Sum(Func<T, decimal> field) => _dbSet.Select(field).Sum();
    public decimal Sum(Func<T, decimal> field, Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).Select(field).Sum();

    public ICollection<T> GetList() => _dbSet.ToList();
    public async Task<ICollection<T>> GetListAsync() => await _dbSet.ToListAsync();


    public ICollection<TType> GetList<TType>(Expression<Func<T, TType>> select) where TType : class
    {
        return _dbSet.Select(select).ToList();
    }

    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Select(select).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.GetOrdered(orderBy).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Include(includes).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Include(includes).Select(select).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Include(includes).GetOrdered(orderBy).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Include(includes).GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Select(select).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).GetOrdered(orderBy).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Include(includes).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Include(includes).Select(select).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).Select(select).ToListAsync();
    }
    public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
    }

    public async Task<ICollection<T>> GetListAsync(Pagination pagination)
    {
        return await _dbSet.GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(OrderBy<T> orderBy)
    {
        return await _dbSet.GetOrdered(orderBy).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(OrderBy<T> orderBy, Pagination pagination)
    {
        return await _dbSet.GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(List<string> includes)
    {
        return await _dbSet.Include(includes).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(List<string> includes, Pagination pagination)
    {
        return await _dbSet.Include(includes).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(List<string> includes, OrderBy<T> orderBy)
    {
        return await _dbSet.Include(includes).GetOrdered(orderBy).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(List<string> includes, OrderBy<T> orderBy, Pagination pagination)
    {
        return await _dbSet.Include(includes).GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsQueryable<T>().Where(predicate).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, Pagination pagination)
    {
        return await _dbSet.Where(predicate).GetPaged(pagination).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy)
    {
        return await _dbSet.Where(predicate).GetOrdered(orderBy).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes)
    {
        return await _dbSet.Where(predicate).Include(includes).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy)
    {
        return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).ToListAsync();
    }
    public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Pagination pagination)
    {
        return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
    }
    
    public async Task<PagedList<T>> GetListAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Pagination pagination)
    {
        var queriable = _dbSet.Where(predicate).GetOrdered(orderBy);

        var count = await queriable.CountAsync();

        if (!pagination.TakeAll)
            queriable = queriable.GetPaged(pagination);

        var list = await queriable.ToListAsync();

        return new PagedList<T>(list, count, pagination.Page, pagination.Size);
    }
    public async Task<PagedList<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, Pagination pagination)
    {
        var queriable = _dbSet.Where(predicate).Include(includes);

        var count = await queriable.CountAsync();
        var list = await queriable.GetPaged(pagination).ToListAsync();


        return new PagedList<T>(list, count, pagination.Page, pagination.Size);

    }
    public async Task<PagedList<DTO>> GetListAsync<DTO>(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, DTO>> select) where DTO : class
    {

        var queriable = _dbSet.Where(predicate).GetOrdered(orderBy);

        var count = await queriable.CountAsync();

        if (!pagination.TakeAll)
            queriable = queriable.GetPaged(pagination);

        var list = await queriable.Select(select).ToListAsync();

        return new PagedList<DTO>(list, count, pagination.Page, pagination.Size);
    }

    public async Task<T?> FindAsync(BT id, bool detach = true)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, bool detach = true)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, bool detach = true)
    {
        var entity = await _dbSet.GetOrdered(orderBy).FirstOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, List<string> includes, bool detach = true)
    {
        var entity = await _dbSet.Include(includes).FirstOrDefaultAsync(predicate); 
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, bool detach = true)
    {
        var entity = await _dbSet.Include(includes).GetOrdered(orderBy).FirstOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public async Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Where(predicate).Select(select).FirstOrDefaultAsync();
    }
    public async Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select, OrderBy<T> orderBy) where TType : class
    {
        return await _dbSet.Where(predicate).GetOrdered(orderBy).Select(select).FirstOrDefaultAsync();
    }
    public async Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select) where TType : class
    {
        return await _dbSet.Include(includes).Where(predicate).Select(select).FirstOrDefaultAsync();
    }
    public async Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select, OrderBy<T> orderBy) where TType : class
    {
        return await _dbSet.Include(includes).Where(predicate).GetOrdered(orderBy).Select(select).FirstOrDefaultAsync();
    }

    public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, bool detach = true)
    {
        var entity = await _dbSet.LastOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, bool detach = true)
    {
        var entity = await _dbSet.GetOrdered(orderBy).LastOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, List<string> includes, bool detach = true)
    {
        var entity = await _dbSet.Include(includes).LastOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }
    public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, bool detach = true)
    {
        var entity = await _dbSet.Include(includes).GetOrdered(orderBy).LastOrDefaultAsync(predicate);
        if (entity != null && detach)
            _dbContext.Entry(entity).State = EntityState.Detached;
        return entity;
    }

    /// <summary>
    /// Returns Grouped List
    /// </summary>
    /// <typeparam name="Key">Group by Key</typeparam>
    /// <typeparam name="Result">Result Object Type</typeparam>
    /// <param name="groupingKey"></param>
    /// <param name="resultSelector"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<Result> GetGrouped<Key, Result>(Expression<Func<T, Key>> groupingKey, Expression<Func<IGrouping<Key, T>, Result>> resultSelector, Expression<Func<T, bool>>? filter = null)
    {
        var query = _dbSet.AsQueryable();
        if (filter != null)
            query = query.Where(filter);

        return query.GroupBy(groupingKey).Select(resultSelector);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
