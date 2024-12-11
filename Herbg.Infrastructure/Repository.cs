using Herbg.Data;
using Herbg.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Herbg.Infrastructure;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<bool> AddAsync(T entity)
    {
        if (entity == null) 
        {
            throw new ArgumentNullException(nameof(entity), "can't be null");
        };
      
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null || entities.Count() == 0)
        {
            throw new ArgumentNullException(nameof(entities), "can't add empty array of entities");
        };

        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return true;
    }

	public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        if (entity == null)
        {
            return false;
        };

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null)
        {
            return false;
        };

        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<T?> FindByConditionAsync(Expression<Func<T, bool>> predicate)
    {
        T? entity = await this._dbSet.FirstOrDefaultAsync(predicate);
        return entity;
    }

    public async Task<T?> FindByIdAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);


        return  entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public IQueryable<T> GetAllAttached()
    {
        return  _dbSet.AsQueryable();
    }

    public DbContext GetDbContext()
    {
        return (DbContext)_context;
    }


	public async Task<bool>SaveChangesAsync()
	{
        await _context.SaveChangesAsync();
        return true;
	}


    public async Task<bool> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            return false;
        };

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async void UpdateRange(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }
}
