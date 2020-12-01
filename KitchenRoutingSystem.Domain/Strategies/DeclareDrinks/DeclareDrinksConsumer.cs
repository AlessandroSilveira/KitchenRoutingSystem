using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareDrinks
{
    public class DeclareDrinksConsumer : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        public DeclareDrinksConsumer(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["DrinksQueueConfiguration:ExchangeDrinksConsumer"], _configuration["DrinksQueueConfiguration:ExchangeDrinksType"], true, false, null);
            _channel.QueueDeclare(_configuration["DrinksQueueConfiguration:DrinksConsumer"], true, false, false);
            _channel.QueueBind(_configuration["DrinksQueueConfiguration:DrinksConsumer"], _configuration["DrinksQueueConfiguration:ExchangeDrinksConsumer"], _configuration["DrinksQueueConfiguration:DrinksConsumer"], null);
        }
    }
}
