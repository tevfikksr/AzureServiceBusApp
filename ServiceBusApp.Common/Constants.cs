using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusApp.Common
{
    public static class Constants
    {
        public const string ConnectionString = "Endpoint=sb://tevfikkeser.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rMxrHQR3Gd0JhO0DxD0tI1/VPYlWIajPZ+ASbOlNzpY=";

        public const string OrderCreatedQueueName = "OrderCreatedQueue";
        public const string OrderDeletedQueueName = "OrderDeleteQueue";

        public const string OrderTopic = "OrderTopic";
        public const string OrderCreatedSub = "OrderCreatedSub";
        public const string OrderDeletedSubName = "OrderDeletedSub";
    }
}
