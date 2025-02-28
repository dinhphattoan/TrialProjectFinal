using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactApp.Server.Contracts.Paginations
{
    public class PaginatedFilter<T> : AdvancedFilter<T> where T : class
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
