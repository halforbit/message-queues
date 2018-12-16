using Halforbit.Facets.Attributes;
using Halforbit.MessageQueues.Serialization.Json.Implementation;
using System;

namespace Halforbit.MessageQueues.Serialization.Json.Facets
{
    public class JsonMessageSerialization : FacetAttribute
    {
        public override Type TargetType => typeof(JsonMessageSerializer);
    }
}
