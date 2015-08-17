namespace ConnectSdk.Querying
{
    public enum QueryResponseStatus
    {
        Successful,
        TooManyResults,
        Unauthorized,
        QueryFormatError,
        NetworkError,
        GeneralError
    }
}