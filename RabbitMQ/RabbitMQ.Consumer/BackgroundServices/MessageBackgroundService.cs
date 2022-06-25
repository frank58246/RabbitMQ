using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

namespace RabbitMQ.Consumer.BackgroundServices
{
    public class MessageBackgroundService : BackgroundService
    {
        private readonly IBusFactory _busFactory;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IRabbitMQHelper _rabbitMQHelper;

        private ILogger<MessageBackgroundService> _logger;

        public MessageBackgroundService(IBusFactory busFactory,
            IServiceScopeFactory serviceScopeFactory,
            IRabbitMQHelper rabbitMQHelper, ILogger<MessageBackgroundService> logger)
        {
            _busFactory = busFactory;
            _serviceScopeFactory = serviceScopeFactory;
            _rabbitMQHelper = rabbitMQHelper;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            {
                var bus = this._busFactory.CrateBus();

                var serviceProvider = this._serviceScopeFactory.CreateScope().ServiceProvider;

                var queueSubfix = Environment.GetEnvironmentVariable("UNIQUE_QUEUE") == "y"
                    ? Environment.MachineName
                    : string.Empty;

                var consumeHouseEventParameter = new ConsumeMessageParameter<IHouseService, House>
                {
                    ExchangeName = "house.update.exchange",
                    ExchangeType = ExchangeType.Fanout,
                    RouteKey = string.Empty,
                    QueueName = "house.update.queue" + queueSubfix,
                    AdvancedBus = bus,
                    OnMessage = data =>
                    {
                        var houseService = serviceProvider.GetService<IHouseService>();
                        houseService.HandleUpdateEvent(data);
                    },
                    MaxRetryTime = 3
                };

                var consumeHouseEventParameter2 = new ConsumeMessageParameter<IHouseService, House>
                {
                    ExchangeName = "house.insert.exchange",
                    ExchangeType = ExchangeType.Direct,
                    RouteKey = new Random().Next(0, 2).ToString(),
                    QueueName = "house.insert.queue" + queueSubfix,
                    AdvancedBus = bus,
                    OnMessage = data =>
                    {
                        var houseService = serviceProvider.GetService<IHouseService>();
                        houseService.HandleInsertEvent(data);
                    },
                    MaxRetryTime = 3
                };

                this._rabbitMQHelper.RegisterConsumer(consumeHouseEventParameter);
                this._rabbitMQHelper.RegisterConsumer(consumeHouseEventParameter2);
            }
        }
    }
}