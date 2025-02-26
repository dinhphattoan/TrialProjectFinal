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

namespace ReactApp.Server.Repository
{
    public class SQLGlossaryRepository : IGlossaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<SQLGlossaryRepository> _logger;

        public SQLGlossaryRepository(ApplicationDbContext dbContext, ILogger<SQLGlossaryRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CheckTermOfPhraseExist(Glossary glossary, CancellationToken cancellationToken)
        {
            return await _dbContext.Glossaries.AnyAsync(g => g.TermOfPhrase == glossary.TermOfPhrase, cancellationToken);
        }

        public async Task<bool> AddAsync(Glossary glossary, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Glossaries.AddAsync(glossary);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error occurred while adding new glossary: {glossary.TermOfPhrase}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Glossary glossary, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Glossaries.Update(glossary);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error occurred while updating glossary: {glossary.TermOfPhrase}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Glossary? removeGlossary = await _dbContext.Glossaries.FindAsync(id, cancellationToken);

                if (removeGlossary == null)
                {
                    return false;
                }

                _dbContext.Glossaries.Remove(removeGlossary);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Database error occurred while deleting glossary. {id}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting glossary in DeleteAsync. {id}");
                return false;
            }
        }

        public async Task<IEnumerable<Glossary>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Glossaries.ToListAsync(cancellationToken);
        }

        public async Task<Glossary?> GetGlossaryByIdAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Glossaries.FindAsync(guid, cancellationToken);
        }

        public async Task<IEnumerable<Glossary>> SearchByTermAsync(string searchString, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Glossaries
                .Where(g => g.TermOfPhrase.ToLower().Contains(searchString.ToLower()))
                .OrderBy(g=>g.TermOfPhrase)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Glossary>> GetGlossariesPagedAsync(int startIndex, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Glossaries
                .OrderBy(g => g.TermOfPhrase)
                .Skip(startIndex)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Glossary>> SearchGlossariesPagedAsync(string search, int startIndex, int count, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Glossaries
                .Where(p=>p.TermOfPhrase.ToLower().Contains(search.ToLower()))
                .Skip(startIndex)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Glossaries.CountAsync(cancellationToken);
        }
    }
}