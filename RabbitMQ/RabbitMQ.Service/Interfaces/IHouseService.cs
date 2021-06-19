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

        void HandleMessage(string message);
    }
}