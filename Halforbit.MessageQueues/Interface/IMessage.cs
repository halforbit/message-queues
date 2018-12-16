using Halforbit.MessageQueues.Model;

namespace Halforbit.MessageQueues.Interface
{
    public interface IMessage
    {
        MessageInfo MessageInfo { get; }
    }
}
