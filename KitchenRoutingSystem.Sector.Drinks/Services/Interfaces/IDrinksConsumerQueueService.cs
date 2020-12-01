using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Drinks.Services.Interfaces
{
    public interface IDrinksConsumerQueueService
    {
        void StartConsumerQueues(Func<BasicDeliverEventArgs, Task<bool>> consumeMessage, string queue);
    }
}
