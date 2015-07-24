using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConnectSdk.Config;
using ConnectSdk.Querying;
using Newtonsoft.Json;

namespace ConnectSdk.Api
{
    public class HttpEventEndpoint : IEventEndpoint
    {
        protected readonly IConfiguration Configuration;

        public const string ContentType = "application/json";
        public const string ApiKeyHeaderKey = "X-Api-Key";
        public const string ProjectIdHeaderKey = "X-Project-Id";


        public HttpEventEndpoint(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async virtual Task<EventPushResponse> Push(string collectionName, Event eventData)
        {
            using (var client = ConfigureClient())
            {
                var eventBody = RequestBodyGenerator.GenerateEventBody(eventData);
                var response = await client.PostAsync("events/" + collectionName, new StringContent(eventBody, Encoding.UTF8, ContentType))
                    .ConfigureAwait(false);
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
                var response = await client.PostAsync("events", new StringContent(batch, Encoding.UTF8, ContentType))
                    .ConfigureAwait(false);
                var responseText = await response.Content.ReadAsStringAsync();

                var eventBatchPushResponse = ResponseParser.ParseEventResponseBatch(response.StatusCode, responseText, eventDataByCollection);
                return eventBatchPushResponse;
            }
        }

        public async Task<QueryResponse<TResult>> Query<TResult>(string collectionName, IQuery<TResult> query)
        {
            using (var client = ConfigureClient())
            {
                var response = await client.GetAsync($"events/{collectionName}?query={query}")
                    .ConfigureAwait(false);
                var responseText = await response.Content.ReadAsStringAsync();
                var queryResponse = JsonConvert.DeserializeObject<QueryResponse<TResult>>(responseText);
                queryResponse.Status = StatusMapper.MapStatusCode(response.StatusCode);
                queryResponse.HttpStatusCode = response.StatusCode;
                return queryResponse;
            }
        }

        protected virtual HttpClient ConfigureClient()
        {
            var client = CreateClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType));
            client.DefaultRequestHeaders.Add(ApiKeyHeaderKey, Configuration.WriteKey);
            client.DefaultRequestHeaders.Add(ProjectIdHeaderKey, Configuration.ProjectId);
            return client;
        }

        protected virtual HttpClient CreateClient()
        {
            return new HttpClient {BaseAddress = new Uri(Configuration.BaseUrl)};
        }
    }
}