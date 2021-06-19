using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common.Model
{
    public class ConsumeMessageParameter<TConsumer, TResponse>
    {
        public IAdvancedBus AdvancedBus { get; set; }

        public string ExchangeName { get; set; }

        public string ExchangeType { get; set; }

        public string QueueName { get; set; }

        public string RouteKey { get; set; }

        public Action<TResponse> OnMessage { get; set; }

        public Action<TResponse> FallBack { get; set; }

        public int MaxRetryTime { get; set; }
    }
}