using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceBusApp.Common;
using ServiceBusApp.Common.Events;
using ServiceBusApp.Common.Models;
using ServiceBusApp.ProducerApi.Controllers.Services;

namespace ServiceBusApp.ProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AzureService azureService;

        public OrderController(AzureService azureService)
        {
            this.azureService = azureService;
        }

        [HttpPost]
        public async Task CreateOrder(OrderDto order)
        {
            var orderCreatedEvent = new OrderCreatedEvent()
            {
                Id = order.Id,
                CreateOn = DateTime.Now,
                ProductName = order.ProductName
            };

            //await azureService.CreateQueueIfNotExist(Constants.OrderCreatedQueueName);
            //await azureService.SendMessageToQueue(Constants.OrderCreatedQueueName, orderCreatedEvent);

            await azureService.CreateTopicIfNotExist(Constants.OrderTopic);
            await azureService.CreateSubscriptionIfNotExist(Constants.OrderTopic, Constants.OrderCreatedSub, "OrderCreated", "OrderCreatedOnly");

            await azureService.SendMassageToTopic(Constants.OrderTopic, orderCreatedEvent, "OrderCreated");
        }

        [HttpDelete("{id}")]
        public async Task DelteOrder(int id)
        {
            var orderDeletedEvent = new OrderDeletedEvent()
            {
                Id = id,
                CreateOn = DateTime.Now,
            };

            //await azureService.CreateQueueIfNotExist(Constants.OrderDeletedQueueName);
            //await azureService.SendMessageToQueue(Constants.OrderDeletedQueueName, orderDeletedEvent);

            await azureService.CreateTopicIfNotExist(Constants.OrderTopic);
            await azureService.CreateSubscriptionIfNotExist(Constants.OrderTopic, Constants.OrderDeletedSubName, "OrderDeleted", "OrderDeletedOnly");

            await azureService.SendMassageToTopic(Constants.OrderTopic, orderDeletedEvent, "OrderDeleted");
        }
    }
}
