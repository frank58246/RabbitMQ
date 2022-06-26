using EasyNetQ.Topology;
using Newtonsoft.Json;
using RabbitMQ.Common.Filter;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Model;
using System;
using System.Threading;
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

        public void HandleInsertEvent(House house)
        {
            Console.WriteLine(JsonConvert.SerializeObject(house) + "is inserted");
        }

        public void HandleUpdateEvent(House house)
        {
            var delay = Environment.GetEnvironmentVariable("DELAY_SECOND");
            int.TryParse(delay, out var delaySecond);

            Thread.Sleep(TimeSpan.FromSeconds(delaySecond));
            Console.WriteLine(JsonConvert.SerializeObject(house) + "is updated");
        }

        public async Task<Result> SendInsertEvent(House house, string routingKey)
        {
            var sendParameter = new SendMessageParameter<House>
            {
                Data = house,
                ExchangeName = "house.insert.exchange",
                ExchangeType = ExchangeType.Direct,
                RoutingKey = routingKey
            };

            return await this._rabbitMQHelper.SendMessage(sendParameter);
        }

        [Profile]
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