using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using ServiceBusApp.Common;
using System.Text;

namespace ServiceBusApp.ProducerApi.Controllers.Services
{
    public class AzureService
    {
        private readonly ManagementClient managementClient;
        public AzureService(ManagementClient managementClient)
        {

            this.managementClient = managementClient;

        }
        public async Task SendMessageToQueue(string queueName, object messageContent, string messageType = null)
        {
            IQueueClient queueClient = new QueueClient(Constants.ConnectionString, queueName);

            await SendMessage(queueClient, messageContent, messageType);
        }

        public async Task CreateQueueIfNotExist(string queueName)
        {
            if (!await managementClient.QueueExistsAsync(queueName))
            {
                await managementClient.CreateQueueAsync(queueName);
            }
        }

        public async Task SendMassageToTopic(string topicName, object messageContent, string messageType = null)
        {
            ITopicClient client = new TopicClient(Constants.ConnectionString, topicName);

            await SendMessage(client, messageContent, messageType);
        }

        public async Task CreateSubscriptionIfNotExist(string topicName, string subsName, string messageType = null, string ruleName = null)
        {
            if (await managementClient.SubscriptionExistsAsync(topicName, subsName))
                return;

            if (messageType != null)
            {
                SubscriptionDescription sd = new(topicName, subsName);

                CorrelationFilter filter = new();
                filter.Properties["MessageType"] = messageType;

                RuleDescription rd = new(ruleName ?? messageType + "Rule", filter);

                await managementClient.CreateSubscriptionAsync(sd, rd);
            }
            else
            {
                await managementClient.CreateSubscriptionAsync(topicName, subsName);
            }


        }

        public async Task CreateTopicIfNotExist(string queueName)
        {
            if (!await managementClient.TopicExistsAsync(queueName))
                await managementClient.CreateTopicAsync(queueName);
        }

        private async Task SendMessage(ISenderClient client, object messageContent, string messageType = null)
        {
            var byteArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));

            var message = new Message(byteArr);
            message.UserProperties["MessageType"] = messageType;
            await client.SendAsync(message);
        }
    }
}
