using KitchenRoutingSystem.Domain.MQ.Channel;
using KitchenRoutingSystem.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace KitchenRoutingSystem.Domain.Services
{
    public class ProcessProductService : QueueChannel, IProcessProductService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProcessProductService> _logger;

        public ProcessProductService(IConfiguration configuration, ILogger<ProcessProductService> logger) : base(configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void SendOrderToDessertSector(byte[] messageBodyBytes)
        {           
            _channel.BasicPublish("", _configuration["DessertQueueConfiguration:DessertConsumer"], null, messageBodyBytes);
            _logger.LogInformation($"Message delived in {_configuration["DessertQueueConfiguration:DessertConsumer"]} queue ");
        }

        public void SendOrderToDrinkSector(byte[] messageBodyBytes)
        {            
            _channel.BasicPublish("", _configuration["DrinksQueueConfiguration:DrinksConsumer"], null, messageBodyBytes);
            _logger.LogInformation($"Message delived in {_configuration["DrinksQueueConfiguration:DrinksConsumer"]} queue ");
        }

        public void SendOrderToFriesSector(byte[] messageBodyBytes)
        {
            _channel.BasicPublish("", _configuration["FriesQueueConfiguration:FriesConsumer"], null, messageBodyBytes);
            _logger.LogInformation($"Message delived in {_configuration["FriesQueueConfiguration:FriesConsumer"]} queue ");
        }

        public void SendOrderToGrillSector(byte[] messageBodyBytes)
        {
            _channel.BasicPublish("", _configuration["GrillQueueConfiguration:GrillConsumer"], null, messageBodyBytes);
            _logger.LogInformation($"Message delived in {_configuration["GrillQueueConfiguration:GrillConsumer"]} queue ");
        }

        public void SendOrderToSaladSector(byte[] messageBodyBytes)
        {
            _channel.BasicPublish("", _configuration["SaladQueueConfiguration:SaladConsumer"], null, messageBodyBytes);
            _logger.LogInformation($"Message delived in {_configuration["SaladQueueConfiguration:SaladConsumer"]} queue ");
        }
    }
}
