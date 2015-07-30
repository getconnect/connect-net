using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ConnectSdk.Querying.Converters
{
    public class FiltersConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var values = (IDictionary<string, IEnumerable<Filter>>) value;

            writer.WriteStartObject();
            foreach (var propertyFilters in values)
            {
                writer.WritePropertyName(propertyFilters.Key);
                var filtersForProperty = propertyFilters.Value.ToDictionary(filter => filter.Operator, filter => filter.Value);
                serializer.Serialize(writer, filtersForProperty);
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
            return objectType == typeof(Dictionary<string, IEnumerable<Filter>>);
        }
    }
}