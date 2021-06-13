using System;
using System.Threading.Tasks;

namespace RabbitMQ.Service
{
    public interface IMessageService
    {
        Task<Result> SendMessage<T>(SendMessageParameter<T> parameter);

        Task RegisterConsumer(ConsumerParameter parameter);
    }
}