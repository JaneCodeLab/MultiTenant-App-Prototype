
using System.Linq.Expressions;

namespace ApplicationCore
{
    public class DataTablePagination<T>: Pagination {
        public int Draw { get; set; }
        public Expression<Func<T, bool>>? Search{ get; set; }

        public OrderBy<T>? OrderBy { get; set; }

        public List<string> Columns { get; set; }
    }
}
