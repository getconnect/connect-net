using System;
using System.Collections.Generic;

namespace ConnectSdk.Querying
{
    public class QueryIntervalResult<TResult>
    {
        public IntervalPeriodResult Interval { get; }

        public DateTime Start => Interval.Start;
        public DateTime End => Interval.End;
        public IEnumerable<TResult> Results { get; }

        public QueryIntervalResult(IntervalPeriodResult interval, IEnumerable<TResult> results = null)
        {
            Interval = interval;
            Results = results;
        }
    }
}