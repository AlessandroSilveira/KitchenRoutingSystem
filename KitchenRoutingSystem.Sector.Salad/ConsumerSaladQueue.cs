using KitchenRoutingSystem.Sector.Salad.Commands.Request;
using KitchenRoutingSystem.Sector.Salad.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Salad
{
    public class ConsumerSaladQueue
    {
        private ISaladConsumerQueueService _saladConsumerQueue;
        private readonly ILogger<ConsumerSaladQueue> _log;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ConsumerSaladQueue(
            ILogger<ConsumerSaladQueue> log,
            IConfiguration configuration,
            IMediator mediator,
            ISaladConsumerQueueService saladConsumerQueue)
        {

            _log = log;
            _configuration = configuration;
            _mediator = mediator;
            _saladConsumerQueue = saladConsumerQueue;
        }

        public async Task StartConsumer()
        {
            try
            {
                _log.LogInformation($"{_configuration["FriesQueueConfiguration:FriesConsumer"] } Consumer started.");
                _saladConsumerQueue.StartConsumerQueues(async (message) => await Dequeue(message), _configuration["FriesQueueConfiguration:FriesConsumer"]);
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
                var request = JsonConvert.DeserializeObject<PrepareSaladRequest>(json);
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
