using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Sector.Grill.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Grill.Handlers.PrepareGrillHandler
{
    public class PrepareGrillHandler : CommandHandler, IRequestHandler<PrepareGrillRequest, CommandResponse>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ILogger<PrepareGrillHandler> _logger;

        public PrepareGrillHandler(IRepository<Product> productRepository, ILogger<PrepareGrillHandler> logger, IRepository<Order> orderRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<CommandResponse> Handle(PrepareGrillRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Grill...");

            var products = _productRepository.GetAll().Result.Where(a => a.ProductType == request.Product.ProductType).FirstOrDefault();
            var order = _orderRepository.Get(request.OrderId).Result;

            if (products.Quantity == 0)
            {
                _logger.LogInformation("Missing Grill, updating your order");

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
                var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Grill).FirstOrDefault().Quantity;
                products.Quantity = newQuantity;

                await _productRepository.Edit(products);
                _logger.LogInformation("Grill quantity has updated");

                order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, products);



                await _orderRepository.Edit(order);

                _logger.LogInformation("Grill delivered");

            }

            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, order.Products, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Grill delivered");


        }
    }

}
