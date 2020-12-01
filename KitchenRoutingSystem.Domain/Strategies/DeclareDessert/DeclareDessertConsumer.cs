using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareDessert
{
    public class DeclareDessertConsumer : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        public DeclareDessertConsumer(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["DessertQueueConfiguration:ExchangeDessertConsumer"], _configuration["DessertQueueConfiguration:ExchangeDessertType"], true, false, null);
            _channel.QueueDeclare(_configuration["DessertQueueConfiguration:DessertConsumer"], true, false, false);
            _channel.QueueBind(_configuration["DessertQueueConfiguration:DessertConsumer"], _configuration["DessertQueueConfiguration:ExchangeDessertConsumer"], _configuration["DessertQueueConfiguration:DessertConsumer"], null);
        }
    }
}
