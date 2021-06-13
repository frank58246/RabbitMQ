using EasyNetQ.Consumer;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service.Implements
{
    public class HouseService : IHouseService
    {
        public async Task HandleAsync(byte[] bytes)
        {
            Console.WriteLine(bytes.ToString());
            await Task.CompletedTask;
        }
    }
}