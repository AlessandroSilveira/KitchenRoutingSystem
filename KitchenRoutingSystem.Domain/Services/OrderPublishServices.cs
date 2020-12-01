using KitchenRoutingSystem.Domain.Commands.OrderCommands.Request;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace KitchenRoutingSystem.Domain.Services
{
    public class OrderPublishServices : IOrderPublishService
    {
        private readonly IConfiguration _configuration;

        public OrderPublishServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void PublishOrder(CreateOrderRequest order)
        {
            var message = JsonConvert.SerializeObject(order);
            var messageBodyBytes = Encoding.UTF8.GetBytes(message);
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitConfig:HostName"],
                Port = Convert.ToInt32(_configuration["RabbitConfig:Port"]),
                UserName = _configuration["RabbitConfig:UserName"],
                Password = _configuration["RabbitConfig:Password"],
                VirtualHost = _configuration["RabbitConfig:VirtualHost"],
                AutomaticRecoveryEnabled = true,
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.BasicPublish("", _configuration["OrderQueueConfiguration:OrderConsumer"], null, messageBodyBytes);

            }
        }
    }
}
