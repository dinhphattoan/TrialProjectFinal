using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactApp.Server.Contracts.DTOs.Glossaries
{
    public class UpdateGlossaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Explaination { get; set; }
    }
}
