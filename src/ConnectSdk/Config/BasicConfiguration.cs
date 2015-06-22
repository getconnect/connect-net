namespace ConnectSdk.Config
{
    public class BasicConfiguration : IConfiguration
    {
        private readonly string _baseUrl;
        private readonly string _pushKey;
        private readonly string _projectId;

        public BasicConfiguration(string pushKey, string projectId, string baseUrl = null)
        {
            _baseUrl = baseUrl ?? "https://api.getconnect.io";
            _pushKey = pushKey;
            _projectId = projectId;
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
    }
}