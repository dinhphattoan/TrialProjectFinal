using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Contracts.DTOs;
using ReactApp.Server.Contracts.Paginations;
using ReactApp.Server.Entity;
using System.Linq.Expressions;

namespace ReactApp.Server.Repository.Interface
{
    public interface IGenericRepository<T, TKey> where T : class, IEntity<TKey>
    {
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        Task<int> DeleteAsync(TKey Id, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<PaginatedResult<T>> GetPaginatedAsync(PaginatedFilter<T> paginatedFilter = null);
    }
}
