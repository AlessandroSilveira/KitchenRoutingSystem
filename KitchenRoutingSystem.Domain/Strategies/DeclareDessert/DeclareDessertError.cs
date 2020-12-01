using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareDessert
{
    public class DeclareDessertError : IDeclare
    {
        private readonly IConfiguration _configuration;
        protected IModel _channel;

        public DeclareDessertError(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["DessertQueueConfiguration:ExchangeDessertError"], _configuration["DessertQueueConfiguration:ExchangeDessertType"], true, false, null);
            _channel.QueueDeclare(_configuration["DessertQueueConfiguration:DessertError"], true, false, false);
            _channel.QueueBind(_configuration["DessertQueueConfiguration:DessertError"], _configuration["DessertQueueConfiguration:ExchangeDessertError"], _configuration["DessertQueueConfiguration:DessertError"], null);
        }
    }
}
