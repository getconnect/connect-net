using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectSdk.Querying.Converters
{
    public class AggregationConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var aggregation = (Aggregation)value;

            if (aggregation.PropertyPath == null)
            {
                serializer.Serialize(writer, aggregation.Operator);
            }
            else
            {
                serializer.Serialize(writer, new Dictionary<string, string>
                {
                    [aggregation.Operator] = aggregation.PropertyPath
                });
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Aggregation);
        }
    }
}