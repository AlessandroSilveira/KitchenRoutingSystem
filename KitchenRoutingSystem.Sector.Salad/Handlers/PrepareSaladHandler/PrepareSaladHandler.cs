using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Sector.Salad.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Salad.Handlers.PrepareSaladHandler
{
    public class PrepareSaladHandler : CommandHandler, IRequestHandler<PrepareSaladRequest, CommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PrepareSaladHandler> _logger;
        private readonly IMapper _mapper;

        public PrepareSaladHandler(IProductRepository productRepository, ILogger<PrepareSaladHandler> logger, IOrderRepository orderRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(PrepareSaladRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Salad...");

            //Verifying product in storage
            var products = _productRepository.SearchAsync(a => a.ProductType == request.products.FirstOrDefault().ProductType).Result.FirstOrDefault();
            var order = _orderRepository.GetByIdAsync(request.orderId).Result;
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);

            if (order != null)
            {
                if (products.Quantity == 0 || products.Quantity < order.Products.FirstOrDefault().Quantity)
                {
                    _logger.LogInformation("Missing Salad, updating your order");

                    try
                    {
                        order.RemoveProduct(products);
                        await _orderRepository.UpdateAsync(order);
                        _logger.LogInformation("Order Updated");

                        await UpdateProductList(products, order);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error on update order, message: {e}");
                        throw;
                    }
                }
                else
                {
                    await UpdateProductList(products, order);
                }
            }
            else
            {
                _logger.LogError($"It´s not possible deliver Salad without an order");
                return BadRequestResponse(null, "It´s not possible deliver Salad without an order");
            }



            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, productDto, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Salad delivered");
        }

        private async Task UpdateProductList(Product products, Order order)
        {
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);
            var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Salad).FirstOrDefault().Quantity;

            products.Quantity = newQuantity;

            await _productRepository.UpdateAsync(products);
            _logger.LogInformation("Salad quantity has updated");

            order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, productDto.FirstOrDefault());
            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation("Salad delivered");

            var productjson = JsonConvert.SerializeObject(products);
            var orderjson = JsonConvert.SerializeObject(order);
            _logger.LogInformation(productjson);
            _logger.LogInformation(orderjson);
        }
    }

}
