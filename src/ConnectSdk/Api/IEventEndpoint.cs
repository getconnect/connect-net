﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectSdk.Querying;

namespace ConnectSdk.Api
{
    public interface IEventEndpoint
    {
        Task<EventPushResponse> Push(string collectionName, Event eventData);
        Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<Event> eventData);
        Task<EventBatchPushResponse> Push(IDictionary<string, IEnumerable<Event>> eventDataByCollection);
        Task<QueryResponse<TResult>> Query<TResult>(string collectionName, IQuery<TResult> query);
    }
}