using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectSdk.Querying;

namespace ConnectSdk
{
    public interface IConnect
    {
        Task<EventPushResponse> Push(string collectionName, object eventData);

        Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<object> eventData);

        Task Add(string collectionName, object eventData);

        Task Add(string collectionName, IEnumerable<object> eventData);

        Task<EventBatchPushResponse> PushPending();

        IQuery<TResult> Query<TResult>(string collectionName);

        IQuery<TResult> Query<TResult>(string collectionName, Dictionary<string, Aggregation> aggregations);

        IQuery<IDictionary<string, object>> Query(string collectionName);
    }
}