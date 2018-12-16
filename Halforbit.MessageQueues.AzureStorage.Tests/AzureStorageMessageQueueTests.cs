using Halforbit.MessageQueues.Model;
using Halforbit.MessageQueues.Serialization.Json.Implementation;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Halforbit.MessageQueues.AzureStorage.Tests
{
    public class AzureStorageMessageQueueTests
    {
        readonly IConfigurationRoot _configuration;

        public AzureStorageMessageQueueTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<AzureStorageMessageQueueTests>()
                .Build();
        }

        [Fact]
        public void Test1()
        {
            var connectionString = _configuration["Halforbit.MessageQueues.AzureStorage.Tests.ConnectionString"];

            var messageQueue = new AzureStorageMessageQueue<TestMessage>(
                new JsonMessageSerializer(),
                connectionString,
                "test-queue");

            var c = messageQueue.GetApproximateMessageCount().Result;

            messageQueue.Put(new TestMessage("Hi queue")).Wait();

            var messages = messageQueue.Peek().Result;
            
            var r = messageQueue.Get().Result;

            messageQueue.Delete(r).Wait();

            messageQueue.Clear().Wait();
        }

        class TestMessage : Message
        {
            public TestMessage(
                string message,
                MessageInfo messageInfo = null) : base(messageInfo)
            {
                Message = message;
            }

            public string Message { get; }
        }
    }
}
