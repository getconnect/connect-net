using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConnectSdk.Api
{
    public static class ResponseParser
    {
        private class ServerBatchResponse
        {
            public bool success { get; set; }
            public bool duplicate { get; set; }
            public string message { get; set; }
        }

        public static EventPushResponse ParseEventResponse(HttpStatusCode statusCode, string responseText, Event eventData)
        {
            var jsonText = string.IsNullOrEmpty(responseText) ? "{}" : responseText;
            var responseJson = JObject.Parse(jsonText);
            var errorMessage = responseJson["errorMessage"] != null ? responseJson["errorMessage"].ToString() : null;
            var errorsJson = responseJson["errors"];
            var errors = errorsJson != null ? errorsJson.ToDictionary(error => error["field"].ToString(), error => error["description"].ToString()) : null;
            return new EventPushResponse(statusCode, eventData, errorMessage, errors);
        }

        public static EventBatchPushResponse ParseEventResponseBatch(HttpStatusCode statusCode, string responseText, IDictionary<string, IEnumerable<Event>> eventDataByCollection)
        {
            if (!IsSuccessStatusCode(statusCode))
            {
                var responseJson = JObject.Parse(responseText);
                var errorMessage = responseJson["errorMessage"] != null ? responseJson["errorMessage"].ToString() : string.Empty;
                return new EventBatchPushResponse(statusCode, errorMessage);
            }

            var successResponse = new EventBatchPushResponse(statusCode);
            var batchedResponsesByCollectionName = JsonConvert.DeserializeObject<IDictionary<string, ServerBatchResponse[]>>(responseText);
            foreach (var keyedBatchResponse in batchedResponsesByCollectionName)
            {
                var eventPushResponses = new List<EventPushResponse>();
                var collectionName = keyedBatchResponse.Key;
                var events = eventDataByCollection[collectionName];
                successResponse.ResponsesByCollection.Add(collectionName, eventPushResponses);
                for (int i = 0; i < keyedBatchResponse.Value.Length; i++)
                {
                    var eventResponse = keyedBatchResponse.Value[i];
                    var originalEvent = events.ElementAt(i);
                    var status = EventPushResponseStatus.Successfull;
                    if (eventResponse.duplicate)
                        status = EventPushResponseStatus.Duplicate;
                    else if (!eventResponse.success)
                        status = EventPushResponseStatus.GeneralError;

                    var eventBatchResponse = new EventPushResponse(status, originalEvent, eventResponse.message);
                    eventPushResponses.Add(eventBatchResponse);
                }
            }
            return successResponse;
        }

        private static bool IsSuccessStatusCode(HttpStatusCode statusCode)
        {
            return StatusMapper.MapPushStatusCode(statusCode) == EventPushResponseStatus.Successfull;
        }
    }
}