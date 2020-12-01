using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Sector.Salad.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Salad.Handlers.PrepareFriesHandler
{
    public class PrepareFriesHandler : CommandHandler, IRequestHandler<PrepareFriesRequest, CommandResponse>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ILogger<PrepareFriesHandler> _logger;

        public PrepareFriesHandler(IRepository<Product> productRepository, ILogger<PrepareFriesHandler> logger, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<CommandResponse> Handle(PrepareFriesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Fries...");

            //Verifying if had in storage
            var products = _productRepository.GetAll().Result.Where(a => a.ProductType == request.Product.ProductType).FirstOrDefault();
            var order =  _orderRepository.Get(request.OrderId).Result;

            

            if (products.Quantity == 0)
            {
                _logger.LogInformation("Missing Fries, updating your order");
               
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
                var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Fries).FirstOrDefault().Quantity;
                products.Quantity = newQuantity;

                await _productRepository.Edit(products);
                _logger.LogInformation("Fries quantity has updated");

                order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, products);

               

                await _orderRepository.Edit(order);

                _logger.LogInformation("Fries delivered");

            }

            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, order.Products, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Fries delivered");


        }
    }
    
}
