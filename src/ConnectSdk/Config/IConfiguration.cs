﻿using Newtonsoft.Json;

namespace ConnectSdk.Config
{
    public interface IConfiguration
    {
        string ProjectId { get; }
        string BaseUrl { get; }
        string ApiKey { get; }
        JsonSerializer EventSerializer { get; }
    }
}