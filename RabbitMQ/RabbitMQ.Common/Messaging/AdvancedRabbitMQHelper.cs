using EasyNetQ;
using EasyNetQ.Topology;
using RabbitMQ.Common.Helpers;
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

        public void RegisterConsumer<TConsumer, TResponseType>(ConsumeMessageParameter<TConsumer, TResponseType> parameter)
        {
            if (parameter.MaxRetryTime <= 0 || parameter.FallBack is null)
            {
                this._basicRabbitMQHelper.RegisterConsumer(parameter);
                return;
            }

            var bus = parameter.AdvancedBus;
            var exchange = bus.ExchangeDeclare(parameter.ExchangeName, parameter.ExchangeType);

            var queue = bus.QueueDeclare(parameter.QueueName, configure =>
            {
                var deadExchange = this.GetDeadLetterExchange(bus, exchange, parameter.QueueName);
                configure.WithDeadLetterExchange(deadExchange);
            });

            bus.Bind(exchange, queue, parameter.RouteKey);

            bus.Consume(queue, (bytes, properties, info) =>
            {
                var data = FormatHelper.ToObject<TResponseType>(bytes);
                parameter.OnMessage.Invoke(data);
            });
        }

        public async Task<Result> SendMessage<TData>(SendMessageParameter<TData> parameter)
        {
            return await this._basicRabbitMQHelper.SendMessage(parameter);
        }

        private IExchange GetDeadLetterExchange(IAdvancedBus bus,
            IExchange originExchange,
            string orgingQueueName)
        {
            var suffix = "wait";
            var waitExchangeName = $"{originExchange.Name}.{suffix}";
            var waitExchangeType = ExchangeType.Direct;
            var waitQueueName = $"{orgingQueueName}.{suffix}";

            var waitExchange = bus.ExchangeDeclare(waitExchangeName, waitExchangeType);

            var waitQueue = bus.QueueDeclare(waitQueueName, configure =>
            {
                // 因為沒有consumer，30秒後自動進入deadLetter，即原來的exchange
                configure.WithMessageTtl(TimeSpan.FromSeconds(30));
                configure.WithDeadLetterExchange(originExchange);
            });

            bus.Bind(waitExchange, waitQueue, string.Empty);

            return waitExchange;
        }
    }
}