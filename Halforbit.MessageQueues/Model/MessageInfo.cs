using System;

namespace Halforbit.MessageQueues.Model
{
    public class MessageInfo
    {
        public MessageInfo(
            string id,
            DateTime? insertionTime,
            DateTime? expirationTime,
            DateTime? nextVisibleTime,
            int dequeueCount,
            string popReceipt)
        {
            Id = id;

            InsertionTime = insertionTime;

            ExpirationTime = expirationTime;

            NextVisibleTime = nextVisibleTime;

            DequeueCount = dequeueCount;

            PopReceipt = popReceipt;
        }

        public string Id { get; }

        public DateTime? InsertionTime { get; }

        public DateTime? ExpirationTime { get; }

        public DateTime? NextVisibleTime { get; }

        public int DequeueCount { get; }

        public string PopReceipt { get; }
    }
}
