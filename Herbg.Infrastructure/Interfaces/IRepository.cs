using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Herbg.Infrastructure.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<IEnumerable<T>> GetAllAsync();

    public IQueryable<T> GetAllAttached();

    public Task<T?> FindByIdAsync(object id);

    public Task<bool> AddAsync(T entity);

    public Task<bool> AddRangeAsync(IEnumerable<T> entities);

    public Task<bool> UpdateAsync(T entity);

    public Task<bool> DeleteAsync(T entity);

    public Task<bool> DeleteRangeAsync(IEnumerable<T> entities);

    public Task<T?> FindByConditionAsync(Expression<Func<T, bool>> predicate);

    public Task<int> CountAsync();

    public Task<bool>SaveChangesAsync();

    DbContext GetDbContext();
}
