
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Infra.Repositories
{
    public class Seed
    {
        private readonly IUnitOfWork _unityOfWork;
        private readonly ILogger<Seed> _logger;

        public Seed(IUnitOfWork unityOfWork, ILogger<Seed> logger)
        {
            _unityOfWork = unityOfWork;
            _logger = logger;
        }

        public async Task SeedProducts()
        {
            var productFries = new Product()
            {
               
                ProductType = Domain.Enums.EProductType.Fries,
                Quantity = 10,
                Status = Domain.Enums.EProductStatus.Pending,
                Value = 2
            };

            var productGrill = new Product()
            {
                
                ProductType = Domain.Enums.EProductType.Grill,
                Quantity = 10,
                Status = Domain.Enums.EProductStatus.Pending,
                Value = 2
            };

            var productSalad = new Product()
            {
               
                ProductType = Domain.Enums.EProductType.Salad,
                Quantity = 10,
                Status = Domain.Enums.EProductStatus.Pending,
                Value = 2
            };

            var productDrink = new Product()
            {
                
                ProductType = Domain.Enums.EProductType.Drink,
                Quantity = 10,
                Status = Domain.Enums.EProductStatus.Pending,
                Value = 2
            };

            var productDessert = new Product()
            {
                
                ProductType = Domain.Enums.EProductType.Dessert,
                Quantity = 10,
                Status = Domain.Enums.EProductStatus.Pending,
                Value = 2
            };

            try
            {
                await _unityOfWork.Products.Add(productFries);
                await _unityOfWork.Products.Add(productGrill);
                await _unityOfWork.Products.Add(productSalad);
                await _unityOfWork.Products.Add(productDrink);
                await _unityOfWork.Products.Add(productDessert);
                _logger.LogInformation("Seed Complete");
            }
            catch (System.Exception e)
            {
                _logger.LogError($"Error on Seed Product Data: {e}" );
                throw e;
            }


            

        }
    }
}
