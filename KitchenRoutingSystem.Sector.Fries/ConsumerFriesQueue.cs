using KitchenRoutingSystem.Sector.Fries.Commands.Request;
using KitchenRoutingSystem.Sector.Fries.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Fries
{
    public class ConsumerFriesQueue
    {
        private IFriesConsumerQueueService _friesConsumerQueue;
        private readonly ILogger<ConsumerFriesQueue> _log;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ConsumerFriesQueue(
            ILogger<ConsumerFriesQueue> log,
            IConfiguration configuration,
            IMediator mediator,
            IFriesConsumerQueueService friesConsumerQueue)
        {

            _log = log;
            _configuration = configuration;
            _mediator = mediator;
            _friesConsumerQueue = friesConsumerQueue;
        }

        public async Task StartConsumer()
        {
            try
            {
                _log.LogInformation($"{_configuration["FriesQueueConfiguration:FriesConsumer"] } Consumer started.");
                _friesConsumerQueue.StartConsumerQueues(async (message) => await Dequeue(message), _configuration["FriesQueueConfiguration:FriesConsumer"]);
            }
            catch (Exception)
            {
                _log.LogError($"Error to start consumer {_configuration["FriesQueueConfiguration:FriesConsumer"] }. ");
            }
        }

        public async Task<bool> Dequeue(BasicDeliverEventArgs message)
        {
            try
            {
                string json = Encoding.UTF8.GetString(message.Body.ToArray());
                var request = JsonConvert.DeserializeObject<PrepareFriesRequest>(json);
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
