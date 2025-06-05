
using ApplicationCore.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore;

public interface IUnitOfWork<CT> : IDisposable where CT : DbContext
{
    IRepository<T, BT> GetRepository<T, BT>() where T : BaseEntity<BT>;
    IUserRepository GetUserRepository();
    ICompositeRepository<T> GetCompositeRepository<T>() where T : class;
    int SaveChanges();
}