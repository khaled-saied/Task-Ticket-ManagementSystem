using DAL.Models;

namespace DAL.Repositories
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllActive();
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<int> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TKey id);
    }
}

