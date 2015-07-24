using System;

namespace ConnectSdk.Querying
{
    public class AbsoluteTimeframe : ITimeframe
    {
        public DateTime? Start { get; }
        public DateTime? End { get; }

        public AbsoluteTimeframe(DateTime? start = null, DateTime? end = null)
        {
            Start = start;
            End = end;
        }
    }
}