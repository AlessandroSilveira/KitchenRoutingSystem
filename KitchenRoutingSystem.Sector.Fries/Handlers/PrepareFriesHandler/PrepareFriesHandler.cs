using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Sector.Fries.Commands.Request;
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

namespace KitchenRoutingSystem.Sector.Fries.Handlers.PrepareFriesHandler
{
    public class PrepareFriesHandler : CommandHandler, IRequestHandler<PrepareFriesRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PrepareFriesHandler> _logger;
        private readonly IMediator _mediator;

        public PrepareFriesHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PrepareFriesHandler> logger, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<CommandResponse> Handle(PrepareFriesRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Fries...");

            var products = _unitOfWork.Products.GetAll().Result.Where(a => a.ProductType == request.products.FirstOrDefault().ProductType).FirstOrDefault();
            var orderProduct = await _unitOfWork.OrderProducts.Get(request.orderId);
            var productDto = _mapper.Map<List<ProductDto>>(Convert.ToInt32(products.ProductId));

            if (orderProduct != null)
            {
                if (products.Quantity == 0 || products.Quantity < orderProduct.Quantity)
                {
                    _logger.LogInformation("Missing Fries, updating your order");

                    try
                    {
                        await _unitOfWork.OrderProducts.Delete(orderProduct.ProductId);                       
                        _logger.LogInformation("OrderProduct Updated");

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
                _logger.LogError($"It´s not possible deliver Fries without an order");
                return BadRequestResponse(null, "It´s not possible deliver Fries without an order");
            }


            var data = new CreateOrderResponse(orderProduct.Number, orderProduct.CreateDate, orderProduct.LastUpdateDate, productDto, orderProduct.Total, orderProduct.Notes, orderProduct.Status);
            return CreateResponse(data, "Fries delivered");
        }

        private async Task UpdateProductList(Product products, Order order)
        {
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);
            var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Fries).FirstOrDefault().Quantity;

            products.Quantity = newQuantity;

            await _unitOfWork.Products.Update(products);
            _logger.LogInformation("Fries quantity has updated");

            //order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, productDto.FirstOrDefault());
            await _unitOfWork.Orders.Update(order);

            _logger.LogInformation("Fries delivered");
        }
    }

}
