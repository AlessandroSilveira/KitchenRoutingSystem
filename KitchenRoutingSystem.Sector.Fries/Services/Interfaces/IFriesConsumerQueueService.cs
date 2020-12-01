using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Fries.Services.Interfaces
{
    public interface IFriesConsumerQueueService
    {
        void StartConsumerQueues(Func<BasicDeliverEventArgs, Task<bool>> consumeMessage, string queue);
    }
}
