using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ConnectSdk
{
    public class Event
    {
        protected const string ReservedPrefix = "tp_";
        protected readonly JObject EventData;

        public Event(object eventData)
        {
            var data = JObject.FromObject(eventData);
            EventData = data;
            DefaultProperties();
            Validate();
        }

        public Event(string eventData)
        {
            var data = JObject.Parse(eventData);
            EventData = data;
            DefaultProperties();
            Validate();
        }

        public virtual string Id
        {
            get { return EventData["id"].ToString(); }
        }

        public virtual DateTime Timestamp
        {
            get { return (DateTime) EventData["timestamp"]; }
        }

        public void Validate()
        {
            var reservedProperties = 
                (from property in EventData.Properties()
                 where property.Name.StartsWith(ReservedPrefix, StringComparison.OrdinalIgnoreCase)
                 select property.Name).ToList();

            if (!reservedProperties.Any())
                return;

            throw new EventDataValidationException(ReservedPrefix, reservedProperties);
        }

        public void DefaultProperties()
        {
            EventData["id"] = EventData["id"] ?? Guid.NewGuid();
            EventData["timestamp"] = EventData["timestamp"] ?? DateTime.UtcNow;
        }

        public virtual JObject Data
        {
            get { return EventData; }
        }
    }
}