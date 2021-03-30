using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using QueuesFunctions.Interfaces;
using System.Threading.Tasks;

namespace QueuesFunctions.Services
{
    public class StorageQueueService : IQueueService
    {
        readonly QueueClient client;

        public StorageQueueService(string connectionString, string queueName)
        {
            client = new QueueClient(connectionString, queueName);           
        }

        public async Task Delete()
        {
            QueueProperties properties = await client.GetPropertiesAsync();

            if (properties.ApproximateMessagesCount == 0)
            {
                await client.DeleteIfExistsAsync();
            }
        }

        public async Task Insert(string message)
        {
            await client.SendMessageAsync(message, default, default, default);
        }

        public async Task<string> Get()
        {
            if (await client.ExistsAsync())
            {
                QueueProperties properties = await client.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    QueueMessage[] retrievedMessage = await client.ReceiveMessagesAsync(1);
                    var message = retrievedMessage[0].MessageText;
                    await client.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                    return message;
                }
            }

            return "The queue does not exist";
        }
    }
}
