using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectSdk
{
    public interface IConnect
    {
        Task<EventPushResponse> Push(string collectionName, object eventData);

        Task<EventBatchPushResponse> Push(string collectionName, IEnumerable<object> eventData);

        Task Add(string collectionName, object eventData);

        Task Add(string collectionName, IEnumerable<object> eventData);

        Task<EventBatchPushResponse> PushPending();
    }
}