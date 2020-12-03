using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Infra.Repositories.Base;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(Context.ApplicationContext context) : base(context)
        {
        }
    }
}