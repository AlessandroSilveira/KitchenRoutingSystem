using System.Threading;
using System.Threading.Tasks;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.Commands.OrderCommands.Response;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repositories;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Newtonsoft.Json;

namespace KitchenRoutingSystem.Domain.Handlers.OrderHandlers
{
    public class CreateOrderHandler : CommandHandler, IRequestHandler<CreateOrderRequest, CommandResponse>
    {        
        private readonly IOrderPublishService _orderPublishService;

        public CreateOrderHandler(IOrderPublishService orderPublishService)
        {           
            _orderPublishService = orderPublishService;
        }

        public async Task<CommandResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            _orderPublishService.PublishOrder(request);

            var data = new CreateOrderResponse(request.Number, request.CreateDate, request.LastUpdateDate, request.Products, request.Total, request.Notes, request.Status);
            return CreateResponse(data, "Order sent successfully");
        }
    }
}