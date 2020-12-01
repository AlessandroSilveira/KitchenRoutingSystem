using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;

namespace KitchenRoutingSystem.Domain.Services.Interfaces
{
    public interface IOrderPublishService
    {
        void PublishOrder(CreateOrderRequest order);
    }
}
