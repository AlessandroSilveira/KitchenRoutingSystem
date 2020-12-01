using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Sector.Desserts.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Dessert.Handlers.PrepareDessertHandler
{
    public class PrepareDessertHandler : CommandHandler, IRequestHandler<PrepareDessertRequest, CommandResponse>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ILogger<PrepareDessertHandler> _logger;

        public PrepareDessertHandler(IRepository<Product> productRepository, ILogger<PrepareDessertHandler> logger, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<CommandResponse> Handle(PrepareDessertRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Dessert...");

            //Verifying if had in storage
            var products = _productRepository.GetAll().Result.FirstOrDefault();//.Result.Where(a => a.ProductType == request.Product.ProductType).FirstOrDefault();
            var order = _orderRepository.Get(request.OrderId).Result;



            if (products.Quantity == 0)
            {
                _logger.LogInformation("Missing Dessert, updating your order");

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
                var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Dessert).FirstOrDefault().Quantity;
                products.Quantity = newQuantity;

                await _productRepository.Edit(products);
                _logger.LogInformation("Dessert quantity has updated");

                order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, products);



                await _orderRepository.Edit(order);

                _logger.LogInformation("Dessert delivered");

            }

            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, order.Products, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Dessert delivered");


        }
    }

}
