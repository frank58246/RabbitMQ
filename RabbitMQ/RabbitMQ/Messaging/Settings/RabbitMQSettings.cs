﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Messaging.Settings
{
    public class RabbitMQSettings
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public List<RabbitMQHostSetting> Hosts { get; set; }
    }
}