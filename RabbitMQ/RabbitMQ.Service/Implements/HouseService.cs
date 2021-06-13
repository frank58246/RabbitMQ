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
        public async Task HandleAsync(byte[] bytes)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                object obj = bf.Deserialize(ms);
                var message = (string)obj;
                Console.WriteLine(message);
            }
        }
    }
}