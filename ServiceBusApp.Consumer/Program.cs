using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusApp.Common;
using ServiceBusApp.Common.Events;
using System.Text;

namespace ServisBusApp.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {

            ConsumeQueue<OrderCreatedEvent>(Constants.OrderCreatedQueueName, i =>
            {
                Console.WriteLine($"OrderCreatedEvent ReceivedMessageWith id:{i.Id},Name:{i.ProductName}");
            }).Wait();

            ConsumeQueue<OrderDeletedEvent>(Constants.OrderDeletedQueueName, i =>
            {
                Console.WriteLine($"OrderDeletedEvent ReceivedMessageWith id:{i.Id}");
            }).Wait(); ;

            Console.ReadLine();
        }


        private static async Task ConsumeQueue<T>(string queueName, Action<T> receivedAction)
        {
            IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);

            client.RegisterMessageHandler(async (message, ct) =>
            {

                var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));

                receivedAction(model);

                await Task.CompletedTask;
            }
            , new MessageHandlerOptions(i => Task.CompletedTask));


            Console.WriteLine($"{typeof(T).Name} is Listing....");
        }

    }
}