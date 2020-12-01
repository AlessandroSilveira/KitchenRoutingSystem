using System.Collections.Generic;
using KitchenRoutingSystem.Domain.Strategies.Declare;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareGrill
{
    public class DeclareGrillError : IDeclare
    {
        private readonly IConfiguration _configuration;
        protected IModel _channel;

        public DeclareGrillError(IConfiguration configuration, IModel channel)
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
            _channel.ExchangeDeclare(_configuration["GrillQueueConfiguration:ExchangeGrillError"], _configuration["GrillQueueConfiguration:ExchangeGrillType"], true, false, null);
            _channel.QueueDeclare(_configuration["GrillQueueConfiguration:GrillError"], true, false, false);
            _channel.QueueBind(_configuration["GrillQueueConfiguration:GrillError"], _configuration["GrillQueueConfiguration:ExchangeGrillError"], _configuration["GrillQueueConfiguration:GrillError"], null);
        }
    }
}
