using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Request;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Response;
using KitchenRoutingSystem.Domain.Commands.ProcessProductCommads;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Handlers.ProcessOrderHandlers
{
    public class ProcessOrderHandler : CommandHandler, IRequestHandler<ProcessOrderRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProcessOrderHandler> _logger;
        private readonly IMediator _mediator;

        public ProcessOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProcessOrderHandler> logger, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<CommandResponse> Handle(ProcessOrderRequest request, CancellationToken cancellationToken)
        {
            if (request.Products == null)
            {
                _logger.LogInformation("Your order contains no products");
                return BadRequestResponse(null, "Your order contains no products");
            }

            var product = _mapper.Map<List<Product>>(request.Products);
            // Create Order
            var order = new Order(product);

            // Consolida as notificações
            AddNotifications(order);

            var newOrder = await _unitOfWork.Orders.Add(order);
            _logger.LogInformation("Order Created");

            // Return data
            if (newOrder != null)
            {
                var processProductsCommand = new ProcessProductCommad { orderId = newOrder.Number, products = request.Products };
                await _mediator.Send(processProductsCommand);
                var data = new ProcessOrderResponse(order.Number, order.CreateDate, order.LastUpdateDate, request.Products, order.Total, order.Notes, order.Status, 1);

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
