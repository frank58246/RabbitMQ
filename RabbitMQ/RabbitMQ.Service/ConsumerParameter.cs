using EasyNetQ;
using EasyNetQ.Consumer;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Service
{
    public class ConsumerParameter
    {
        public IAdvancedBus AdvancedBus { get; set; }

        public string QueueName { get; set; }

        public IMessageHandler<byte[]> MessageHandler { get; set; }
    }
}