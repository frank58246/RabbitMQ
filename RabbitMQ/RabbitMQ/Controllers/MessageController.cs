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
    [Route("[controller]")]
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
        [Route("house")]
        public async Task<Result> SendHouseAsync(House house)
        {
            try
            {
                return await this._houseService.SendUpdateEvent(house);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return new Result();
            }
            
        }
    }
}