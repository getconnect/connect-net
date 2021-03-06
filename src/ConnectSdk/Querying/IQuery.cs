﻿using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace ConnectSdk.Querying
{
    public interface IQuery<TResult>
    {
        IDictionary<string, Aggregation> Select { get; }
        IDictionary<string, IEnumerable<Filter>> Filter { get; }
        ITimeframe Timeframe { get; }
        string[] GroupBy { get; }
        object Timezone { get; }
        Interval? Interval { get; }
        IDictionary<string, object> Custom { get; }
        Query<TNewResultType> UpdateWith<TNewResultType>(IDictionary<string, Aggregation> aggregations = null, IDictionary<string, IEnumerable<Filter>> filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null, IDictionary<string, object> custom = null);
        Task<QueryResponse<TResult>> Execute();
    }
}