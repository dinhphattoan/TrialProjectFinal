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

namespace ReactApp.Server.Repository
{
    public class GlossaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GlossaryRepository> _logger;

        public GlossaryRepository(ApplicationDbContext dbContext, ILogger<GlossaryRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Glossary>> GetOrderedGlossaries()
        {
            try
            {
                if (_dbContext == null || _dbContext.Glossaries == null)
                {
                    _logger.LogError("DbContext or Glossaries is null.");
                    throw new InvalidOperationException("DbContext or Glossaries is null.");
                }

                return await _dbContext.Glossaries.AsNoTracking().OrderBy(g => g.TermOfPhrase).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving ordered glossaries.");
                throw;
            }
        }

        public async Task<IEnumerable<Glossary>> GetGlossaryByStringSearch(string search)
        {
            try
            {
                if (_dbContext == null || _dbContext.Glossaries == null)
                {
                    _logger.LogError("DbContext or Glossaries is null.");
                    throw new InvalidOperationException("DbContext or Glossaries is null.");
                }

                if (string.IsNullOrEmpty(search))
                {
                    return await _dbContext.Glossaries.AsNoTracking().ToListAsync();
                }

                return await _dbContext.Glossaries
                    .AsNoTracking()
                    .Where(g => EF.Functions.Like(g.TermOfPhrase, $"%{search}%"))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching glossaries with search term: {SearchTerm}.", search);
                return Enumerable.Empty<Glossary>();
            }

        }
        public async Task<int> GetTotalGlossaryAsync()
        {
            return await _dbContext.Glossaries.CountAsync();
        }
        public async Task<IEnumerable<Glossary>> GetGlossariesAtRangeOrderByTermOfPhrase(int startIndex, int length)
        {
            return await _dbContext.Glossaries
        .OrderBy(g => g.TermOfPhrase)
        .Skip(startIndex)
        .Take(length)
        .AsNoTracking().ToListAsync();
        }
        public IAsyncEnumerable<Glossary> GetTotalGlossaries()
        {
            return _dbContext.Glossaries.AsNoTracking().AsAsyncEnumerable();
        }
        
        public async Task<bool> AddNewGlossaryAsync(Glossary glossary)
        {
            try
            {
                await _dbContext.Glossaries.AddAsync(glossary);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error occurred while adding new glossary in AddNewGlossaryAsync.");
                return false;
            }
            
        }
        public async Task<bool> CheckTermOfPhraseExist(Glossary glossary)
        {
            return await _dbContext.Glossaries.AnyAsync(g => g.TermOfPhrase == glossary.TermOfPhrase);
        }
    }
}