using RabbitMQ.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using KitchenRoutingSystem.Domain.Strategies.Declare;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareDessert
{
    public class DeclareDessertRetry : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public DeclareDessertRetry(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare.Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000},
                {"x-dead-letter-exchange", _configuration["DessertQueueConfiguration:ExchangeDessertConsumer"]}
            };

            _channel.ExchangeDeclare(_configuration["DessertQueueConfiguration:ExchangeDessertRetry"], _configuration["DessertQueueConfiguration:ExchangeDessertType"], true, false, null);
            _channel.QueueDeclare(_configuration["DessertQueueConfiguration:DessertRetry"], true, false, false);
            _channel.QueueBind(_configuration["DessertQueueConfiguration:DessertRetry"], _configuration["DessertQueueConfiguration:ExchangeDessertRetry"], _configuration["DessertQueueConfiguration:DessertRetry"], null);
        }
    }
}
