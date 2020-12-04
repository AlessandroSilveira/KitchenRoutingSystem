using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Enums;
using KitchenRoutingSystem.Domain.Repository;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
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
        private readonly IUnitOfWork _unityOfWork;
        private readonly ILogger<PrepareSaladHandler> _logger;
        private readonly IMapper _mapper;

        public PrepareSaladHandler(ILogger<PrepareSaladHandler> logger, IMapper mapper, IUnitOfWork unityOfWork)
        { 
            _logger = logger;            
            _mapper = mapper;
            _unityOfWork = unityOfWork;
        }

        public async Task<CommandResponse> Handle(PrepareSaladRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Salad...");

            //Verifying product in storage
            var products = _unityOfWork.Products.GetAll().Result.Where(a => a.ProductType == request.products.FirstOrDefault().ProductType).FirstOrDefault();
            var order = _unityOfWork.Orders.Get(request.orderId).Result;
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);

            if (order != null)
            {
                if (products.Quantity == 0 || products.Quantity < order.Products.FirstOrDefault().Quantity)
                {
                    _logger.LogInformation("Missing Salad, updating your order");

                    try
                    {
                        order.RemoveProduct(products);
                        await _unityOfWork.Orders.Update(order);
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

            await _unityOfWork.Products.Update(products);
            _logger.LogInformation("Salad quantity has updated");

            UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, productDto.FirstOrDefault());
            await _unityOfWork.Orders.Update(order);

            _logger.LogInformation("Salad delivered");

            var productjson = JsonConvert.SerializeObject(products);
            var orderjson = JsonConvert.SerializeObject(order);
            _logger.LogInformation(productjson);
            _logger.LogInformation(orderjson);
        }

        public void UpdateProductStatus(EProductStatus status, ProductDto products)
        {
            var product = _mapper.Map<Product>(products);
            _unityOfWork.Products.Delete(Convert.ToInt32(product.ProductId));
            products.Status = status;
            _unityOfWork.Products.Add(product);
        }
    }

}
