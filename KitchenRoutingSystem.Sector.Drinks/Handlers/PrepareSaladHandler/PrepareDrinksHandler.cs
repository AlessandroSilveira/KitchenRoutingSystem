using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Sector.Drinks.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Drinks.Handlers.PrepareDrinksHandler
{
    public class PrepareDrinksHandler : CommandHandler, IRequestHandler<PrepareDrinksRequest, CommandResponse>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ILogger<PrepareDrinksHandler> _logger;

        public PrepareDrinksHandler(IRepository<Product> productRepository, ILogger<PrepareDrinksHandler> logger, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<CommandResponse> Handle(PrepareDrinksRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Drinks...");

            //Verifying if had in storage
            var products = _productRepository.GetAll().Result.Where(a => a.ProductType == request.Product.ProductType).FirstOrDefault();
            var order = _orderRepository.Get(request.OrderId).Result;

            if (products.Quantity == 0)
            {
                _logger.LogInformation("Missing Drinks, updating your order");

                try
                {
                    order.RemoveProduct(products);
                    await _orderRepository.Edit(order);
                    _logger.LogInformation("Order Updated");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error on update order, message: {e}");
                    throw;
                }
            }
            else
            {
                var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Drink).FirstOrDefault().Quantity;
                products.Quantity = newQuantity;

                await _productRepository.Edit(products);
                _logger.LogInformation("Drinks quantity has updated");

                order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, products);
                await _orderRepository.Edit(order);

                _logger.LogInformation("Drinks delivered");

            }

            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, order.Products, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Drinks delivered");


        }
    }

}
