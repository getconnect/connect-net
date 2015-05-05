using System.Collections.Generic;
using System.Net;
using ConnectSdk.Api;

namespace ConnectSdk
{
    public class EventPushResponse
    {
        public EventPushResponse(HttpStatusCode httpStatusCode, Event @event, string errorErrorMessage = null, IDictionary<string, string> fieldErrors = null)
        {
            Status = MapStatus(httpStatusCode);
            ErrorMessage = errorErrorMessage;
            HttpStatusCode = httpStatusCode;
            FieldErrors = fieldErrors;
            Event = @event;
        }

        public EventPushResponse(EventPushResponseStatus status, Event @event, string errorErrorMessage = null, IDictionary<string, string> fieldErrors = null)
        {
            Status = status;
            ErrorMessage = errorErrorMessage;
            FieldErrors = fieldErrors;
            Event = @event;
        }

        public Event Event { get; private set; }
        public EventPushResponseStatus Status { get; private set; }
        public HttpStatusCode? HttpStatusCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public IDictionary<string, string> FieldErrors { get; private set; }

        private EventPushResponseStatus MapStatus(HttpStatusCode httpStatusCode)
        {
            return StatusMapper.MapStatusCode(httpStatusCode);
        }
    }
}