using Halforbit.Facets.Attributes;
using System;

namespace Halforbit.MessageQueues.AzureStorage.Facets
{
    public class QueueName : FacetParameterAttribute
    {
        public QueueName(string value = null, string configKey = null) : base(value, configKey) { }

        public override string ParameterName => "queueName";

        public override Type TargetType => typeof(AzureStorageMessageQueue<>);
    }
}
