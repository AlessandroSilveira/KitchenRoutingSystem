using KitchenRoutingSystem.Sector.Grill.Commands.Request;
using KitchenRoutingSystem.Sector.Grill.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Grill
{
    public class ConsumerGrillQueue
    {
        private IGrillConsumerQueueService _grillConsumerQueue;
        private readonly ILogger<ConsumerGrillQueue> _log;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ConsumerGrillQueue(
            ILogger<ConsumerGrillQueue> log,
            IConfiguration configuration,
            IMediator mediator,
            IGrillConsumerQueueService grillConsumerQueue)
        {

            _log = log;
            _configuration = configuration;
            _mediator = mediator;
            _grillConsumerQueue = grillConsumerQueue;
        }

        public async Task StartConsumer()
        {
            try
            {
                _log.LogInformation($"{_configuration["FriesQueueConfiguration:FriesConsumer"] } Consumer started.");
                _grillConsumerQueue.StartConsumerQueues(async (message) => await Dequeue(message), _configuration["FriesQueueConfiguration:FriesConsumer"]);
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
                var request = JsonConvert.DeserializeObject<PrepareGrillRequest>(json);
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
