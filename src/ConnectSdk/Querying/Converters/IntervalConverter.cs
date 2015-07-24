using System;
using Newtonsoft.Json;

namespace ConnectSdk.Querying.Converters
{
    public class IntervalConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timeframe = (RelativeTimefame)value;
            
            serializer.Serialize(writer, timeframe.Period);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (RelativeTimefame);
        }
    }
}