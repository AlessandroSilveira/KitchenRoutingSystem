using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Sector.Desserts.Commands.Request;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Dessert.Handlers.PrepareDessertHandler
{
    public class PrepareDessertHandler : CommandHandler, IRequestHandler<PrepareDessertRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PrepareDessertHandler> _logger;
        private readonly IMediator _mediator;

        public PrepareDessertHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PrepareDessertHandler> logger, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<CommandResponse> Handle(PrepareDessertRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Dessert...");

            var products = _unitOfWork.Products.GetAll().Result.Where(a => a.ProductType == request.products.FirstOrDefault().ProductType).FirstOrDefault();
            var orderProduct = await _unitOfWork.OrderProducts.Get(request.orderId);
            var productDto = _mapper.Map<List<ProductDto>>(Convert.ToInt32(products.ProductId));

            if (orderProduct != null)
            {
                if (products.Quantity == 0)
                {
                    _logger.LogInformation("Missing Dessert, updating your order");

                    try
                    {
                        await _unitOfWork.OrderProducts.Delete(orderProduct.ProductId);
                        _logger.LogInformation("OrderProduct Updated");

                        var orderDto = _mapper.Map<OrderDto>(orderProduct);

                        await UpdateProductList(products, orderDto);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error on update OrderProduct, message: {e}");
                        throw;
                    }
                }
                else if (products.Quantity < orderProduct.Quantity)
                {
                    try
                    {
                        orderProduct.ChangeQuantity(products.Quantity);
                        await _unitOfWork.OrderProducts.Update(orderProduct);
                        _logger.LogError($"Product quantity was updated in OrderProduct, the new quantity is {orderProduct.Quantity} ");

                        var orderDto = _mapper.Map<OrderDto>(orderProduct);
                        await UpdateProductList(products, orderDto);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error on update quantity in OrderProduct, message: {e}");
                        throw;
                    }
                }
                else
                {
                    var orderDto = _mapper.Map<OrderDto>(orderProduct);
                    await UpdateProductList(products, orderDto);
                }
            }
            else
            {
                _logger.LogError($"It´s not possible deliver Dessert without an order");
                return BadRequestResponse(null, "It´s not possible deliver Dessert without an order");
            }

            var order = _mapper.Map<Order>(orderProduct);
            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, productDto, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Dessert delivered");
        }

        private async Task UpdateProductList(Product products, OrderDto orderDto)
        {
            var productDto = _mapper.Map<List<ProductDto>>(orderDto.Products);
            var newQuantity = products.Quantity - orderDto.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Dessert).FirstOrDefault().Quantity;

            products.Quantity = newQuantity;

            await _unitOfWork.Products.Update(products);
            _logger.LogInformation("Dessert quantity has updated");

            var order = _mapper.Map<Order>(orderDto);

            await _unitOfWork.Orders.Update(order);

            _logger.LogInformation("Dessert delivered");
        }
    }

}
