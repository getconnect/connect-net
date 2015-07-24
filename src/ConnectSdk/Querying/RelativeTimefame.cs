namespace ConnectSdk.Querying
{
    public class RelativeTimefame : ITimeframe
    {
        public string Period { get; }

        public RelativeTimefame(string period)
        {
            Period = period;
        }
    }
}