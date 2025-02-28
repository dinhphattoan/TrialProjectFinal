using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ReactApp.Server.Contracts.Paginations
{
    public class AdvancedFilter<T>
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }
        public Expression<Func<T, T>> Select { get; set; }
    }
}
