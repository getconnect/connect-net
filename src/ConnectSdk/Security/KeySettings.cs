namespace ConnectSdk.Security
{
    public class KeySettings
    {
        public KeySettings(bool canPush, bool canQuery)
        {
            CanPush = canPush;
            CanQuery = canQuery;
        }

        public bool CanPush { get; }
        public bool CanQuery { get; }
    }
}