using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Common.Helpers;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Common.Messaging.Factory;
using RabbitMQ.Common.Model;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.BackgroundServices
{
    public class MessageBackgroundService : BackgroundService
    {
        private readonly IBusFactory _busFactory;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IRabbitMQHelper _rabbitMQHelper;

        public MessageBackgroundService(IBusFactory busFactory,
            IServiceScopeFactory serviceScopeFactory,
            IRabbitMQHelper rabbitMQHelper)
        {
            _busFactory = busFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitMQHelper = rabbitMQHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bus = this._busFactory.CrateBus();

            var serviceProvider = this._serviceScopeFactory.CreateScope().ServiceProvider;

            var consumeHouseEventParameter = new ConsumeMessageParameter<IHouseService, House>
            {
                ExchangeName = "house.update.exchange",
                ExchangeType = ExchangeType.Fanout,
                RouteKey = string.Empty,
                QueueName = "house.update.queue",
                AdvancedBus = bus,
                OnMessage = data =>
                {
                    var houseService = serviceProvider.GetService<IHouseService>();
                    houseService.HandleUpdateEvent(data);
                },
                FallBack = data =>
                {
                    Console.WriteLine("Some thing fxxc up !!");
                },
                MaxRetryTime = 3
            };

            this._rabbitMQHelper.RegisterConsumer(consumeHouseEventParameter);
        }
    }
}