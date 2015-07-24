using System.Net;

namespace ConnectSdk.Api
{
    public static class StatusMapper
    {
        public static ResponseStatus MapStatusCode(HttpStatusCode statusCode)
        {
            if (statusCode >= HttpStatusCode.OK && statusCode <= (HttpStatusCode) 299)
            {
                return ResponseStatus.Successfull;
            }
            else if (statusCode == HttpStatusCode.Conflict)
            {
                return ResponseStatus.Duplicate;
            }
            else if (statusCode == (HttpStatusCode)422)
            {
                return ResponseStatus.EventFormatError;
            }
            else if (statusCode == HttpStatusCode.NotAcceptable)
            {
                return ResponseStatus.EventFormatError;
            }
            else if (statusCode == HttpStatusCode.BadGateway || statusCode == HttpStatusCode.GatewayTimeout)
            {
                return ResponseStatus.NetworkError;
            }
                
            return ResponseStatus.GeneralError;
        }
    }
}