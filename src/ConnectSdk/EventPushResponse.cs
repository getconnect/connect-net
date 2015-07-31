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

        public Event Event { get; }
        public EventPushResponseStatus Status { get; }
        public HttpStatusCode? HttpStatusCode { get; }
        public string ErrorMessage { get; }
        public IDictionary<string, string> FieldErrors { get; }

        private EventPushResponseStatus MapStatus(HttpStatusCode httpStatusCode)
        {
            return StatusMapper.MapPushStatusCode(httpStatusCode);
        }
    }
}