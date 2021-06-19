using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Service.Model
{
    [Serializable]
    public class House
    {
        public string Caption { get; set; }

        public double UpPrice { get; set; }

        public double DownPrice { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}