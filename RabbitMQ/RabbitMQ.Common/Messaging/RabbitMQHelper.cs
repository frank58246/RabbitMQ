using EasyNetQ;
using RabbitMQ.Common.Filter;
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
    public class RabbitMQHelper : IRabbitMQHelper
    {
        private readonly IBusFactory _busFactory;

        public RabbitMQHelper(IBusFactory busFactory)
        {
            this._busFactory = busFactory;
        }

        public void RegisterConsumer<TConsumer, TResponseType>(ConsumeMessageParameter<TConsumer, TResponseType> parameter)
        {
            var bus = parameter.AdvancedBus;
            var exchange = bus.ExchangeDeclare(parameter.ExchangeName, parameter.ExchangeType);
            var queue = bus.QueueDeclare(parameter.QueueName);
            bus.Bind(exchange, queue, parameter.RouteKey);

            bus.Consume(queue, (bytes, properties, info) =>
            {
                var data = FormatHelper.ToObject<TResponseType>(bytes);
                parameter.OnMessage.Invoke(data);
            });
        }

        [Profile]
        public async Task<Result> SendMessage<TDate>(SendMessageParameter<TDate> parameter)
        {
            using (var bus = this._busFactory.CrateBus())
            {

                var exchange = await bus.ExchangeDeclareAsync(parameter.ExchangeName, _config =>
                {
                    _config.WithType(parameter.ExchangeType.ToString());
                });

                var mandatory = false;

                var routeKey = parameter.RoutingKey;

                var messageProperties = new MessageProperties
                {
                    DeliveryMode = 2 // persistent
                };

                var body = FormatHelper.ToByteArray(parameter.Data);

                await bus.PublishAsync(exchange, routeKey, mandatory, messageProperties, body);

                return new Result { Success = true };


            }
        }
    }
}