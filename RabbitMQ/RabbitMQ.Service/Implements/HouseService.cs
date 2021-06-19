using EasyNetQ.Consumer;
using EasyNetQ.Topology;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Model;
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
        private readonly IRabbitMQHelper _rabbitMQHelper;

        public HouseService(IRabbitMQHelper rabbitMQHelper)
        {
            _rabbitMQHelper = rabbitMQHelper;
        }

        public void HandleMessage(string message)
        {
            Console.WriteLine(message);
        }

        public async Task<Result> SendUpdateEvent(House house)
        {
            var sendParameter = new SendMessageParameter<House>
            {
                Data = house,
                ExchangeName = "house",
                ExchangeType = ExchangeType.Direct
            };

            return await this._rabbitMQHelper.SendMessage(sendParameter);
        }
    }
}