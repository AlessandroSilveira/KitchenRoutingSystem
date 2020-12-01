using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareFries
{
    public class DeclareFriesConsumer : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        public DeclareFriesConsumer(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["FriesQueueConfiguration:ExchangeFriesConsumer"], _configuration["FriesQueueConfiguration:ExchangeFriesType"], true, false, null);
            _channel.QueueDeclare(_configuration["FriesQueueConfiguration:FriesConsumer"], true, false, false);
            _channel.QueueBind(_configuration["FriesQueueConfiguration:FriesConsumer"], _configuration["FriesQueueConfiguration:ExchangeFriesConsumer"], _configuration["FriesQueueConfiguration:FriesConsumer"], null);
        }
    }
}
