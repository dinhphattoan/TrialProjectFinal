using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactApp.Server.Contracts.DTOs
{
    public class PaginatedResultDto<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => PageItems == null || !PageItems.Any() ?
                    1 :
                    (int)Math.Ceiling(TotalItems * 1.0 / PageSize);
        public IEnumerable<T> PageItems { get; set; }
    }
}
