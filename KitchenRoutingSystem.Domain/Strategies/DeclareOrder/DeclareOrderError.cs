using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.Declare
{
    public class DeclareOrderError : IDeclare
    {
        private readonly IConfiguration _configuration;
        protected IModel _channel;

        public DeclareOrderError(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }
        public void Declare(Declare declare)
        {
            var queueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", _configuration["RabbitConfig:ExchangeRetry"] }
            };
            _channel.ExchangeDeclare(_configuration["OrderQueueConfiguration:ExchangeOrderError"], _configuration["OrderQueueConfiguration:ExchangeOrderType"], true, false, null);
            _channel.QueueDeclare(_configuration["OrderQueueConfiguration:OrderError"], true, false, false);
            _channel.QueueBind(_configuration["OrderQueueConfiguration:OrderError"], _configuration["OrderQueueConfiguration:ExchangeOrderError"], _configuration["OrderQueueConfiguration:OrderError"], null);
        }
    }
}
