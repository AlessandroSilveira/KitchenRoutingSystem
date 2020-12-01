using KitchenRoutingSystem.Domain.MQ.Channel;
using KitchenRoutingSystem.Domain.MQ.OrderConsumerQueue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Domain.MQ.OrderConsumer
{
    public class OrderConsumerQueue : QueueChannel, IOrderConsumerQueue
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderConsumerQueue> _log;

        public OrderConsumerQueue(IConfiguration configuration, ILogger<OrderConsumerQueue> log) : base(configuration)
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
            _channel.BasicConsume(_configuration["OrderQueueConfiguration:OrderConsumer"], false, consumer);
        }

        private void SendMessageToErrorQueue(BasicDeliverEventArgs ea)
        {
            TryOpen();
            _channel.BasicPublish("", _configuration["OrderQueueConfiguration:OrderError"], null, ea.Body);
            _channel.BasicAck(ea.DeliveryTag, false);
        }

        private static IEnumerable<object> GetListOfHeader(object header)
        {
            List<object> headerItems = null;
            if (header is IEnumerable)
                headerItems = (List<object>)header;

            return headerItems;
        }
    }
}

