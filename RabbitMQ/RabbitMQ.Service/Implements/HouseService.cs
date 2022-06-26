﻿using EasyNetQ.Topology;
using Newtonsoft.Json;
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

        public async Task<Result> SendUpdateEvent(House house)
        {
            var sendParameter = new SendMessageParameter<House>
            {
                Data = house,
                ExchangeName = "house.update.exchange",
                ExchangeType = ExchangeType.Fanout
            };

            var delay = Environment.GetEnvironmentVariable("DELAY") == "y"
                         ? 2
                         : 0;
            Thread.Sleep(delay);

            return await this._rabbitMQHelper.SendMessage(sendParameter);
        }
    }
}