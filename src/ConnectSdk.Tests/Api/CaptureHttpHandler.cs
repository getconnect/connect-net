using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConnectSdk.Tests.Api
{
    public class CaptureHttpHandler : DelegatingHandler
    {
        private readonly string _response;
        private readonly HttpStatusCode _statusCode;
        public JObject ParsedRequestBody { get; private set; }
        public string RequestBody { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }

        public CaptureHttpHandler(string response = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestBody = await request.Content.ReadAsStringAsync();
            Headers = request.Headers.ToDictionary(header => header.Key, header => header.Value.FirstOrDefault());
            ParsedRequestBody = JObject.Parse(RequestBody);
            var httpResponseMessage = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response)
            };
            return httpResponseMessage;
        }
    }
}