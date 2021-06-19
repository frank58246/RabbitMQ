using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Common.Messaging.Model
{
    public class Result
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }
    }
}