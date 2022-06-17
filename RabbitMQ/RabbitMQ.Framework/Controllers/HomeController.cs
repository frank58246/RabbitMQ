using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace RabbitMQ.Framework.Controllers
{
    public class Message
    {
        public string Text { get; set; }
    }

    public class HomeController : Controller
    {
        private string exchangeName = "my.Exchange";
        private string routeKey = "my.routing";
        private string queueName = "my.queue";
        private IBus bus;

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            using (var bus = RabbitHutch.CreateBus("host=127.0.0.1;port=5672;username=admin;password=1qaz2wsx;virtualhost=message").Advanced)
            {
                var exchange = bus.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                var queue = bus.QueueDeclare(queueName);
                bus.Bind(exchange, queue, routeKey);
                bus.Publish(exchange, "my.routing", false, new Message<string>("Hello, world"));
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}