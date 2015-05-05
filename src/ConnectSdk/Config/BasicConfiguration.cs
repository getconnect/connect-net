namespace ConnectSdk.Config
{
    public class BasicConfiguration : IConfiguration
    {
        private readonly string _baseUrl;
        private readonly string _writeKey;

        public BasicConfiguration(string writeKey, string baseUrl = null)
        {
            _baseUrl = baseUrl ?? "https://api.getconnect.io";
            _writeKey = writeKey;
        }

        public virtual string BaseUrl
        {
            get { return _baseUrl; }
        }

        public virtual string WriteKey
        {
            get { return _writeKey; }
        }
    }
}