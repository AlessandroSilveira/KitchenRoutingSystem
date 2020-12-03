using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Repository.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity obj);
        Task<TEntity> GetByIdAsync(string id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> UpdateAsync(TEntity obj);
        Task RemoveAsync(string id);
        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate);

        Task Dispose();
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);
        void DetachLocal(Func<TEntity, bool> predicate);
    }
}