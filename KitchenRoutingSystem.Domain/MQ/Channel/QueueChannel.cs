using KitchenRoutingSystem.Domain.Strategies.Declare;
using KitchenRoutingSystem.Domain.Strategies.DeclareDessert;
using KitchenRoutingSystem.Domain.Strategies.DeclareDrinks;
using KitchenRoutingSystem.Domain.Strategies.DeclareFries;
using KitchenRoutingSystem.Domain.Strategies.DeclareGrill;
using KitchenRoutingSystem.Domain.Strategies.DeclareSalad;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace KitchenRoutingSystem.Domain.MQ.Channel
{
    public class QueueChannel
    {
        protected IModel _channel;
        protected IConnection _connection;
        private readonly IConfiguration _configuration;

        public QueueChannel(IConfiguration configuration)
        {
            _configuration = configuration;
            OpenConection();
        }
        private void OpenConection()
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

            _connection = factory.CreateConnection();
            OpenChannel();
        }

        private void OpenChannel()
        {
            _channel = _connection.CreateModel();
            Declare();
        }

        protected virtual void Declare()
        {
            DeclareConsumerQueue();
            DeclareErrorQueue();
            DeclareRetryQueue();

            DeclareFriesConsumerQueue();
            DeclareFriesErrorQueue();
            DeclareFriesRetryQueue();

            DeclareGrillConsumerQueue();
            DeclareGrillErrorQueue();
            DeclareGrillRetryQueue();

            DeclareSaladConsumerQueue();
            DeclareSaladErrorQueue();
            DeclareSaladRetryQueue();

            DeclareDrinksConsumerQueue();
            DeclareDrinksErrorQueue();
            DeclareDrinksRetryQueue();

            DeclareDessertConsumerQueue();
            DeclareDessertErrorQueue();
            DeclareDessertRetryQueue();
        }

        #region OrderQuue
        private void DeclareConsumerQueue()
        {
            new Declare(new DeclareOrderConsumer(_configuration, _channel)).DeclareQueue();
        }

        private void DeclareRetryQueue()
        {
            new Declare(new DeclareOrderRetry(_configuration, _channel)).DeclareQueue();
        }
        private void DeclareErrorQueue()
        {
            new Declare(new DeclareOrderError(_configuration, _channel)).DeclareQueue();
        }
        #endregion

        #region FriesQueue
        private void DeclareFriesConsumerQueue()
        {
            new Declare(new DeclareFriesConsumer(_configuration, _channel)).DeclareQueue();
        }

        private void DeclareFriesRetryQueue()
        {
            new Declare(new DeclareFriesRetry(_configuration, _channel)).DeclareQueue();
        }
        private void DeclareFriesErrorQueue()
        {
            new Declare(new DeclareFriesError(_configuration, _channel)).DeclareQueue();
        }
        #endregion

        #region GrillQueue
        private void DeclareGrillConsumerQueue()
        {
            new Declare(new DeclareGrillConsumer(_configuration, _channel)).DeclareQueue();
        }

        private void DeclareGrillRetryQueue()
        {
            new Declare(new DeclareGrillRetry(_configuration, _channel)).DeclareQueue();
        }
        private void DeclareGrillErrorQueue()
        {
            new Declare(new DeclareGrillError(_configuration, _channel)).DeclareQueue();
        }
        #endregion

        #region SaladQueue
        private void DeclareSaladConsumerQueue()
        {
            new Declare(new DeclareSaladConsumer(_configuration, _channel)).DeclareQueue();
        }

        private void DeclareSaladRetryQueue()
        {
            new Declare(new DeclareSaladRetry(_configuration, _channel)).DeclareQueue();
        }
        private void DeclareSaladErrorQueue()
        {
            new Declare(new DeclareSaladError(_configuration, _channel)).DeclareQueue();
        }
        #endregion

        #region DrinksQueue
        private void DeclareDrinksConsumerQueue()
        {
            new Declare(new DeclareDrinksConsumer(_configuration, _channel)).DeclareQueue();
        }

        private void DeclareDrinksRetryQueue()
        {
            new Declare(new DeclareDrinksRetry(_configuration, _channel)).DeclareQueue();
        }
        private void DeclareDrinksErrorQueue()
        {
            new Declare(new DeclareDrinksError(_configuration, _channel)).DeclareQueue();
        }
        #endregion

        #region DessertQueue
        private void DeclareDessertConsumerQueue()
        {
            new Declare(new DeclareDessertConsumer(_configuration, _channel)).DeclareQueue();
        }

        private void DeclareDessertRetryQueue()
        {
            new Declare(new DeclareDessertRetry(_configuration, _channel)).DeclareQueue();
        }
        private void DeclareDessertErrorQueue()
        {
            new Declare(new DeclareDessertError(_configuration, _channel)).DeclareQueue();
        }
        #endregion



        protected void TryOpen()
        {
            if (!_connection.IsOpen)
            {
                OpenConection();
            }

            if (!_channel.IsOpen)
            {
                OpenChannel();
            }
        }
    }
}
