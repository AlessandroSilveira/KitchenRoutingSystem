using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareSalad
{
    public class DeclareSaladError : IDeclare
    {
        private readonly IConfiguration _configuration;
        protected IModel _channel;

        public DeclareSaladError(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }
        public void Declare(Declare.Declare declare)
        {
            var queueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", _configuration["RabbitConfig:ExchangeRetry"] }
            };
            _channel.ExchangeDeclare(_configuration["SaladQueueConfiguration:ExchangeSaladError"], _configuration["SaladQueueConfiguration:ExchangeSaladType"], true, false, null);
            _channel.QueueDeclare(_configuration["SaladQueueConfiguration:SaladError"], true, false, false);
            _channel.QueueBind(_configuration["SaladQueueConfiguration:SaladError"], _configuration["SaladQueueConfiguration:ExchangeSaladError"], _configuration["SaladQueueConfiguration:SaladError"], null);
        }
    }
}
