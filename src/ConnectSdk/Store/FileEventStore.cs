using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;
using PCLStorage;

namespace ConnectSdk.Store
{
    public class FileEventStore : IEventStore
    {
        private readonly string _projectId;
        private readonly string _rootFolderName;
        private static readonly AsyncLock Mutex = new AsyncLock();

        public FileEventStore(string projectId, string rootFolderName = ".tp-events")
        {
            _projectId = projectId;
            _rootFolderName = rootFolderName;
        }

        public async Task Add(string collectionName, Event @event)
        {
            using (await Mutex.LockAsync())
            {
                var fileName = GenerateEventFileName(@event);
                var rootFolder = await GetRootFolder();
                var folder = await rootFolder.CreateFolderAsync(collectionName, CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                await file.WriteAllTextAsync(@event.Data.ToString());
            }
        }

        public async Task<IEnumerable<Event>> Read(string collectionName)
        {
            using (await Mutex.LockAsync())
            {
                var rootFolder = await GetRootFolder();
                var folder = await rootFolder.CreateFolderAsync(collectionName, CreationCollisionOption.OpenIfExists);
                var collectionFolder = await folder.GetFolderAsync(collectionName);
                return await GetEvents(collectionFolder);
            }
        }

        public async Task<IDictionary<string, IEnumerable<Event>>> ReadAll()
        {
            using (await Mutex.LockAsync())
            {
                var rootFolder = await GetRootFolder();
                var collectionFolders = await rootFolder.GetFoldersAsync();
                var eventsByCollection = await TaskEx.WhenAll(collectionFolders.Select(async collectionFolder => new
                {
                    ColleactionName = collectionFolder.Name,
                    Events = await GetEvents(collectionFolder)
                }));
                return eventsByCollection.ToDictionary(ebc => ebc.ColleactionName, ebc => ebc.Events);
            }
        }

        public async Task Acknowlege(string collectionName, Event @event)
        {
            using (await Mutex.LockAsync())
            {
                var fileName = GenerateEventFileName(@event);
                var rootFolder = await GetRootFolder();
                var folder = await rootFolder.GetFolderAsync(collectionName);
                var existsResults = await folder.CheckExistsAsync(fileName);

                if (existsResults == ExistenceCheckResult.FileExists)
                {
                    var file = await folder.GetFileAsync(fileName);
                    await file.DeleteAsync();
                }
            }
        }

        private async Task<IFolder> GetRootFolder()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var connectFolder = await rootFolder.CreateFolderAsync(_rootFolderName, CreationCollisionOption.OpenIfExists);
            return await connectFolder.CreateFolderAsync(_projectId, CreationCollisionOption.OpenIfExists);
        }

        private async Task<IEnumerable<Event>> GetEvents(IFolder collectionFolder)
        {
            var files = await collectionFolder.GetFilesAsync();
            var fileContents = await TaskEx.WhenAll(files.Select(file => file.ReadAllTextAsync()));
            return fileContents.Select(fc => new Event(fc));
        }

        private string GenerateEventFileName(Event @event)
        {
            return string.Format("{0}.json", @event.Id);
        }
    }
}