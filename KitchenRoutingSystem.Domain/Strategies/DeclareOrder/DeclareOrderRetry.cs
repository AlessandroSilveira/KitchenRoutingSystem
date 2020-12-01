using RabbitMQ.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace KitchenRoutingSystem.Domain.Strategies.Declare
{
    public class DeclareOrderRetry : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public DeclareOrderRetry(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000},
                {"x-dead-letter-exchange", _configuration["OrderQueueConfiguration:ExchangeOrderConsumer"]}
            };

            _channel.ExchangeDeclare(_configuration["OrderQueueConfiguration:ExchangeOrderRetry"], _configuration["OrderQueueConfiguration:ExchangeOrderType"], true, false, null);
            _channel.QueueDeclare(_configuration["OrderQueueConfiguration:OrderRetry"], true, false, false);
            _channel.QueueBind(_configuration["OrderQueueConfiguration:OrderRetry"], _configuration["OrderQueueConfiguration:ExchangeOrderRetry"], _configuration["OrderQueueConfiguration:OrderRetry"], null);
        }
    }
}
