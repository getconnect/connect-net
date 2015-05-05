using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectSdk.Store
{
    public interface IEventStore
    {
        Task Add(string collectionName, Event @event);
        Task<IEnumerable<Event>> Read(string collectionName);
        Task<IDictionary<string, IEnumerable<Event>>> ReadAll();
        Task Acknowlege(string collectionName, Event @event);
    }
}