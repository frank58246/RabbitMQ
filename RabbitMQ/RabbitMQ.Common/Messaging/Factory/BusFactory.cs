using EasyNetQ;
using RabbitMQ.Common.Messaging.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Common.Messaging.Factory
{
    public class BusFactory : IBusFactory
    {
        private readonly ConnectionConfiguration _config;

        public BusFactory(RabbitMQSettings setting)
        {
            var config = new ConnectionConfiguration
            {
                UserName = setting.UserName,
                Password = setting.Password,
                VirtualHost = setting.VirtualHost,
                Hosts = setting.Hosts.Select(x =>
                {
                    var host = new HostConfiguration();

                    host.Host = x.Host;
                    host.Port = x.Port;
                    host.Ssl.Enabled = x.SslEnable;
                    host.Ssl.CertPath = x.SslCertPath;
                    host.Ssl.ServerName = x.SslServcerName;

                    return host;
                }).ToList()
            };

            this._config = config;
        }

        public IAdvancedBus CrateBus()
        {
            return RabbitHutch.CreateBus(this._config, service => { }).Advanced;
        }
    }
}