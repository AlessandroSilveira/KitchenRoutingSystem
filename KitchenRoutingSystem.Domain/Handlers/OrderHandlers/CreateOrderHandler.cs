using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;

namespace KitchenRoutingSystem.Domain.Handlers.OrderHandlers
{
    public class CreateOrderHandler : CommandHandler, IRequestHandler<OrderDto, CommandResponse>
    {        
        private readonly IOrderPublishService _orderPublishService;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IOrderPublishService orderPublishService, IMapper mapper)
        {
            _orderPublishService = orderPublishService;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(OrderDto request, CancellationToken cancellationToken)
        {
            var productMap = _mapper.Map<List<Product>>(request.Products);
            var order = new Order(productMap);
            var createOrderRequest = _mapper.Map<CreateOrderRequest>(order);

            _orderPublishService.PublishOrder(createOrderRequest);

            var data = new CreateOrderResponse(createOrderRequest.Number, createOrderRequest.CreateDate, createOrderRequest.LastUpdateDate, request.Products, createOrderRequest.Total, createOrderRequest.Notes, createOrderRequest.Status);
            return CreateResponse(data, "Order sent successfully");
        }
    }
}