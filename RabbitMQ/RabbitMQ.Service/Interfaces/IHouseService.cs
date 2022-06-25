using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service.Interfaces
{
    public interface IHouseService
    {
        Task<Result> SendUpdateEvent(House house);

        Task<Result> SendInsertEvent(House house, string routingKey);

        void HandleUpdateEvent(House house);

        void HandleInsertEvent(House house);
    }
}