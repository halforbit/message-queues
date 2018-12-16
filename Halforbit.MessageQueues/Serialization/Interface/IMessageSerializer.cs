using Halforbit.MessageQueues.Interface;
using Halforbit.MessageQueues.Model;
using System.Threading.Tasks;

namespace Halforbit.MessageQueues.Serialization.Interface
{
    public interface IMessageSerializer
    {
        Task<byte[]> Serialize<TMessage>(TMessage message)
            where TMessage : IMessage;

        Task<TMessage> Deserialize<TMessage>(
            byte[] bytes,
            MessageInfo messageInfo)
            where TMessage : IMessage;
    }
}
