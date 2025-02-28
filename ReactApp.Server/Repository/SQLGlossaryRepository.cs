using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using ReactApp.Server.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using ReactApp.Server.Repository.Interface;
using ReactApp.Server.Contracts.DTOs;
using ReactApp.Server.Contracts.Paginations;
using ReactApp.Server.DTO.Glossary;
using System.Linq.Expressions;

namespace ReactApp.Server.Repository
{
    public class SQLGlossaryRepository : GenericRepository<Glossary, Guid>,IGlossaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SQLGlossaryRepository> _logger;

        public SQLGlossaryRepository(ApplicationDbContext dbContext,
            ILogger<SQLGlossaryRepository> logger): 
            base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



    }
}