using Newtonsoft.Json;

namespace ConnectSdk.Config
{
    public class BasicConfiguration : IConfiguration
    {
        public BasicConfiguration(string apiKey, string projectId, string baseUrl = null, JsonSerializer eventSerializer = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(projectId))
            {
                throw new ConnectInitializationException("Both an apiKey and a projectId must be supplied to the configuration to authorize with the Connect API. " +
                                                         "These can be retrieved from https://api.getconnect.io");
            }

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