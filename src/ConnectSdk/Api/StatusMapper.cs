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
                return EventPushResponseStatus.Successful;
            }
            if (statusCode == HttpStatusCode.Conflict)
            {
                return EventPushResponseStatus.Duplicate;
            }
            if (statusCode == (HttpStatusCode)422)
            {
                return EventPushResponseStatus.EventFormatError;
            }
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return EventPushResponseStatus.Unauthorized;
            }
            if (statusCode == HttpStatusCode.NotAcceptable)
            {
                return EventPushResponseStatus.EventFormatError;
            }
            if (statusCode == HttpStatusCode.BadGateway || statusCode == HttpStatusCode.GatewayTimeout)
            {
                return EventPushResponseStatus.NetworkError;
            }

            return EventPushResponseStatus.GeneralError;
        }

        public static QueryResponseStatus MapQueryStatusCode(HttpStatusCode statusCode)
        {
            if (statusCode >= HttpStatusCode.OK && statusCode <= (HttpStatusCode) 299)
            {
                return QueryResponseStatus.Successful;
            }
            if (statusCode == (HttpStatusCode)422)
            {
                return QueryResponseStatus.QueryFormatError;
            }
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return QueryResponseStatus.Unauthorized;
            }
            if (statusCode == HttpStatusCode.RequestEntityTooLarge)
            {
                return QueryResponseStatus.TooManyResults;
            }
            if (statusCode == HttpStatusCode.NotAcceptable)
            {
                return QueryResponseStatus.QueryFormatError;
            }
            if (statusCode == HttpStatusCode.BadGateway || statusCode == HttpStatusCode.GatewayTimeout)
            {
                return QueryResponseStatus.NetworkError;
            }

            return QueryResponseStatus.GeneralError;
        }
    }
}