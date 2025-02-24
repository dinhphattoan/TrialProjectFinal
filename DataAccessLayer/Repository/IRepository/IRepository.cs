using ReactApp.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.IRepository
{
    internal interface IRepository
    {
        ApplicationDbContext DbContext { get; }

    }
}
