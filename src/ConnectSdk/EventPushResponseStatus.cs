namespace ConnectSdk
{
    public enum EventPushResponseStatus
    {
        Successful,
        Duplicate,
        Unauthorized,
        EventFormatError,
        NetworkError,
        GeneralError
    }
}