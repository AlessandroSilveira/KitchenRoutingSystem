using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareGrill
{
    public class DeclareGrillConsumer : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        public DeclareGrillConsumer(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["GrillQueueConfiguration:ExchangeGrillConsumer"], _configuration["GrillQueueConfiguration:ExchangeGrillType"], true, false, null);
            _channel.QueueDeclare(_configuration["GrillQueueConfiguration:GrillConsumer"], true, false, false);
            _channel.QueueBind(_configuration["GrillQueueConfiguration:GrillConsumer"], _configuration["GrillQueueConfiguration:ExchangeGrillConsumer"], _configuration["GrillQueueConfiguration:GrillConsumer"], null);
        }
    }
}
