using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private static readonly List<Product> products = new List<Product>
        {
            new Product
            {
                ProductId = "1",
                ProductType = Domain.Enums.EProductType.Fries,
                Quantity = 5,
                Value = 2
            },
            new Product
            {
                ProductId = "2",
                ProductType = Domain.Enums.EProductType.Grill,
                Quantity = 3,
                Value = 5
            },
            new Product
            {
                ProductId = "3",
                ProductType = Domain.Enums.EProductType.Salad,
                Quantity = 4,
                Value = 1
            },
            new Product
            {
                ProductId = "4",
                ProductType = Domain.Enums.EProductType.Drink,
                Quantity = 10,
                Value = 6
            },
             new Product
            {
                ProductId = "5",
                ProductType = Domain.Enums.EProductType.Dessert,
                Quantity = 4,
                Value = 3
            },
        };


        public async Task<List<Product>> GetAll()
        {
            return await Task.Run(() => products.ToList());
        }

        public async Task<Product> Get(string id)
        {
            return await Task.Run(() => products.Where(a => a.ProductId == id).FirstOrDefault());
        }

        public async Task<Product> Add(Product product)
        {
            await Task.Run(() => products.Add(product));
            return product;
        }

        public async Task Edit(Product product)
        {
            await Task.Run(() =>
            {
                products.Remove(product);
                products.Add(product);
            });
        }

        public async Task Delete(string id)
        {
            var product = Get(id).Result;
            await Task.Run(() => products.Remove(product));
        }
    }
}