using System;
using Halforbit.Facets.Attributes;

namespace Halforbit.MessageQueues.AzureStorage.Facets
{
    public class QueueConnectionStringAttribute : FacetParameterAttribute
    {
        public QueueConnectionStringAttribute(string value = null, string configKey = null) : base(value, configKey) { }

        public override string ParameterName => "connectionString";

        public override Type TargetType => typeof(AzureStorageMessageQueue<>);
    }
}
