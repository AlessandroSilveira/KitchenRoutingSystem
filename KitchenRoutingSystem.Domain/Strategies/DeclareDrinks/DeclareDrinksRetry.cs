using RabbitMQ.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using KitchenRoutingSystem.Domain.Strategies.Declare;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareDrinks
{
    public class DeclareDrinksRetry : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public DeclareDrinksRetry(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare.Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000},
                {"x-dead-letter-exchange", _configuration["DrinksQueueConfiguration:ExchangeDrinksConsumer"]}
            };

            _channel.ExchangeDeclare(_configuration["DrinksQueueConfiguration:ExchangeDrinksRetry"], _configuration["DrinksQueueConfiguration:ExchangeDrinksType"], true, false, null);
            _channel.QueueDeclare(_configuration["DrinksQueueConfiguration:DrinksRetry"], true, false, false);
            _channel.QueueBind(_configuration["DrinksQueueConfiguration:DrinksRetry"], _configuration["DrinksQueueConfiguration:ExchangeDrinksRetry"], _configuration["DrinksQueueConfiguration:DrinksRetry"], null);
        }
    }
}
