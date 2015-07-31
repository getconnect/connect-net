using System;
using System.Collections.Generic;

namespace ConnectSdk.Querying
{
    public class QueryIntervalResult<TResult>
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public IEnumerable<TResult> Results { get; }

        public QueryIntervalResult(DateTime start, DateTime end, IEnumerable<TResult> results = null)
        {
            Start = start;
            End = end;
            Results = results;
        }
    }
}