using EasyNetQ;
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

        public void ConsumeMessage<TConsumer, TResponseType>(ConsumeMessageParameter<TConsumer, TResponseType> parameter)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> SendMessage<TDate>(SendMessageParameter<TDate> parameter)
        {
            using (var bus = this._busFactory.CrateBus())
            {
                try
                {
                    var exchange = await bus.ExchangeDeclareAsync(parameter.ExchangeName, _config =>
                    {
                        _config.WithType(parameter.ExchangeType.ToString());
                    });

                    var mandatory = false;

                    var routeKey = string.Empty; // routeKey 由consumer綁定

                    var messageProperties = new MessageProperties
                    {
                        DeliveryMode = 2 // persistent
                    };

                    var body = FormatHelper.ToByteArray(parameter.Data);

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