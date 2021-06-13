using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common.Messaging
{
    public interface IMessageHandler
    {
        void HandleAsync(byte[] bytes);
    }
}