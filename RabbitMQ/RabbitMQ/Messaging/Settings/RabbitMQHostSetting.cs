using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Messaging.Settings
{
    public class RabbitMQHostSetting
    {
        public string Host { get; set; }

        public ushort Port { get; set; }

        public bool SslEnable { get; set; }

        public string SslCertPath { get; set; }

        public string SslServcerName { get; set; }
    }
}