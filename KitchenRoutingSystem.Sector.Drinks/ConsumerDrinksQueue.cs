using KitchenRoutingSystem.Sector.Drinks.Commands.Request;
using KitchenRoutingSystem.Sector.Drinks.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Drinks
{
    public class ConsumerDrinksQueue
    {
        private IDrinksConsumerQueueService _drinksConsumerQueue;
        private readonly ILogger<ConsumerDrinksQueue> _log;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ConsumerDrinksQueue(
            ILogger<ConsumerDrinksQueue> log,
            IConfiguration configuration,
            IMediator mediator,
            IDrinksConsumerQueueService drinksConsumerQueue)
        {
            _log = log;
            _configuration = configuration;
            _mediator = mediator;
            _drinksConsumerQueue = drinksConsumerQueue;
        }

        public async Task StartConsumer()
        {
            try
            {
                _log.LogInformation($"{_configuration["DrinksQueueConfiguration:DrinksConsumer"] } Consumer started.");
                _drinksConsumerQueue.StartConsumerQueues(async (message) => await Dequeue(message), _configuration["DrinksQueueConfiguration:DrinksConsumer"]);
            }
            catch (Exception)
            {
                _log.LogError($"Error to start consumer {_configuration["DrinksQueueConfiguration:DrinksConsumer"] }. ");
            }
        }

        public async Task<bool> Dequeue(BasicDeliverEventArgs message)
        {
            try
            {
                string json = Encoding.UTF8.GetString(message.Body.ToArray());
                var request = JsonConvert.DeserializeObject<PrepareDrinksRequest>(json);
                var orderprcessed = await _mediator.Send(request);

                return orderprcessed.StatusCode == 201 ? true : false;
            }
            catch (Exception e)
            {
                _log.LogError($"Error on Dequeue message: {e}");
                throw;
            }
        }
    }
}
