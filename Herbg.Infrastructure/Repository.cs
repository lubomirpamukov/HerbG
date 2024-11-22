using Herbg.Data;
using Herbg.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbg.Infrastructure;

public class Repository<T> : IRepositroy<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context, DbSet<T>dbSet)
    {
        _context = context;
        _dbSet = dbSet;
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

    public async Task<bool> AnyAsync(Func<T, bool> predicate)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        return await _dbSet.AnyAsync(x => predicate(x));
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "can't be null");
        };

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities), "can't be null");
        };

        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<T?> FindByIdAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "can't be null");
        };

        return  entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public IQueryable<T> GetAllAttachedAsync()
    {
        return  _dbSet.AsQueryable();
    }

    public Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SoftDeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SoftDeleteRangeAsync(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "can't be null");
        };

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
