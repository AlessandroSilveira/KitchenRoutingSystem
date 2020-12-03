using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Infra.Repositories.Base;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class ProductRepository : RepositoryBase<Product> , IProductRepository
    {
        public ProductRepository(Context.ApplicationContext context) : base(context)
        {
        }
    }
}