using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace KitchenRoutingSystem.Domain.MQ.OrderConsumerQueue
{
    public interface IOrderConsumerQueue
    {
        void StartConsumerQueues(Func<BasicDeliverEventArgs, Task<bool>> consumeMessage, string queue);
    }
}