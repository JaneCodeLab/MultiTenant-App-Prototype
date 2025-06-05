using ApplicationCore.DomainModel;
using System.Linq.Expressions;

namespace ApplicationCore;

public interface IUserRepository : IDisposable
{
    Task<SysCustomUser> InsertAsync(SysCustomUser entity, string user);
    SysCustomUser Update(SysCustomUser entity, string user);
    Task UpdateAsync(SysCustomUser entity, string user);

    Task Delete(string id);
    void Delete(SysCustomUser entity);

    Task<SysCustomUser> FindAsync(string id, bool detach = false);
    bool Exists(Expression<Func<SysCustomUser, bool>> predicate);
    Task<ICollection<SysCustomUser>> GetListAsync();
    Task<ICollection<SysCustomUser>> GetListAsync(Expression<Func<SysCustomUser, bool>> predicate);
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<SysCustomUser, bool>> predicate, Expression<Func<SysCustomUser, TType>> select) where TType : class;
    Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<SysCustomUser, bool>> predicate, Expression<Func<SysCustomUser, TType>> select, OrderBy<SysCustomUser> orderBy) where TType : class;
}