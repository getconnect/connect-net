using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectSdk.Querying.Converters
{
    public class PeriodRelativeTimefameConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timeframe = (PeriodRelativeTimefame)value;
            
            serializer.Serialize(writer, new Dictionary<string, Dictionary<TimeType, int>>
            {
                [timeframe.Period] = new Dictionary<TimeType, int>
                {
                    [timeframe.TimeType] = timeframe.Value
                }
            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (PeriodRelativeTimefame);
        }
    }
}