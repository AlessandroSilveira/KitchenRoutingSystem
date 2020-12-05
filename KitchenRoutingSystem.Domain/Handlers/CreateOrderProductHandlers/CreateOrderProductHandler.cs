using AutoMapper;
using KitchenRoutingSystem.Domain.DTOs;
using KitchenRoutingSystem.Domain.Entities;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Handlers.CreateOrderProductHandlers
{
    public class CreateOrderProductHandler : CommandHandler, IRequestHandler<OrderProductDto, CommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderProductHandler> _logger;       

        public CreateOrderProductHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateOrderProductHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;           
        }

        public async Task<CommandResponse> Handle(OrderProductDto request, CancellationToken cancellationToken)
        {
            //Map Data
            var orderProduct = _mapper.Map<OrderProduct>(request);

            //Create OrderProduct
            var data = await _unitOfWork.OrderProducts.Add(orderProduct);


            // Return data
            if (data != null)
            {              
                _logger.LogInformation("OrderProduct successfully registered");
                return CreateResponse(orderProduct, "OrderProduct successfully registered");
            }
            else
            {
                _logger.LogInformation("Error on create OrderProduct, please try again");
                return BadRequestResponse(null, "Error on create OrderProduct, please try again");
            }
        }
    }
}
