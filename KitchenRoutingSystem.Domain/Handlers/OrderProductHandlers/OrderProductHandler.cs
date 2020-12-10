using AutoMapper;
using KitchenRoutingSystem.Domain.Commands.OrderProductCommands.Request;
using KitchenRoutingSystem.Domain.Commands.OrderProductCommands.Response;
using KitchenRoutingSystem.Domain.Repository.UnitOfWork;
using KitchenRoutingSystem.Shared.Commands.Response;
using KitchenRoutingSystem.Shared.Handler;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.Handlers.OrderProductHandlers
{
    public class OrderProductHandler : CommandHandler, IRequestHandler<OrderProductRequest, CommandResponse>
    {
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unityOfWork;       

        public OrderProductHandler(IMapper mapper, IUnitOfWork unityOfWork)
        {
            _mapper = mapper;
            _unityOfWork = unityOfWork;
        }

        public async Task<CommandResponse> Handle(OrderProductRequest request, CancellationToken cancellationToken)
        {
            var orderProductMapped = _mapper.Map<Entities.OrderProduct>(request);

            var orderProduct = await _unityOfWork.OrderProducts.Add(orderProductMapped);

            var data = new OrderProductResponse(orderProduct.OrderId, orderProduct.ProductId, orderProduct.Value, orderProduct.Quantity,  orderProduct.ProductType);
            return CreateResponse(data, "Order sent successfully");
        }
    }
}
