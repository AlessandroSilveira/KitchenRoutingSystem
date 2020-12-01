using KitchenRoutingSystem.Domain.Commands.PorcessOrderCommands.Request;
using KitchenRoutingSystem.Domain.MQ.OrderConsumerQueue;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Api
{
    public class OrderConsumer
    {
        private readonly IOrderConsumerQueue _orderConsumerQueue;
        private readonly ILogger<OrderConsumer> _log;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public OrderConsumer(IOrderConsumerQueue orderConsumerQueue,
            ILogger<OrderConsumer> log,
            IConfiguration configuration,
            IMediator mediator)
        {
            _orderConsumerQueue = orderConsumerQueue;
            _log = log;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task StartConsumer()
        {
            try
            {
                _log.LogInformation($"{_configuration["OrderQueueConfiguration:OrderConsumer"] } Consumer started.");
                _orderConsumerQueue.StartConsumerQueues(async (message) => await Dequeue(message), _configuration["OrderQueueConfiguration:OrderConsumer"]);
            }
            catch (Exception)
            {
                _log.LogError($"Error to start consumer {_configuration["OrderQueueConfiguration:OrderConsumer"] }. ");
            }
        }

        public async Task<bool> Dequeue(BasicDeliverEventArgs message)
        {
            try
            {
                string json = Encoding.UTF8.GetString(message.Body.ToArray());
                var orderTransfer = JsonConvert.DeserializeObject<ProcessOrderRequest>(json);
                orderTransfer.DeliveryTag = message.DeliveryTag;
                var orderprcessed = await _mediator.Send(orderTransfer);

                return orderprcessed.StatusCode == 200 ? true : false;
            }
            catch (Exception e)
            {
                _log.LogError($"Error on Dequeue message: {e}");
                throw;
            }
        }
    }
}
