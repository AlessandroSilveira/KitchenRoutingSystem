using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareDrinks
{
    public class DeclareDrinksError : IDeclare
    {
        private readonly IConfiguration _configuration;
        protected IModel _channel;

        public DeclareDrinksError(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["DrinksQueueConfiguration:ExchangeDrinksError"], _configuration["DrinksQueueConfiguration:ExchangeDrinksType"], true, false, null);
            _channel.QueueDeclare(_configuration["DrinksQueueConfiguration:DrinksError"], true, false, false);
            _channel.QueueBind(_configuration["DrinksQueueConfiguration:DrinksError"], _configuration["DrinksQueueConfiguration:ExchangeDrinksError"], _configuration["DrinksQueueConfiguration:DrinksError"], null);
        }
    }
}
