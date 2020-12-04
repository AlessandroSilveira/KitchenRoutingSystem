using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Enums;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Sector.Grill.Commands.Request;
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

namespace KitchenRoutingSystem.Sector.Grill.Handlers.PrepareGrillHandler
{
    public class PrepareGrillHandler : CommandHandler, IRequestHandler<PrepareGrillRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PrepareGrillHandler> _logger;
        private readonly IMapper _mapper;

        public PrepareGrillHandler(ILogger<PrepareGrillHandler> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResponse> Handle(PrepareGrillRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Grill...");

            var products = _unitOfWork.Products.GetAll().Result.Where(a => a.ProductType == request.products.FirstOrDefault().ProductType).FirstOrDefault();
            var order = _unitOfWork.Orders.Get(request.orderId).Result;
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);

            if (order != null)
            {
                if (products.Quantity == 0 || products.Quantity < order.Products.FirstOrDefault().Quantity)
                {
                    _logger.LogInformation("Missing Grill, updating your order");

                    try
                    {
                        order.RemoveProduct(products);
                        await _unitOfWork.Orders.Update(order);
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
                    await UpdateProductList(products, order);                
            }
            else
            {
                _logger.LogError($"It´s not possible deliver Grill without an order");
                return BadRequestResponse(null, "It´s not possible deliver Grill without an order");
            }

            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, productDto, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Grill delivered");
        }

        private async Task UpdateProductList(Product products, Order order)
        {
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);
            var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Grill).FirstOrDefault().Quantity;

            products.Quantity = newQuantity;

            await _unitOfWork.Products.Update(products);
            _logger.LogInformation("Grill quantity has updated");

            UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, productDto.FirstOrDefault());
            await _unitOfWork.Orders.Update(order);

            _logger.LogInformation("Grill delivered");

            var productjson = JsonConvert.SerializeObject(products);
            var orderjson = JsonConvert.SerializeObject(order);
            _logger.LogInformation(productjson);
            _logger.LogInformation(orderjson);
        }

        public void UpdateProductStatus(EProductStatus status, ProductDto products)
        {

            _unitOfWork.Products.Remove(product);
            .Status = status;
            Products.Add(product);
        }
    }

}
