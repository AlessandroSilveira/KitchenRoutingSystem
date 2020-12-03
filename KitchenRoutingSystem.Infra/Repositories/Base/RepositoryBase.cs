using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KitchenRoutingSystem.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace KitchenRoutingSystem.Infra.Repositories.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {

        protected Context.ApplicationContext Context;


        public RepositoryBase(Context.ApplicationContext context)
        {
            Context = context;
        }

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            try
            {
                await Context.Set<TEntity>().AddAsync(obj);
                await Context.SaveChangesAsync();
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            try
            {
                return await Context.Set<TEntity>().FindAsync(id);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return Context.Set<TEntity>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {


            Context.Entry(obj).State = EntityState.Detached;
            Context.Entry(obj).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return obj;
        }

        public async Task RemoveAsync(string id)
        {
            var entity = Context.Set<TEntity>().FindAsync(id).Result;
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();

        }

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }


        public async Task Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        public virtual void DetachLocal(Func<TEntity, bool> predicate)
        {
            var local = Context.Set<TEntity>().Local.Where(predicate).FirstOrDefault();
            if (local != null)
            {
                Context.Entry(local).State = EntityState.Detached;
            }
        }
    }
}