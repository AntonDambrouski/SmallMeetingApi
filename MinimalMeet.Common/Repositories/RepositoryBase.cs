using Microsoft.EntityFrameworkCore;
using MinimalMeet.Common.Entities;
using MinimalMeet.Common.Interfaces;

namespace MinimalMeet.Common.Repositories;

public class RepositoryBase<TEntity, TEntityId, TContext> : IRepository<TEntity, TEntityId>
    where TEntity : EntityBase<TEntityId>
    where TEntityId : struct
    where TContext : DbContext
{
    protected readonly TContext _context;

    protected RepositoryBase(TContext context)
    {
        _context = context;
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
      return await _context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id)
    {
        return await _context.Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync();

        return entity;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(TEntityId id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            return await SaveChangesAsync();
        }

        return false;
    }

    protected async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
