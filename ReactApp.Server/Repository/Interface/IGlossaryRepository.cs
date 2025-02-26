using Microsoft.Data.SqlClient;
using ReactApp.Server.Entity;
using System.Net.Http.Headers;

namespace ReactApp.Server.Repository.Interface
{
    public interface IGlossaryRepository
    {
        public Task<Glossary> GetGlossaryByIdAsync(Guid guid, CancellationToken cancellationToken = default);
        public Task<bool> AddAsync(Glossary glossary, CancellationToken cancellationToken = default);
        public Task<bool> UpdateAsync(Glossary glossary, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Glossary>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<int> GetCountAsync(CancellationToken cancellationToken = default);
        public Task<int> GetSearchCountAsync(string search, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Glossary>> SearchByTermAsync(string search, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Glossary>> GetGlossariesPagedAsync(int startIndex, int count, CancellationToken cancellationToken = default);
        public Task<(IEnumerable<Glossary> Items, int TotalCount)> SearchGlossariesPagedAsync(string search, int startIndex, int count, CancellationToken cancellationToken = default);
    }
}
