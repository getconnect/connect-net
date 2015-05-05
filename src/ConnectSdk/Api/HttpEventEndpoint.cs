using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConnectSdk.Config;

namespace ConnectSdk.Api
{
    public class HttpEventEndpoint : IEventEndpoint
    {
        protected readonly IConfiguration Configuration;

        public HttpEventEndpoint(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async virtual Task<EventPushResponse> Push(string collectionName, Event eventData)
        {
            using (var client = ConfigureClient())
            {
                var eventBody = RequestBodyGenerator.GenerateEventBody(eventData);
                var response = await client.PostAsync("events/" + collectionName, new StringContent(eventBody, Encoding.UTF8, "application/json"))
                    .ConfigureAwait(continueOnCapturedContext: false);
                var responseText = await response.Content.ReadAsStringAsync();
                return ResponseParser.ParseEventResponse(response.StatusCode, responseText, eventData);
            }
        }

        public async virtual Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<Event> eventData)
        {
            return await Push(new Dictionary<string, IEnumerable<Event>> { { collectionName, eventData } });
        }

        public async virtual Task<EventBatchPushResponse> Push(IDictionary<string, IEnumerable<Event>> eventDataByCollection)
        {
            using (var client = ConfigureClient())
            {
                var batch = RequestBodyGenerator.GenerateBatchBody(eventDataByCollection);
                var response = await client.PostAsync("events", new StringContent(batch, Encoding.UTF8, "application/json"))
                    .ConfigureAwait(continueOnCapturedContext: false);
                var responseText = await response.Content.ReadAsStringAsync();

                var eventBatchPushResponse = ResponseParser.ParseEventResponseBatch(response.StatusCode, responseText, eventDataByCollection);
                return eventBatchPushResponse;
            }
        }

        protected virtual HttpClient ConfigureClient()
        {
            var client = new HttpClient {BaseAddress = new Uri(Configuration.BaseUrl)};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-API-Key", Configuration.WriteKey);
            return client;
        }
    }
}