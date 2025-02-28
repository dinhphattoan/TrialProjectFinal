using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactApp.Server.Contracts.DTOs
{
    public class PagingDto
    {
        public int? Page { get; set; } = default;
        public int? PageSize { get; set; } = default;
    }
}
