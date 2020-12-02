using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private static readonly List<Order> orders = new List<Order>();
        public async Task<List<Order>> GetAll()
        {
            return await Task.Run(() => orders.ToList());
        }

        public async Task<Order> Get(string id)
        {
            return await Task.Run(() => orders.Where(a=>a.Number == id.ToString()).FirstOrDefault());
        }

        public async Task<Order> Add(Order order)
        {
            await Task.Run(() => orders.Add(order));
            return order;
        }

        public async Task Edit(Order order)
        {
            await Task.Run(() =>
            {
                orders.Remove(order);
                orders.Add(order);
            });
        }

        public async Task Delete(string id)
        {
            var order = Get(id.ToString()).Result;
            await Task.Run(() => orders.Remove(order));
        }

     
    }
}