﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service.Interfaces
{
    public interface IHouseService
    {
        Task HandleAsync(byte[] bytes);
    }
}