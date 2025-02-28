using ReactApp.Server.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactApp.Server.Contracts.Paginations
{
    public class FilterDto : PagingDto
    {
        public string? Keyword { get; set; }
        public string? OrderBy { get; set; } 
        public bool? Desc { get; set; }
    }
}
