using KitchenRoutingSystem.Domain.MQ.Channel;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace KitchenRoutingSystem.Domain.Services
{
    public class ProcessProductService : QueueChannel, IProcessProductService
    {
        private readonly IConfiguration _configuration;
        

        public ProcessProductService(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        

        public void SendOrderToDesertector(byte[] messageBodyBytes)
        {
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
                channel.BasicPublish("", _configuration["DessertQueueConfiguration:DessertQueue"], null, messageBodyBytes);

            }
           
        }

        public void SendOrderToDrinkSector(byte[] messageBodyBytes)
        {
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
                channel.BasicPublish("", _configuration["DrinksQueueConfiguration:DrinksQueue"], null, messageBodyBytes);

            }

          
        }

        public void SendOrderToFriesSector(byte[] messageBodyBytes)
        {
            //var factory = new ConnectionFactory
            //{
            //    HostName = _configuration["RabbitConfig:HostName"],
            //    Port = Convert.ToInt32(_configuration["RabbitConfig:Port"]),
            //    UserName = _configuration["RabbitConfig:UserName"],
            //    Password = _configuration["RabbitConfig:Password"],
            //    VirtualHost = _configuration["RabbitConfig:VirtualHost"],
            //    AutomaticRecoveryEnabled = true,
            //};
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.BasicPublish("", _configuration["FriesQueueConfiguration:FriesQueue"], null, messageBodyBytes);

            //}
            _channel.BasicPublish("", _configuration["FriesQueueConfiguration:FriesConsumer"], null, messageBodyBytes);
        }

        public void SendOrderToGrillSector(byte[] messageBodyBytes)
        {
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
                channel.BasicPublish("", _configuration["GrillQueueConfiguration:GrillQueue"], null, messageBodyBytes);

            }
          
        }

        public void SendOrderToSaladSector(byte[] messageBodyBytes)
        {
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
                channel.BasicPublish("", _configuration["SaladQueueConfiguration:SaladQueue"], null, messageBodyBytes);

            }

            
        }
    }
}
