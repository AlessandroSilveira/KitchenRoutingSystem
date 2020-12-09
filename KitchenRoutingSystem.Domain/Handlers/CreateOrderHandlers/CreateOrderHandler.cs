using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Request;
using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Response;
using KitchenRoutingSystem.Domain.Commands.ProcessProductCommads;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Handlers.CreateOrderHandlers
{
    public class CreateOrderHandler : CommandHandler, IRequestHandler<ProcessOrderRequest, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly IMediator _mediator;

        public CreateOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateOrderHandler> logger, IMediator mediator)
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
            var order = new Order();
            await _unitOfWork.Orders.Add(order);


            //Create OrderProcess
            foreach(var itens in request.Products)
            {
                var orderProductDto = new OrderProductDto(order.Id, Guid.Parse(itens.ProductId), itens.Value, itens.Quantity, itens.ProductType);
                await _mediator.Send(orderProductDto);
            }

            // Consolida as notificações
            AddNotifications(order);


            // Return data
            if (order != null)
            {
                var processProductsCommand = new ProcessProductCommad { orderId = order.Number, products = request.Products };
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
