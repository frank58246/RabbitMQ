using Microsoft.AspNetCore.Mvc;
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
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IHouseService _houseService;

        public MessageController(IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpPost]
        [Route("house")]
        public async Task<Result> SendHouseAsync(House house)
        {
            return await this._houseService.SendUpdateEvent(house);
        }
    }
}