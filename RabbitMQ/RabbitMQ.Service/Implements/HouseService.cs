using EasyNetQ.Consumer;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service.Implements
{
    public class HouseService : IHouseService
    {
        public void HandleMessage(string message)
        {
            Console.WriteLine(message);
            
        }
    }
}