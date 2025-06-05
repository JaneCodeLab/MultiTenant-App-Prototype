using ApplicationCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class Extensions
    {
        public static IQueryable<T> GetOrdered<T>(this DbSet<T> source, OrderBy<T> orderBy) where T : class
        {
            if (orderBy.OrderType == OrderType.Desc)
            {
                var query = source.OrderByDescending(orderBy.Orders.First());
                foreach (var order in orderBy.Orders.Skip(1))
                    query = query.ThenByDescending(order);
                return query;
            }
            else
            {
                var query = source.OrderBy(orderBy.Orders.First());
                foreach (var order in orderBy.Orders.Skip(1))
                    query = query.ThenBy(order);
                return query;
            }
        }
        public static IQueryable<T> GetOrdered<T>(this IQueryable<T> source, OrderBy<T> orderBy) where T : class
        {
            if (orderBy.OrderType == OrderType.Desc)
            {
                var query = source.OrderByDescending(orderBy.Orders.First());
                foreach (var order in orderBy.Orders.Skip(1))
                    query = query.ThenByDescending(order);
                return query;
            }
            else
            {
                var query = source.OrderBy(orderBy.Orders.First());
                foreach (var order in orderBy.Orders.Skip(1))
                    query = query.ThenBy(order);
                return query;
            }
        }
        public static IQueryable<T> GetPaged<T>(this DbSet<T> source, Pagination pagination) where T : class
        {
            return source.Skip(pagination.SkipCount).Take(pagination.Size);
        }
        public static IQueryable<T> GetPaged<T>(this IQueryable<T> source, Pagination pagination) where T : class
        {
            return source.Skip(pagination.SkipCount).Take(pagination.Size);
        }
        public static IQueryable<T> Include<T>(this DbSet<T> source, List<string> includes) where T : class
        {
            IQueryable<T> query = source;
            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, List<string> includes) where T : class
        {
            foreach (var include in includes)
                source = source.Include(include);
            
            return source;
        }
    }
}
