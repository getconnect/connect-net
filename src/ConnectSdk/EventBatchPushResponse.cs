using System.Collections.Generic;
using System.Net;
using ConnectSdk.Api;

namespace ConnectSdk
{
    public class EventBatchPushResponse
    {
        public EventBatchPushResponse(HttpStatusCode httpStatusCode, string errorErrorMessage = null)
        {
            ResponsesByCollection = new Dictionary<string, IList<EventPushResponse>>();
            Status = MapStatus(httpStatusCode);
            ErrorMessage = errorErrorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public IDictionary<string, IList<EventPushResponse>> ResponsesByCollection { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }
        public EventPushResponseStatus Status { get; private set; }
        public string ErrorMessage { get; private set; }

        private EventPushResponseStatus MapStatus(HttpStatusCode httpStatusCode)
        {
            return StatusMapper.MapStatusCode(httpStatusCode);
        }
    }
}