using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectSdk.Config;

namespace ConnectSdk
{
    public static class Connect
    {
        private static IConnect _connect;

        public static void Inititialize(IConfiguration configuration)
        {
            _connect = new ConnectClient(configuration);
        }

        public static void Inititialize(IConnect connect)
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

        public static Task<EventBatchPushResponse> PushStored()
        {
            return _connect.PushPending();
        }
    }
}
