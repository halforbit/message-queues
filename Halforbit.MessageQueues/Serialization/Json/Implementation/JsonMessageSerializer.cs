using Halforbit.MessageQueues.Interface;
using Halforbit.MessageQueues.Model;
using Halforbit.MessageQueues.Serialization.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halforbit.MessageQueues.Serialization.Json.Implementation
{
    public class JsonMessageSerializer :
        IMessageSerializer
    {
        static readonly Encoding _utf8Encoding = new UTF8Encoding(false);

        public async Task<TMessage> Deserialize<TMessage>(
            byte[] bytes,
            MessageInfo messageInfo) 
            where TMessage : IMessage
        {
            return await Task.Run(() =>
            {
                var jObject = JObject.Parse(_utf8Encoding.GetString(bytes));

                jObject[nameof(MessageInfo)] = JObject.FromObject(messageInfo);

                return jObject.ToObject<TMessage>();
            });
        }

        public async Task<byte[]> Serialize<TMessage>(
            TMessage message)
            where TMessage : IMessage
        {
            return await Task.Run(() =>
            {
                var jObject = JObject.FromObject(message);

                jObject.Remove(nameof(MessageInfo));

                return _utf8Encoding.GetBytes(jObject.ToString(Formatting.None));
            });
        }
    }
}
