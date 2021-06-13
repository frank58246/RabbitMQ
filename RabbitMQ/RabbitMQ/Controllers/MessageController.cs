using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Common.Enums;
using RabbitMQ.Service;
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
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpPost]
        public async Task<Result> SendSimpleStringAsync(string value)
        {
            var parameter = new SendMessageParameter<string>
            {
                Data = value,
                ExchangeName = "message",
                ExchangeType = ExchangeTypeEnum.direct,
                RouteKey = "simpleQueue"
            };

            return await this.messageService.SendMessage(parameter);
        }
    }
}