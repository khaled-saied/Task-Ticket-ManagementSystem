using DAL.Data.DbContexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity, TKey>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        //Get all
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public IQueryable<TEntity> GetAllActive()
        {
            return _dbContext.Set<TEntity>().Where(e => !e.IsDeleted);
        }

        //Get by id
        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        //Add
        public async Task<int> AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }

        //Update
        public async Task<int> UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        //Delete
        public async Task<int> DeleteAsync(TKey id)
        {
            _dbContext.Set<TEntity>().Remove(await GetByIdAsync(id));
            return await _dbContext.SaveChangesAsync();
        }

    }
}

