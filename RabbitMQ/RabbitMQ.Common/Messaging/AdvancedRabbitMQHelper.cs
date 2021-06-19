using RabbitMQ.Common.Messaging.Factory;
using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Common.Messaging
{
    public class AdvancedRabbitMQHelper : IRabbitMQHelper
    {
        private readonly IRabbitMQHelper _basicRabbitMQHelper;

        private readonly IBusFactory _busFactory;

        public AdvancedRabbitMQHelper(IRabbitMQHelper basicRabbitMQHelper,
            IBusFactory busFactory)
        {
            this._basicRabbitMQHelper = basicRabbitMQHelper;
            this._busFactory = busFactory;
        }

        public void ConsumeMessage<TConsumer, TResponseType>(ConsumeMessageParameter<TConsumer, TResponseType> parameter)
        {
            throw new NotImplementedException();
        }

        public Task<Result> SendMessage<TData>(SendMessageParameter<TData> parameter)
        {
            throw new NotImplementedException();
        }
    }
}