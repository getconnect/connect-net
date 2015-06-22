namespace ConnectSdk.Config
{
    public interface IConfiguration
    {
        string ProjectId { get; }
        string BaseUrl { get; }
        string WriteKey { get; }
    }
}