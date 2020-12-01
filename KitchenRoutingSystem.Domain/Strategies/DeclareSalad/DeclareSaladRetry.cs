using RabbitMQ.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using KitchenRoutingSystem.Domain.Strategies.Declare;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareSalad
{
    public class DeclareSaladRetry : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public DeclareSaladRetry(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare.Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000},
                {"x-dead-letter-exchange", _configuration["SaladQueueConfiguration:ExchangeSaladConsumer"]}
            };

            _channel.ExchangeDeclare(_configuration["SaladQueueConfiguration:ExchangeSaladRetry"], _configuration["SaladQueueConfiguration:ExchangeSaladType"], true, false, null);
            _channel.QueueDeclare(_configuration["SaladQueueConfiguration:SaladRetry"], true, false, false);
            _channel.QueueBind(_configuration["SaladQueueConfiguration:SaladRetry"], _configuration["SaladQueueConfiguration:ExchangeSaladRetry"], _configuration["SaladQueueConfiguration:SaladRetry"], null);
        }
    }
}
