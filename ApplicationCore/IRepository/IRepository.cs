
using ApplicationCore.DomainModel;
using ApplicationCore.IRepository.Model;
using System.Linq.Expressions;

namespace ApplicationCore;

public interface IRepository<T, BT> : IDisposable where T : BaseEntity<BT>
{
    T Insert(T entity, SysCustomUser user);
    Task<T> InsertAsync(T entity, SysCustomUser user);
    T Update(T entity, SysCustomUser user);
    T Update(T entity, List<Expression<Func<T, object>>> properties, SysCustomUser user);
    void UpdateRange(ICollection<T> entities, SysCustomUser user);
    Task DeleteAsync(BT id);
    void Delete(T entity);
    void DeleteRange(List<T> entity);
    T SoftDelete(T entity, SysCustomUser user);
    T RollbackSoftDelete(T entity, SysCustomUser user);
    Task SoftDeleteAsync(BT id, SysCustomUser user);
    Task<bool> Any(Expression<Func<T, bool>> predicate);

    Task<T?> FindAsync(BT id, bool detach = true);
    Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, bool detach = true);
    Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, bool detach = true);
    Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, List<string> includes, bool detach = true);
    Task<T?> FirstAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, bool detach = true);
    Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class;
    Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select, OrderBy<T> orderBy) where TType : class;
    Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select) where TType : class;
    Task<TType?> FirstAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select, OrderBy<T> orderBy) where TType : class;

    Task<T?> LastAsync(Expression<Func<T, bool>> predicate, bool detach = true);
    Task<T?> LastAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, bool detach = true);
    Task<T?> LastAsync(Expression<Func<T, bool>> predicate, List<string> includes, bool detach = true);
    Task<T?> LastAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, bool detach = true);

    IEnumerable<Result> GetGrouped<Key, Result>(Expression<Func<T, Key>> groupingKey, Expression<Func<IGrouping<Key, T>, Result>> resultSelector, Expression<Func<T, bool>>? filter = null);

    ICollection<T> GetList();
    ICollection<TType> GetList<TType>(Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync();
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(OrderBy<T> orderBy);
    Task<ICollection<TType>> GetListAsync<TType>(OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(OrderBy<T> orderBy, Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(List<string> includes);
    Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(List<string> includes, Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(List<string> includes, OrderBy<T> orderBy);
    Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(List<string> includes, OrderBy<T> orderBy, Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(List<string> includes, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class;
    Task<PagedList<T>> GetListAsync(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Pagination pagination);
    Task<PagedList<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Expression<Func<T, TType>> select) where TType : class;
    Task<PagedList<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Expression<Func<T, TType>> select) where TType : class;
    Task<ICollection<T>> GetListAsync(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Pagination pagination);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<T, bool>> predicate, List<string> includes, OrderBy<T> orderBy, Pagination pagination, Expression<Func<T, TType>> select) where TType : class;

    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    object? Min(Func<T, object> field);
    object? Min(Func<T, object> field, Expression<Func<T, bool>> predicate);
    object? Max(Func<T, object> field);
    object? Max(Func<T, object> field, Expression<Func<T, bool>> predicate);
    decimal Sum(Func<T, decimal> field);
    decimal Sum(Func<T, decimal> field, Expression<Func<T, bool>> predicate);
}