using System.Collections.Generic;
using System.Threading.Tasks;
using ConnectSdk.Config;
using ConnectSdk.Querying;

namespace ConnectSdk
{
    public static class Connect
    {
        private const string UnitializedMessage = "Before Pushing or Querying with Connect it must be initialized. " +
                                                  "The simpilist way initialize it is by " +
                                                  "Calling Connect.Initialize(new BasicConfiguration(\"YOUR_API_KEY\", \"YOUR_PROJECT_ID\"))." +
                                                  "Api Keys and Project Ids can be retreived from https://app.getconnect.io";
        private static IConnect _connectClient;

        private static IConnect ConnectClient
        {
            get
            {
                if (_connectClient == null)
                    throw new ConnectInitializationException(UnitializedMessage);

                return _connectClient;
            }
        }

        public static void Initialize(IConfiguration configuration)
        {
            _connectClient = new ConnectClient(configuration);
        }

        public static void Initialize(IConnect connect)
        {
            _connectClient = connect;
        }

        public static Task<EventPushResponse> Push(string collectionName, object eventData)
        {
            return ConnectClient.Push(collectionName, eventData);
        }

        public static Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<object> eventData)
        {
            return ConnectClient.Push(collectionName, eventData);
        }

        public static Task Add(string collectionName, object eventData)
        {
           return ConnectClient.Add(collectionName, eventData);
        }

        public static Task Add(string collectionName, IEnumerable<object> eventData)
        {
            return ConnectClient.Add(collectionName, eventData);
        }

        public static Task<EventBatchPushResponse> PushPending()
        {
            return ConnectClient.PushPending();
        }

        public static IQuery<TResult> Query<TResult>(string collectionName)
        {
            return ConnectClient.Query<TResult>(collectionName);
        }

        public static IQuery<IDictionary<string, object>> Query(string collectionName)
        {
            return ConnectClient.Query(collectionName);
        }
    }
}
