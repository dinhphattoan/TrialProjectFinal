using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Contracts.DTOs;
using ReactApp.Server.Contracts.Paginations;
using ReactApp.Server.Entity;
using System.Linq.Expressions;
using System.Threading;

namespace ReactApp.Server.Repository.Interface
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<PaginatedResult<T>> GetPaginatedAsync(PaginatedFilter<T> paginatedFilter)
        {
            var result = new PaginatedResult<T>();
            IQueryable<T> query = _dbSet;

            if (paginatedFilter?.Filter is not null)
            {
                query = _dbSet.Where(paginatedFilter.Filter).AsNoTracking();
            }
            var count = await query.CountAsync();
            if (paginatedFilter?.PageIndex is not null &&
                paginatedFilter?.PageSize is not null &&
                paginatedFilter?.PageIndex > 0 &&
                paginatedFilter?.PageSize > 0)
            {

                var startIndex = ((paginatedFilter.PageIndex - 1) < 0 ? 0 : (paginatedFilter.PageIndex - 1)) * paginatedFilter.PageSize ?? default;
                var items = await query.Skip(startIndex).Take(paginatedFilter.PageSize.Value).AsNoTracking().ToListAsync();
                result.Items = items;
                result.PageSize = paginatedFilter.PageSize.Value;
                result.PageIndex = paginatedFilter.PageIndex.Value;
                result.Count = count;
                return result;
            }
            result.PageIndex = 1;
            result.PageSize = count;
            result.Count = count;
            result.Items = await _dbSet.AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var addedEntityEntry = await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return addedEntityEntry.Entity;
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().AnyAsync(expression);
        }

        public async Task<int> DeleteAsync(TKey Id, CancellationToken cancellationToken = default)
        {
            var exist = await _dbSet.FindAsync(Id, cancellationToken);
            if (exist != null)
            {
                _dbSet.Remove(exist);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            var exist = await _dbSet.FindAsync(entity.Id, cancellationToken);
            if (exist != null)
            {
                _dbSet.Entry(exist).CurrentValues.SetValues(entity);
            }
            return await _context.SaveChangesAsync();
        }
    }
}
