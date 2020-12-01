using RabbitMQ.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using KitchenRoutingSystem.Domain.Strategies.Declare;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareFries
{
    public class DeclareFriesRetry : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public DeclareFriesRetry(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare.Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000},
                {"x-dead-letter-exchange", _configuration["FriesQueueConfiguration:ExchangeFriesConsumer"]}
            };

            _channel.ExchangeDeclare(_configuration["FriesQueueConfiguration:ExchangeFriesRetry"], _configuration["FriesQueueConfiguration:ExchangeFriesType"], true, false, null);
            _channel.QueueDeclare(_configuration["FriesQueueConfiguration:FriesRetry"], true, false, false);
            _channel.QueueBind(_configuration["FriesQueueConfiguration:FriesRetry"], _configuration["FriesQueueConfiguration:ExchangeFriesRetry"], _configuration["FriesQueueConfiguration:FriesRetry"], null);
        }
    }
}
