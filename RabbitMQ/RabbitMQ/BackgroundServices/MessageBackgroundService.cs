﻿using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Common.Helpers;
using RabbitMQ.Common.Messaging.Factory;
using RabbitMQ.Service.Interfaces;
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

        public MessageBackgroundService(IBusFactory busFactory,
            IServiceScopeFactory serviceScopeFactory)
        {
            _busFactory = busFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bus = this._busFactory.CrateBus();

            var serviceProvider = this._serviceScopeFactory.CreateScope().ServiceProvider;

            var queue = await bus.QueueDeclareAsync("simpleQueue", service => { });
            bus.Consume(queue, (bytes, properties, info) =>
            {
                var houseService = serviceProvider.GetService<IHouseService>();

                var stringValue = FormatHelper.ToObject<string>(bytes);

                houseService.HandleMessage(stringValue);
            });
        }
    }
}