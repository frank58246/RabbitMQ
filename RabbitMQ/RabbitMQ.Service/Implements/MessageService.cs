using EasyNetQ;
using RabbitMQ.Common.Messaging.Factory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
                    var bf = new BinaryFormatter();
                    using (var ms = new MemoryStream())
                    {
                        bf.Serialize(ms, parameter.Data);
                        body = ms.ToArray();
                    }

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