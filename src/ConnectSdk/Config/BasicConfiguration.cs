using Newtonsoft.Json;

namespace ConnectSdk.Config
{
    public class BasicConfiguration : IConfiguration
    {
        public BasicConfiguration(string apiKey, string projectId, string baseUrl = null, JsonSerializer eventSerializer = null)
        {
            BaseUrl = baseUrl ?? "https://api.getconnect.io";
            ApiKey = apiKey;
            ProjectId = projectId;
            EventSerializer = eventSerializer ?? new JsonSerializer
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        public virtual string BaseUrl { get; }

        public string ProjectId { get; }

        public virtual string ApiKey { get; }

        public virtual JsonSerializer EventSerializer { get; }
    }
}