using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ConnectSdk.Api
{
    public static class RequestBodyGenerator
    {
        public static string GenerateEventBody(Event eventData)
        {
            return eventData.Data.ToString();
        }

        public static string GenerateBatchBody(IDictionary<string, IEnumerable<Event>> eventDataByCollection)
        {
            return eventDataByCollection.Aggregate(new JObject(), (json, edbc) =>
            {
                var collectionName = edbc.Key;
                json[collectionName] = edbc.Value.Aggregate(new JArray(), (events, ed) =>
                {
                    events.Add(ed.Data);
                    return events;
                });
                return json;
            }).ToString();
        }
    }
}