using ReactApp.Server.Entity;
using ReactApp.Server.Repository.Interface;
using ReactApp.Server.Services.Interface;
using System.Data.Common;

namespace ReactApp.Server.Services
{
    public class GlossaryService : IGlossaryService
    {
        private readonly IGlossaryRepository _glossaryRepository;
        private readonly ILogger<GlossaryService> _logger;
        public GlossaryService(ILogger<GlossaryService> logger, IGlossaryRepository glossaryRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _glossaryRepository = glossaryRepository ?? throw new ArgumentNullException(nameof(glossaryRepository));
        }

        public async Task<IEnumerable<Glossary>> GetGlossariesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _glossaryRepository.GetAllAsync();
            }
            catch(DbException ex)
            {
                _logger.LogError(ex, "Database error occured while get all Glossaries in GetGlossariesAsync");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected Error occured while get all Glossaries in GetGlossariesAsync");
            }
            return Enumerable.Empty<Glossary>();
        }

        public async Task<IEnumerable<Glossary>> GetGlossariesByRange(int startIndex, int count, string search = "", CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    return await _glossaryRepository.GetGlossariesPagedAsync(startIndex, count,cancellationToken);
                    
                }
                else
                {
                    return await _glossaryRepository.SearchGlossariesPagedAsync(search,startIndex,count, cancellationToken);
                    
                }
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Database error occured while get all Glossaries in GetGlossariesByRange: {startIndex}, {count}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected Error occured while get all Glossaries in GetGlossariesByRange");
            }
            return Enumerable.Empty<Glossary>();
        }
        public async Task<IEnumerable<Glossary>> GetGlossariesBySearchAsync(string search,CancellationToken cancellationToken)
        {
            try
            {
                return await _glossaryRepository.SearchByTermAsync(search, cancellationToken);
                
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, $"Database error occured while get all Glossaries in GetGlossariesAsync: {search}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected Error occured while get all Glossaries in GetGlossariesAsync");
            }
            return Enumerable.Empty<Glossary>();
        }

        public Task<Glossary> GetServiceByIdAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Glossary>> GetServicesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> @int(CancellationToken cancellationToken)
        {
            try
            {
                return await _glossaryRepository.GetCountAsync(cancellationToken);
            }
            catch (DbException ex)
            {
                _logger.LogError(ex,"Error occured while get all count");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting glossary count.");
            }
            return 0;
        }
    }
}
