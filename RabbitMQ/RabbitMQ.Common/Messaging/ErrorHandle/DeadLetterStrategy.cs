using EasyNetQ;
using EasyNetQ.Consumer;
using RabbitMQ.Client;
using RabbitMQ.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common.Messaging.ErrorHandle
{
    public class DeadLetterStrategy : DefaultConsumerErrorStrategy
    {
        // 系統容忍最大重試次數
        private const int MAX_RETRY_COUNT = 10;

        public DeadLetterStrategy(IPersistentConnection connection, ISerializer serializer, IConventions conventions, ITypeNameSerializer typeNameSerializer, IErrorMessageSerializer errorMessageSerializer, ConnectionConfiguration configuration) : base(connection, serializer, conventions, typeNameSerializer, errorMessageSerializer, configuration)
        {
        }

        public override AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception)
        {
            object deathHeaderObject;
            if (!context.Properties.Headers.TryGetValue("x-death", out deathHeaderObject))
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var deathHeaders = deathHeaderObject as IList<object>;

            if (deathHeaders == null)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var retries = 0;
            foreach (IDictionary<string, object> header in deathHeaders)
            {
                var count = int.Parse(header["count"].ToString());
                var exchangeName = Encoding.UTF8.GetString(header["exchange"] as byte[]);
                var originExchange = context.ReceivedInfo.Exchange;

                // 因為會經過waitExchange的deadLetter，死亡次數只算原本的exchange
                if (exchangeName == originExchange)
                {
                    retries += count;
                }
            }
            Console.WriteLine($"retry time: {retries}");
            if (retries < MAX_RETRY_COUNT)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            return base.HandleConsumerError(context, exception);
        }
    }
}