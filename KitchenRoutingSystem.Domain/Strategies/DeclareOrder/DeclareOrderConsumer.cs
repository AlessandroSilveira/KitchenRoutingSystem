using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.Declare
{
    public class DeclareOrderConsumer : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        public DeclareOrderConsumer(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", _configuration["RabbitConfig:ExchangeRetry"] }
            };
            _channel.ExchangeDeclare(_configuration["OrderQueueConfiguration:ExchangeOrderConsumer"], _configuration["OrderQueueConfiguration:ExchangeOrderType"], true, false, null);
            _channel.QueueDeclare(_configuration["OrderQueueConfiguration:OrderConsumer"], true, false, false);
            _channel.QueueBind(_configuration["OrderQueueConfiguration:OrderConsumer"], _configuration["OrderQueueConfiguration:ExchangeOrderConsumer"], _configuration["OrderQueueConfiguration:OrderConsumer"], null);
        }
    }
}
