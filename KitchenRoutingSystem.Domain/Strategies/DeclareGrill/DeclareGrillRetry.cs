using RabbitMQ.Client;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using KitchenRoutingSystem.Domain.Strategies.Declare;

namespace KitchenRoutingSystem.Domain.Strategies.DeclareGrill
{
    public class DeclareGrillRetry : IDeclare
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;

        public DeclareGrillRetry(IConfiguration configuration, IModel channel)
        {
            _channel = channel;
            _configuration = configuration;
        }

        public void Declare(Declare.Declare declare)
        {
            var args = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000},
                {"x-dead-letter-exchange", _configuration["GrillQueueConfiguration:ExchangeGrillConsumer"]}
            };

            _channel.ExchangeDeclare(_configuration["GrillQueueConfiguration:ExchangeGrillRetry"], _configuration["GrillQueueConfiguration:ExchangeGrillType"], true, false, null);
            _channel.QueueDeclare(_configuration["GrillQueueConfiguration:GrillRetry"], true, false, false);
            _channel.QueueBind(_configuration["GrillQueueConfiguration:GrillRetry"], _configuration["GrillQueueConfiguration:ExchangeGrillRetry"], _configuration["GrillQueueConfiguration:GrillRetry"], null);
        }
    }
}
