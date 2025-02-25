using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using ReactApp.Server.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}