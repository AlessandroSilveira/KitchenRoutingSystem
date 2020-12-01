using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareSalad
{
    public class DeclareSaladConsumer : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        public DeclareSaladConsumer(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare.Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", _configuration["RabbitConfig:ExchangeRetry"] }
            };
            _channel.ExchangeDeclare(_configuration["SaladQueueConfiguration:ExchangeSaladConsumer"], _configuration["SaladQueueConfiguration:ExchangeSaladType"], true, false, null);
            _channel.QueueDeclare(_configuration["SaladQueueConfiguration:SaladConsumer"], true, false, false);
            _channel.QueueBind(_configuration["SaladQueueConfiguration:SaladConsumer"], _configuration["SaladQueueConfiguration:ExchangeSaladConsumer"], _configuration["SaladQueueConfiguration:SaladConsumer"], null);
        }
    }
}
