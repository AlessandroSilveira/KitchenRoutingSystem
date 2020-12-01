using KitchenRoutingSystem.Sector.Desserts.Commands.Request;
using KitchenRoutingSystem.Sector.Desserts.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace KitchenRoutingSystem.Sector.Dessert
{
    public class ConsumerDessertQueue
    {
        private IDessertConsumerQueueService _DessertConsumerQueue;
        private readonly ILogger<ConsumerDessertQueue> _log;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public ConsumerDessertQueue(
            ILogger<ConsumerDessertQueue> log,
            IConfiguration configuration,
            IMediator mediator,
            IDessertConsumerQueueService DessertConsumerQueue)
        {

            _log = log;
            _configuration = configuration;
            _mediator = mediator;
            _DessertConsumerQueue = DessertConsumerQueue;
        }

        public async Task StartConsumer()
        {
            try
            {
                _log.LogInformation($"{_configuration["DessertQueueConfiguration:DessertConsumer"] } Consumer started.");
                _DessertConsumerQueue.StartConsumerQueues(async (message) => await Dequeue(message), _configuration["DessertQueueConfiguration:DessertConsumer"]);
            }
            catch (Exception)
            {
                _log.LogError($"Error to start consumer {_configuration["DessertQueueConfiguration:DessertConsumer"] }. ");
            }
        }

        public async Task<bool> Dequeue(BasicDeliverEventArgs message)
        {
            try
            {
                string json = Encoding.UTF8.GetString(message.Body.ToArray());
                var request = JsonConvert.DeserializeObject<PrepareDessertRequest>(json);
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
