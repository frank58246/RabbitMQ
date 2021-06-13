using RabbitMQ.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Service
{
    public class SendMessageParameter<T>
    {
        public T Data { get; set; }

        public string ExchangeName { get; set; }

        public ExchangeTypeEnum ExchangeType { get; set; }

        public string RouteKey { get; set; }
    }
}