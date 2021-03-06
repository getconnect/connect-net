﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectSdk.Api;
using ConnectSdk.Config;
using ConnectSdk.Querying;
using ConnectSdk.Store;
using Newtonsoft.Json;

namespace ConnectSdk
{
    public class ConnectClient : IConnect
    {
        protected readonly IEventStore EventStore;
        protected readonly IEventEndpoint HttpEventEndpoint;
        protected readonly JsonSerializer Serializer;

        public ConnectClient(IConfiguration configuration, IEventStore eventStore = null)
            : this(configuration, new HttpEventEndpoint(configuration), eventStore)
        {
        }

        public ConnectClient(IConfiguration configuration, IEventEndpoint eventEndpoint, IEventStore eventStore = null)
        {
            EventStore = eventStore ?? new FileEventStore(configuration.ProjectId);
            HttpEventEndpoint = eventEndpoint;
            Serializer = configuration.EventSerializer;
        }

        public virtual async Task<EventPushResponse> Push(string collectionName, object eventData)
        {
            return await HttpEventEndpoint.Push(collectionName, new Event(eventData, Serializer));
        }

        public virtual async Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<object> eventData)
        {
            return await HttpEventEndpoint.Push(collectionName, eventData.Select(ed => new Event(ed, Serializer)));
        }

        public virtual async Task Add(string collectionName, object eventData)
        {
            await EventStore.Add(collectionName, new Event(eventData, Serializer));
        }

        public virtual async Task Add(string collectionName, IEnumerable<object> eventData)
        {
            await TaskEx.WhenAll(eventData.Select(ed => EventStore.Add(collectionName, new Event(ed, Serializer))));
        }

        public virtual async Task<EventBatchPushResponse> PushPending()
        {
            var events = await EventStore.ReadAll();
            var results = await HttpEventEndpoint.Push(events);

            foreach (var keyedResponses in results.ResponsesByCollection)
            {
                foreach (var eventPushResponse in keyedResponses.Value)
                {
                    var status = eventPushResponse.Status;
                    if (status == EventPushResponseStatus.Successful || status == EventPushResponseStatus.Duplicate)
                        await EventStore.Acknowlege(keyedResponses.Key, eventPushResponse.Event);
                }
            }

            return results;
        }

        public virtual IQuery<TResult> Query<TResult>(string collectionName)
        {
            return new Query<TResult>(collectionName, HttpEventEndpoint);
        }

        public virtual IQuery<IDictionary<string, object>> Query(string collectionName)
        {
            return Query<IDictionary<string, object>>(collectionName);
        }
    }
}
