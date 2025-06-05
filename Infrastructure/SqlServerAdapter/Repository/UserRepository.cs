
using ApplicationCore;
using ApplicationCore.DomainModel;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel;
using System.Linq.Expressions;

namespace Infrastructure.SqlServerAdapter
{
    public class UserRepository : IUserRepository
    {
        public readonly DbContext _dbContext;
        public readonly DbSet<SysCustomUser> _dbSet;

        public UserRepository(DbContext dbContext)
        {
            if (dbContext == null || dbContext is not ApplicationDbContext)
                throw new ArgumentNullException("dbContext can not be null");

            _dbContext = dbContext;
            _dbSet = _dbContext.Set<SysCustomUser>();
        }

        public async Task<SysCustomUser> InsertAsync(SysCustomUser entity, string user)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.CreatedBy = user;
            return (await _dbSet.AddAsync(entity)).Entity;
        }
        public SysCustomUser Update(SysCustomUser entity, string user)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = user;
            return _dbSet.Update(entity).Entity;
        }
        public async Task UpdateAsync(SysCustomUser entity, string user)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = user;
            _dbContext.Entry(entity).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }


        public async Task Delete(string id)
        {
            var entity = await FindAsync(id);
            if (entity != null)
                Delete(entity);
        }
        public void Delete(SysCustomUser entity)
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


        public async Task<SysCustomUser> FindAsync(string id, bool detach = false)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null && detach)
                _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public bool Exists(Expression<Func<SysCustomUser, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
        public async Task<ICollection<SysCustomUser>> GetListAsync()
        {
            return await _dbSet.AsQueryable<SysCustomUser>().ToListAsync();
        }
        public async Task<ICollection<SysCustomUser>> GetListAsync(Expression<Func<SysCustomUser, bool>> predicate)
        {
            return await _dbSet.AsQueryable<SysCustomUser>().Where(predicate).ToListAsync();
        }
        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<SysCustomUser, bool>> predicate, Expression<Func<SysCustomUser, TType>> select) where TType : class
        {
            return await _dbSet.Where(predicate).Select(select).ToListAsync();
        }
        public async Task<ICollection<TType>> GetListAsync<TType>(Expression<Func<SysCustomUser, bool>> predicate, Expression<Func<SysCustomUser, TType>> select, OrderBy<SysCustomUser> orderBy) where TType : class
        {
            return await _dbSet.Where(predicate).GetOrdered(orderBy).Select(select).ToListAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
