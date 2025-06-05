
using ApplicationCore;
using ApplicationCore.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SqlServerAdapter
{
    public class UnitOfWork<CT> : IUnitOfWork<CT> where CT : DbContext
    {
        private bool disposed = false;
        private readonly CT _dbContext;

        public UnitOfWork(CT dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException($"{nameof(dbContext)} can not be null");

            _dbContext = dbContext;
        }

        public IRepository<T, BT> GetRepository<T, BT>() where T : BaseEntity<BT>
        {
            return new Repository<T, BT>(_dbContext);
        }

        public IUserRepository GetUserRepository()
        {
            if (_dbContext is ApplicationDbContext)
                return new UserRepository(_dbContext);

            return default;
        }

        public ICompositeRepository<T> GetCompositeRepository<T>() where T : class
        {
            return new CompositeRepository<T>(_dbContext);
        }

        public int SaveChanges()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //TODO : handle exceptions
                throw;
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
