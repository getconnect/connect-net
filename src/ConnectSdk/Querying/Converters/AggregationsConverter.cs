using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectSdk.Querying.Converters
{
    public class AggregationsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var values = (IDictionary<string, Aggregation>) value;

            writer.WriteStartObject();
            foreach (var aliasedAggregation in values)
            {
                writer.WritePropertyName(aliasedAggregation.Key);
                serializer.Serialize(writer, aliasedAggregation.Value);
            }
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, Aggregation>);
        }
    }
}