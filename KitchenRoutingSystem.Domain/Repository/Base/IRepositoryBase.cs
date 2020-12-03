using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Repository.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> Get(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Add(TEntity entity);
        Task<int> Delete(int id);
        Task<int> Update(TEntity entity);
    }
}