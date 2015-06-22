using System;
using System.Net.Http;
using System.Net.Http.Headers;
using ConnectSdk.Api;
using ConnectSdk.Config;

namespace ConnectSdk.Tests.Api
{
    public class CaptureHttpEventEndpoint : HttpEventEndpoint
    {
        private readonly CaptureHttpHandler _captureHttpHandler;

        public CaptureHttpEventEndpoint(IConfiguration configuration, CaptureHttpHandler captureHttpHandler) : base(configuration)
        {
            _captureHttpHandler = captureHttpHandler;
        }

        protected override HttpClient CreateClient()
        {
            var client = new HttpClient(_captureHttpHandler) { BaseAddress = new Uri(Configuration.BaseUrl) };
            return client;
        }
    }
}