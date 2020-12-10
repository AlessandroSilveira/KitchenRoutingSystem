using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Commands.OrderProductCommands.Request;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;

namespace KitchenRoutingSystem.Domain.Handlers.ProcessOrderHandler
{
    public class ProcessOrderHandler : CommandHandler, IRequestHandler<OrderDto, CommandResponse>
    {
        private readonly IOrderPublishService _orderPublishService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unityOfWork;
        private readonly IMediator _mediator;

        public ProcessOrderHandler(IOrderPublishService orderPublishService, IMapper mapper, IUnitOfWork unityOfWork, IMediator mediator)
        {
            _orderPublishService = orderPublishService;
            _mapper = mapper;
            _unityOfWork = unityOfWork;
            _mediator = mediator;
        }

        public async Task<CommandResponse> Handle(OrderDto request, CancellationToken cancellationToken)
        {
            var productMap = _mapper.Map<List<Product>>(request.Products);

            productMap = CheckForProductInStock(productMap);

            var order = new Order();
            var createOrderRequest = _mapper.Map<CreateOrderRequest>(order);
            _orderPublishService.PublishOrder(createOrderRequest);

            foreach (var item in productMap)
            {
                var ProductOrder = new OrderProductRequest(order.Id, Guid.Parse(item.ProductId), item.Value, item.Quantity, item.ProductType);
                await _mediator.Send(ProductOrder);
            }
          
            var data = new CreateOrderResponse(createOrderRequest.Number, createOrderRequest.CreateDate, createOrderRequest.LastUpdateDate, request.Products, createOrderRequest.Total, createOrderRequest.Notes, createOrderRequest.Status);
            return CreateResponse(data, "Order sent successfully");
        }

        private List<Product> CheckForProductInStock(List<Product> productMap)
        {
            foreach (var item in productMap)
            {
                var productQuatity = _unityOfWork.Products.Get(item.ProductId).Result.Quantity;
                if(productQuatity == 0)                
                    productMap.Remove(item);
                
                if(productQuatity < item.Quantity)                
                    item.Quantity = productQuatity;                
            }

            return productMap;
        }
    }
}