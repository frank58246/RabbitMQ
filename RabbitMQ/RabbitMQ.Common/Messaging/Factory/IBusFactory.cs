using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Common.Messaging.Factory
{
    public interface IBusFactory
    {
        IAdvancedBus CrateBus();
    }
}