
using System.Linq.Expressions;

namespace ApplicationCore
{
    public class OrderBy<T>
    {
        public List<Expression<Func<T, object>>> Orders = new List<Expression<Func<T, object>>>();
        public OrderType OrderType { get; set; }
    }
}
