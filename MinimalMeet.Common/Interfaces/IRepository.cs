using MinimalMeet.Common.Entities;

namespace MinimalMeet.Common.Interfaces;

public interface IRepository<TEntity, TEntityId>
    where TEntity : EntityBase<TEntityId>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(TEntityId id);
    Task<List<TEntity>> GetAllAsync();
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntityId id);
}
