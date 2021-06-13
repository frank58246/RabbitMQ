using EasyNetQ;
using EasyNetQ.Consumer;
using RabbitMQ.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Service
{
    public class ConsumerParameter
    {
        public IAdvancedBus AdvancedBus { get; set; }

        public string QueueName { get; set; }
    }
}