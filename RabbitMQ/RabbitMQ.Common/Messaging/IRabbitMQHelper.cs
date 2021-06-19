using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Common.Messaging
{
    public interface IRabbitMQHelper
    {
        Task<Result> SendMessage<TData>(SendMessageParameter<TData> parameter);

        void ConsumeMessage<TConsumer, TResponseType>
                (ConsumeMessageParameter<TConsumer, TResponseType> parameter);
    }
}