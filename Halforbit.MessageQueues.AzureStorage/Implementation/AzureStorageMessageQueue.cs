using Halforbit.MessageQueues.Interface;
using Halforbit.MessageQueues.Model;
using Halforbit.MessageQueues.Serialization.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halforbit.MessageQueues.AzureStorage
{
    public class AzureStorageMessageQueue<TMessage> : 
        IMessageQueue<TMessage>
        where TMessage : IMessage
    {
        static readonly Encoding _utf8Encoding = new UTF8Encoding(false);

        readonly Lazy<CloudQueue> _cloudQueue;

        readonly IMessageSerializer _messageSerializer;

        public AzureStorageMessageQueue(
            IMessageSerializer messageSerializer,
            string connectionString, 
            string queueName)
        {
            _cloudQueue = new Lazy<CloudQueue>(() => CloudStorageAccount
                .Parse(connectionString)
                .CreateCloudQueueClient()
                .GetQueueReference(queueName));

            _messageSerializer = messageSerializer;
        }

        public async Task Clear()
        {
            await _cloudQueue.Value.ClearAsync();
        }

        public async Task Delete(TMessage message)
        {
            await _cloudQueue.Value.DeleteMessageAsync(
                message.MessageInfo.Id, 
                message.MessageInfo.PopReceipt);
        }

        public async Task<TMessage> Get(TimeSpan? visibilityTimeout = null)
        {
            visibilityTimeout = visibilityTimeout ?? TimeSpan.FromMinutes(5);

            var message = await _cloudQueue.Value.GetMessageAsync(visibilityTimeout, null, null);
            
            return await _messageSerializer.Deserialize<TMessage>(
                message.AsBytes,
                new MessageInfo(
                    message.Id,
                    message.InsertionTime.Value.UtcDateTime,
                    message.ExpirationTime.Value.UtcDateTime,
                    message.NextVisibleTime.Value.UtcDateTime,
                    message.DequeueCount,
                    message.PopReceipt));
        }

        public async Task<int> GetApproximateMessageCount()
        {
            await _cloudQueue.Value.FetchAttributesAsync();

            return _cloudQueue.Value.ApproximateMessageCount ?? 0;
        }

        public async Task<IReadOnlyList<TMessage>> Peek(int maxCount = 32)
        {
            var messages = await _cloudQueue.Value.PeekMessagesAsync(maxCount);

            var deserializeTasks = messages
                .Select(async m => await _messageSerializer.Deserialize<TMessage>(
                    m.AsBytes, 
                    new MessageInfo(
                        m.Id,
                        m.InsertionTime?.UtcDateTime,
                        m.ExpirationTime?.UtcDateTime,
                        m.NextVisibleTime?.UtcDateTime,
                        m.DequeueCount,
                        m.PopReceipt)))
                .ToArray();

            await Task.WhenAll(deserializeTasks);

            return deserializeTasks
                .Select(t => t.Result)
                .ToList();
        }

        public async Task<bool> Put(
            TMessage message,
            TimeSpan? timeToLive = null,
            TimeSpan? initialVisibilityDelay = null)
        {
            var cloudMessage = new CloudQueueMessage(
                _utf8Encoding.GetString(await _messageSerializer.Serialize(message)));

            await _cloudQueue.Value.AddMessageAsync(
                cloudMessage,
                timeToLive,
                initialVisibilityDelay,
                null,
                null);

            return true;
        }
    }
}
