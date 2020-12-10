using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Request;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Response;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Handlers.CreateOrderHandlers
{
    public class CreateOrderHandler : CommandHandler, IRequestHandler<ProcessOrderRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;       
        private readonly ILogger<CreateOrderHandler> _logger;       

        public CreateOrderHandler(IUnitOfWork unitOfWork, ILogger<CreateOrderHandler> logger)
        {
            _unitOfWork = unitOfWork;            
            _logger = logger;           
        }

        public async Task<CommandResponse> Handle(ProcessOrderRequest request, CancellationToken cancellationToken)
        {
            var order = new Order();
            var result = await _unitOfWork.Orders.Add(order);

            // Consolida as notificações
            AddNotifications(order);

            // Return data
            if (order != null)
            {
                var data = new ProcessOrderResponse(result.Number, result.CreateDate, result.LastUpdateDate, result.Total, result.Notes, result.Status, 1);
                _logger.LogInformation("Order successfully registered");
                return CreateResponse(data, "Order successfully registered");
            }
            else
            {
                _logger.LogInformation("Error on create order, please try again");
                return BadRequestResponse(null, "Error on create order, please try again");
            }
        }
    }
}
