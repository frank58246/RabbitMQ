using EasyNetQ;
using RabbitMQ.Common.Messaging.Factory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service
{
    public class MessageService : IMessageService
    {
        private readonly IBusFactory _busFactory;

        public MessageService(IBusFactory busFactory)
        {
            _busFactory = busFactory;
        }

        public async Task RegisterConsumer(ConsumerParameter parameter)
        {
            var bus = parameter.AdvancedBus;
            var queue = await bus.QueueDeclareAsync(parameter.QueueName, service => { });
            bus.Consume(queue, registration =>
            {
                registration.Add<byte[]>(parameter.MessageHandler);
            });
        }

        public async Task<Result> SendMessage<T>(SendMessageParameter<T> parameter)
        {
            using (var bus = this._busFactory.CrateBus())
            {
                try
                {
                    var exchange = await bus.ExchangeDeclareAsync(parameter.ExchangeName, _config =>
                    {
                        _config.WithType(parameter.ExchangeType.ToString());
                    });

                    var routeKey = parameter.RouteKey;

                    var mandatory = false;

                    var messageProperties = new MessageProperties
                    {
                        DeliveryMode = 2 // persistent
                    };

                    var body = new byte[] { };

                    await bus.PublishAsync(exchange, routeKey, mandatory, messageProperties, body);

                    return new Result { Success = true };
                }
                catch (Exception e)
                {
                    var result = new Result
                    {
                        Success = false,
                        ErrorMessage = e.Message,
                        StackTrace = e.StackTrace
                    };

                    return result;
                }
            }
        }
    }
}