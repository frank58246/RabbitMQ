using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common.Messaging.Model
{
    public class SendMessageParameter<T>
    {
        public T Data { get; set; }

        public string ExchangeName { get; set; }

        public string ExchangeType { get; set; }

        public string RoutingKey { get; set; } = string.Empty;
    }
}