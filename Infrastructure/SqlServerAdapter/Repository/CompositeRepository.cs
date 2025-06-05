
using ApplicationCore;
using ApplicationCore.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.SqlServerAdapter
{
    public partial class CompositeRepository<T> : ICompositeRepository<T> where T : class
    {
        public readonly DbContext _dbContext;
        public readonly DbSet<T> _dbSet;

        public CompositeRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null");

            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public T Insert(T entity, SysCustomUser user)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task<T> InsertAsync(T entity, SysCustomUser user)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public T Update(T entity, SysCustomUser user)
        {
            return _dbSet.Update(entity).Entity;
        }

        public T Update(T entity, List<Expression<Func<T, object>>> properties, SysCustomUser user)
        {
            _dbContext.Entry(entity).State = EntityState.Unchanged;

            foreach (var property in properties)
                _dbContext.Entry(entity).Property(property).IsModified = true;

            return entity;
        }

        public void UpdateRange(List<T> entities, SysCustomUser user)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task DeleteAsync(params object[] id)
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

        public T SoftDelete(T entity, SysCustomUser user)
        {
            return _dbSet.Update(entity).Entity;
        }

        public async Task SoftDeleteAsync(SysCustomUser user, params object[] id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                SoftDelete(entity, user);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public object Max(Func<T, object> field)
        {
            return _dbSet.Max(field);
        }

        public object Max(Func<T, object> field, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Max(field);
        }

        public decimal Sum(Func<T, decimal> field)
        {
            return _dbSet.Select(field).Sum();
        }

        public decimal Sum(Func<T, decimal> field, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Select(field).Sum();
        }

        public object Min(Func<T, object> field)
        {
            return _dbSet.Min(field);
        }

        public object Min(Func<T, object> field, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Min(field);
        }

        public ICollection<T> GetList()
        {
            return _dbSet.ToList();
        }

        public ICollection<TType> GetList<TType>(Expression<Func<T, TType>> select) where TType : class
        {
            return _dbSet.Select(select).ToList();
        }

        public async Task<ICollection<T>> GetListAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Pagination pagination)
        {
            return await _dbSet.GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(OrderBy<T> orderBy)
        {
            return await _dbSet.GetOrdered(orderBy).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.GetOrdered(orderBy).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(OrderBy<T> orderBy, Pagination pagination)
        {
            return await _dbSet.GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(List<string> includes)
        {
            return await _dbSet.Include(includes).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Include(includes).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(List<string> includes, Pagination pagination)
        {
            return await _dbSet.Include(includes).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Include(includes).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(List<string> includes, OrderBy<T> orderBy)
        {
            return await _dbSet.Include(includes).GetOrdered(orderBy).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Include(includes).GetOrdered(orderBy).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(List<string> includes, OrderBy<T> orderBy, Pagination pagination)
        {
            return await _dbSet.Include(includes).GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Include(includes).GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsQueryable<T>().Where(predicate).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, Pagination pagination)
        {
            return await _dbSet.Where(predicate).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy)
        {
            return await _dbSet.Where(predicate).GetOrdered(orderBy).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).GetOrdered(orderBy).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Pagination pagination)
        {
            return await _dbSet.Where(predicate).GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes)
        {
            return await _dbSet.Where(predicate).Include(includes).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Include(includes).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, Pagination pagination)
        {
            return await _dbSet.Where(predicate).Include(includes).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Include(includes).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy)
        {
            return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).Select(select).ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Pagination pagination)
        {
            return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).GetPaged(pagination).ToListAsync();
        }

        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Include(includes).GetOrdered(orderBy).Select(select).GetPaged(pagination).ToListAsync();
        }

        public async Task<T?> FindAsync(params object[] id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, List<string> includes)
        {
            return await _dbSet.Include(includes).FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.LastOrDefaultAsync(predicate);
        }
        public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy)
        {
            return await _dbSet.GetOrdered(orderBy).LastOrDefaultAsync(predicate);
        }

        public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, List<string> includes)
        {
            return await _dbSet.Include(includes).LastOrDefaultAsync(predicate);
        }
        public async Task<T?> LastAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy)
        {
            return await _dbSet.Include(includes).GetOrdered(orderBy).LastOrDefaultAsync(predicate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
