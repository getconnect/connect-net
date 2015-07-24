using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectSdk.Config;
using ConnectSdk.Querying;

namespace ConnectSdk
{
    public static class Connect
    {
        private static IConnect _connect;

        public static void Initialize(IConfiguration configuration)
        {
            _connect = new ConnectClient(configuration);
        }

        public static void Initialize(IConnect connect)
        {
            _connect = connect;
        }

        public static Task<EventPushResponse> Push(string collectionName, object eventData)
        {
            return _connect.Push(collectionName, eventData);
        }

        public static Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<object> eventData)
        {
            return _connect.Push(collectionName, eventData);
        }

        public static Task Add(string collectionName, object eventData)
        {
           return _connect.Add(collectionName, eventData);
        }

        public static Task Add(string collectionName, IEnumerable<object> eventData)
        {
            return _connect.Add(collectionName, eventData);
        }

        public static Task<EventBatchPushResponse> PushPending()
        {
            return _connect.PushPending();
        }

        public static IQuery<TResult> Query<TResult>(string collectionName)
        {
            return _connect.Query<TResult>(collectionName);
        }

        public static IQuery<Dictionary<string, object>> Query(string collectionName)
        {
            return _connect.Query(collectionName);
        }
    }
}
