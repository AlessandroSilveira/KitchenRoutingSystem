using KitchenRoutingSystem.Domain.MQ.Channel;
using KitchenRoutingSystem.Domain.MQ.OrderConsumerQueue;
using KitchenRoutingSystem.Sector.Fries.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Fries.Services
{
    public class FriesConsumerQueueService : QueueChannel, IFriesConsumerQueueService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FriesConsumerQueueService> _log;

        public FriesConsumerQueueService(IConfiguration configuration, ILogger<FriesConsumerQueueService> log) : base(configuration)
        {
            _configuration = configuration;
            _log = log;
        }

        public void StartConsumerQueues(Func<BasicDeliverEventArgs, Task<bool>> consumeMessage, string queue)
        {
            var attempts = 5;
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    if (await consumeMessage(ea))
                        _channel.BasicAck(ea.DeliveryTag, false);
                    else
                    {
                        _log.LogError($"Message not consumed.");
                        _channel.BasicNack(ea.DeliveryTag, false, false);
                    }

                }
                catch (Exception e)
                {
                    SendMessageToErrorQueue(ea);
                    _log.LogError($"Message not consumed.");
                }
            };

            _channel.BasicQos(0, 100, false);
            _channel.BasicConsume(_configuration["FriesQueueConfiguration:FriesConsumer"], false, consumer);
        }

        private void SendMessageToErrorQueue(BasicDeliverEventArgs ea)
        {
            TryOpen();
            _channel.BasicPublish("", _configuration["FriesQueueConfiguration:FriesError"], null, ea.Body);
            _channel.BasicAck(ea.DeliveryTag, false);
        }
    }
}

