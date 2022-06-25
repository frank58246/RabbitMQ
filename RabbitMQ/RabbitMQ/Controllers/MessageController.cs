using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Common.Messaging.Model;
using RabbitMQ.Service;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Model;

using RabbitMQ.Service.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MessageController : ControllerBase
    {
        private readonly IHouseService _houseService;

        private readonly ILogger<MessageController> _logger;

        public MessageController(IHouseService houseService, ILogger<MessageController> logger)
        {
            _houseService = houseService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<Result> SendUpdateHouseAsync(House house)
        {
            return await this._houseService.SendUpdateEvent(house);
        }

        [HttpPost]
        public async Task<Result> SendInsertHouseAsync(House house, string rountingKey)
        {
            return await this._houseService.SendInsertEvent(house, rountingKey);
        }
    }
}