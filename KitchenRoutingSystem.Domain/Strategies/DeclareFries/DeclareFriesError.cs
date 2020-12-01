using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareFries
{
    public class DeclareFriesError : IDeclare
    {
        private readonly IConfiguration _configuration;
        protected IModel _channel;

        public DeclareFriesError(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["FriesQueueConfiguration:ExchangeFriesError"], _configuration["FriesQueueConfiguration:ExchangeFriesType"], true, false, null);
            _channel.QueueDeclare(_configuration["FriesQueueConfiguration:FriesError"], true, false, false);
            _channel.QueueBind(_configuration["FriesQueueConfiguration:FriesError"], _configuration["FriesQueueConfiguration:ExchangeFriesError"], _configuration["FriesQueueConfiguration:FriesError"], null);
        }
    }
}
