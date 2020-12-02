﻿using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Sector.Drinks.Commands.Request;
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

namespace KitchenRoutingSystem.Sector.Drinks.Handlers.PrepareDrinksHandler
{
    public class PrepareDrinksHandler : CommandHandler, IRequestHandler<PrepareDrinksRequest, CommandResponse>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ILogger<PrepareDrinksHandler> _logger;
        private readonly IMapper _mapper;

        public PrepareDrinksHandler(IRepository<Product> productRepository, ILogger<PrepareDrinksHandler> logger, IRepository<Order> orderRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = logger;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(PrepareDrinksRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing Drinks...");

            //Verifying product in storage
            var products = _productRepository.GetAll().Result.Where(a => a.ProductType == request.products.FirstOrDefault().ProductType).FirstOrDefault();
            var order = _orderRepository.Get(request.orderId).Result;
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);

            if (order != null)
            {
                if (products.Quantity == 0 || products.Quantity < order.Products.FirstOrDefault().Quantity)
                {
                    _logger.LogInformation("Missing Drinks, updating your order");

                    try
                    {
                        order.RemoveProduct(products);
                        await _orderRepository.Edit(order);
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
                _logger.LogError($"It´s not possible deliver Drinks without an order");
                return BadRequestResponse(null, "It´s not possible deliver Drinks without an order");
            }

            var data = new CreateOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, productDto, order.Total, order.Notes, order.Status);
            return CreateResponse(data, "Drinks delivered");
        }

        private async Task UpdateProductList(Product products, Order order)
        {
            var productDto = _mapper.Map<List<ProductDto>>(order.Products);
            var newQuantity = products.Quantity - order.Products.Where(a => a.ProductType == Domain.Enums.EProductType.Drink).FirstOrDefault().Quantity;

            products.Quantity = newQuantity;

            await _productRepository.Edit(products);
            _logger.LogInformation("Drinks quantity has updated");

            order.UpdateProductStatus(Domain.Enums.EProductStatus.Delivered, productDto.FirstOrDefault());
            await _orderRepository.Edit(order);

            _logger.LogInformation("Drinks delivered");

            var productjson = JsonConvert.SerializeObject(products);
            var orderjson = JsonConvert.SerializeObject(order);
            _logger.LogInformation(productjson);
            _logger.LogInformation(orderjson);
        }
    }

}
