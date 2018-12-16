using Halforbit.MessageQueues.Interface;

namespace Halforbit.MessageQueues.Model
{
    public class Message : IMessage
    {
        public Message(
            MessageInfo messageInfo)
        {
            MessageInfo = messageInfo;
        }

        public MessageInfo MessageInfo { get; }
    }
}
