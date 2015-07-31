using System;

namespace ConnectSdk.Querying
{
    public class IntervalPeriodResult
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public IntervalPeriodResult(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}