using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.DbContexts;
using DAL.Models;

namespace DAL.Repositories
{
    public class UnitOfWork(ApplicationDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            //1=>Get Typt Name
            var typeName = typeof(TEntity).Name;
            //2=> Dictionary<string, object> ===> string key [Name Of Entity type] , object value [Object of GenericRepository]

            //if(_repositories.ContainsKey(typeName))
            //{
            //    return (IGenericRepository<TEntity, Tkey>)_repositories[typeName];
            //}
            if (_repositories.TryGetValue(typeName, out object? repo))
                return (IGenericRepository<TEntity, Tkey>)repo;

            else
            {
                //1=> Create Object of GenericRepository
                var Repo = new GenericRepository<TEntity, Tkey>(_dbContext);
                //2=> Add obj to Dictionary
                //_repositories.Add(typeName, Repo);
                _repositories["typeName"] = Repo;
                //3=> Return the Object
                return Repo;
            }

        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
