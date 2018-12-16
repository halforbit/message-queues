using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Halforbit.MessageQueues.Interface
{
    public interface IMessageQueue<TMessage> 
        where TMessage : IMessage
    {
        Task Clear();

        Task Delete(TMessage message);

        Task<TMessage> Get(TimeSpan? visibilityTimeout = null);

        Task<int> GetApproximateMessageCount();

        Task<IReadOnlyList<TMessage>> Peek(int maxCount = 32);

        Task<bool> Put(
            TMessage message, 
            TimeSpan? timeToLive = null,
            TimeSpan? initialVisibilityDelay = null);
    }
}
