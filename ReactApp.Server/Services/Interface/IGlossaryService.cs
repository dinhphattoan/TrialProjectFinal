using Microsoft.AspNetCore.Identity;
using ReactApp.Server.DTO;
using ReactApp.Server.Entity;

namespace ReactApp.Server.Services.Interface
{
    public interface IGlossaryService
    {
        public Task<IEnumerable<Glossary>> GetGlossariesAsync(CancellationToken cancellationToken = default); //Order by term of phrase
        public Task<IEnumerable<Glossary>> GetGlossariesBySearchAsync(string search, CancellationToken cancellationToken = default); //Search by term of phrase
        public Task<GlossaryRecordResultDTO> GetGlossariesByRangeAsync(int startIndex, int count, string search="", CancellationToken cancellationToken = default); //Get glossaries by range
        public Task<int> @int(CancellationToken cancellationToken =default); //Count of glossaries
        public Task<bool> AddGlossariesAsync(GlossaryCreateDTO glossaryCreateDTO, IdentityUser identityUser, CancellationToken cancellation);
    }
}
