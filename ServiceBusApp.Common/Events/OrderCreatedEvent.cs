﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusApp.Common.Events
{
    public class OrderCreatedEvent : EventBase
    {
        public string ProductName { get; set; }
    }
}
