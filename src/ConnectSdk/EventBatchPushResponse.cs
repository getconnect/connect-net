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

        public IDictionary<string, IList<EventPushResponse>> ResponsesByCollection { get;  }
        public HttpStatusCode HttpStatusCode { get;  }
        public ResponseStatus Status { get;  }
        public string ErrorMessage { get;  }

        private ResponseStatus MapStatus(HttpStatusCode httpStatusCode)
        {
            return StatusMapper.MapStatusCode(httpStatusCode);
        }
    }
}