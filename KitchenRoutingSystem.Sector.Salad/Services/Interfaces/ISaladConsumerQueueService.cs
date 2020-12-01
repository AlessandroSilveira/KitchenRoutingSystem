using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Salad.Services.Interfaces
{
    public interface ISaladConsumerQueueService
    {
        void StartConsumerQueues(Func<BasicDeliverEventArgs, Task<bool>> consumeMessage, string queue);
    }
}
