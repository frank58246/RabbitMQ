using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RabbitMQ.Framework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private string exchangeName = "my.Exchange";
        private string routeKey = "my.routing";
        private string queueName = "my.queue";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var bus = RabbitHutch.CreateBus("host=127.0.0.1;port=5672;username=admin;password=1qaz2wsx;virtualhost=message");

            bus.SendReceive.Receive<string>(queueName, (m) =>
            {
                Console.WriteLine(m);
            });
        }
    }
}