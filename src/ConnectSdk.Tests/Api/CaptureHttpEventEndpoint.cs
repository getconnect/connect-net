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

        protected override HttpClient ConfigureClient()
        {
            var client = new HttpClient(_captureHttpHandler) { BaseAddress = new Uri(Configuration.BaseUrl) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Api-Key", Configuration.WriteKey);
            return client;
        }
    }
}