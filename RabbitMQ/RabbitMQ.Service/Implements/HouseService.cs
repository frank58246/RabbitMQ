using EasyNetQ.Topology;
using Newtonsoft.Json;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Model;
using System;
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

        public void HandleUpdateEvent(House house)
        {
            Console.WriteLine(JsonConvert.SerializeObject(house));
        }

        public async Task<Result> SendUpdateEvent(House house)
        {
            var sendParameter = new SendMessageParameter<House>
            {
                Data = house,
                ExchangeName = "house.update.exchange",
                ExchangeType = ExchangeType.Fanout
            };

            return await this._rabbitMQHelper.SendMessage(sendParameter);
        }
    }
}