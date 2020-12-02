using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.MQ.Channel;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace KitchenRoutingSystem.Domain.Services
{
    public class OrderPublishServices : QueueChannel, IOrderPublishService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderPublishServices> _logger;

        public OrderPublishServices(IConfiguration configuration, ILogger<OrderPublishServices> logger) : base(configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void PublishOrder(CreateOrderRequest order)
        {
            var message = JsonConvert.SerializeObject(order);
            var messageBodyBytes = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("", _configuration["OrderQueueConfiguration:OrderConsumer"], null, messageBodyBytes);
            _logger.LogInformation($"Message delived in {_configuration["OrderQueueConfiguration: OrderConsumer"]} queue ");
        }
    }
}
