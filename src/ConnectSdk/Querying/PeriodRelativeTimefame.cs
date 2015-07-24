namespace ConnectSdk.Querying
{
    public class PeriodRelativeTimefame : ITimeframe
    {
        public string Period { get; }
        public TimeType TimeType { get; }
        public int Value { get; }

        public PeriodRelativeTimefame(string period, TimeType timeType, int value)
        {
            Period = period;
            TimeType = timeType;
            Value = value;
        }
    }
}