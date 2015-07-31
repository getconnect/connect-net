using System.Net;
using ConnectSdk.Querying;

namespace ConnectSdk.Api
{
    public static class StatusMapper
    {
        public static EventPushResponseStatus MapPushStatusCode(HttpStatusCode statusCode)
        {
            if (statusCode >= HttpStatusCode.OK && statusCode <= (HttpStatusCode) 299)
            {
                return EventPushResponseStatus.Successfull;
            }
            else if (statusCode == HttpStatusCode.Conflict)
            {
                return EventPushResponseStatus.Duplicate;
            }
            else if (statusCode == (HttpStatusCode)422)
            {
                return EventPushResponseStatus.EventFormatError;
            }
            else if (statusCode == HttpStatusCode.NotAcceptable)
            {
                return EventPushResponseStatus.EventFormatError;
            }
            else if (statusCode == HttpStatusCode.BadGateway || statusCode == HttpStatusCode.GatewayTimeout)
            {
                return EventPushResponseStatus.NetworkError;
            }
                
            return EventPushResponseStatus.GeneralError;
        }

        public static QueryResponseStatus MapQueryStatusCode(HttpStatusCode statusCode)
        {
            if (statusCode >= HttpStatusCode.OK && statusCode <= (HttpStatusCode) 299)
            {
                return QueryResponseStatus.Successfull;
            }
            else if (statusCode == (HttpStatusCode)422)
            {
                return QueryResponseStatus.QueryFormatError;
            }
            else if (statusCode == HttpStatusCode.NotAcceptable)
            {
                return QueryResponseStatus.QueryFormatError;
            }
            else if (statusCode == HttpStatusCode.BadGateway || statusCode == HttpStatusCode.GatewayTimeout)
            {
                return QueryResponseStatus.NetworkError;
            }
                
            return QueryResponseStatus.GeneralError;
        }
    }
}