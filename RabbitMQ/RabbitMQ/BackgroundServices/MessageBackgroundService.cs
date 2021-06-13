using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Common.Messaging.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var bus = this._busFactory.CrateBus();

            throw new NotImplementedException();
        }
    }
}