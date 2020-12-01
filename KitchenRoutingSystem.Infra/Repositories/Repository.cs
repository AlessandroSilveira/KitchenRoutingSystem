//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using KitchenRoutingSystem.Domain.Entities;
//using KitchenRoutingSystem.Domain.Repositories;
//using KitchenRoutingSystem.Infra.Context;

//namespace KitchenRoutingSystem.Infra.Repositories
//{
//    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
//    {
//        private static List<Order> orders = new List<Order>();

//        public IEnumerable<TEntity> GetAll()
//        {
//            try
//            {
//                return OrderContext.Set<TEntity>();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Couldn't retrieve entities {ex.Message}");
//            }
//        }

//        public async Task<TEntity> AddAsync(TEntity entity)
//        {
//            if (entity == null)
//            {
//                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
//            }

//            try
//            {
//                await OrderContext.AddAsync(entity);
//                await OrderContext.SaveChangesAsync();

//                return entity;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"{nameof(entity)} could not be saved {ex.Message}");
//            }
//        }

//        public async Task<TEntity> UpdateAsync(TEntity entity)
//        {
//            if (entity == null)
//            {
//                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
//            }

//            try
//            {
//                OrderContext.Update(entity);
//                await OrderContext.SaveChangesAsync();

//                return entity;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"{nameof(entity)} could not be updated {ex.Message}");
//            }
//        }

//        public async Task UpdateRangeAsync(List<TEntity> entities)
//        {
//            if (entities == null)
//            {
//                throw new ArgumentNullException($"{nameof(UpdateRangeAsync)} entities must not be null");
//            }

//            try
//            {
//                OrderContext.UpdateRange(entities);
//                await OrderContext.SaveChangesAsync();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"{nameof(entities)} could not be updated {ex.Message}");
//            }
//        }
//    }
//}