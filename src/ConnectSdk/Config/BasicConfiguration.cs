using Newtonsoft.Json;

namespace ConnectSdk.Config
{
    public class BasicConfiguration : IConfiguration
    {
        private readonly string _baseUrl;
        private readonly string _pushKey;
        private readonly string _projectId;
        private readonly JsonSerializer _serializer;

        public BasicConfiguration(string pushKey, string projectId, string baseUrl = null, JsonSerializer serializer = null)
        {
            _baseUrl = baseUrl ?? "https://api.getconnect.io";
            _pushKey = pushKey;
            _projectId = projectId;
            _serializer = serializer ?? new JsonSerializer
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        public virtual string BaseUrl
        {
            get { return _baseUrl; }
        }

        public string ProjectId
        {
            get { return _projectId; }
        }

        public virtual string WriteKey
        {
            get { return _pushKey; }
        }

        public virtual JsonSerializer Serializer
        {
            get { return _serializer; }
        }
    }
}