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
        public JObject RequestBody { get; private set; }

        public CaptureHttpHandler(string response = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _response = response;
            _statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestBody = JObject.Parse(await request.Content.ReadAsStringAsync());
            var httpResponseMessage = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_response)
            };
            return httpResponseMessage;
        }
    }
}