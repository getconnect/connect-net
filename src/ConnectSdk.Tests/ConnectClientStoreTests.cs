using System;
using System.Net;
using System.Threading.Tasks;
using ConnectSdk.Config;
using ConnectSdk.Store;
using ConnectSdk.Tests.Api;
using PCLStorage;
using Xunit;

namespace ConnectSdk.Tests
{
    public class ConnectClientStoreTests
    {
        public class WhenAddingSingleEvent : IDisposable
        {            
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolder;
            private const string Collection = "WhenAddingSingleEvent";

            public WhenAddingSingleEvent()
            {
                _rootFolder = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler();
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore, new FileEventStore(_rootFolder));
            }

            [Fact]
            public async Task It_should_add_event_file()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new { id });

                var result = await rootFolder.CheckExistsAsync(string.Format("{0}/{1}/{2}.json", _rootFolder, Collection, id));
                Assert.Equal(ExistenceCheckResult.FileExists, result);
                
            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolder);
                await rootFolder.DeleteAsync();
            }
        }

        public class WhenAddingBatchEvent : IDisposable
        {            
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolder;
            private const string Collection = "WhenAddingBatchEvent";

            public WhenAddingBatchEvent()
            {
                _rootFolder = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler();
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore, new FileEventStore(_rootFolder));
            }

            [Fact]
            public async Task It_should_add_event_file()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });

                var result = await rootFolder.CheckExistsAsync(string.Format("{0}/{1}/{2}.json", _rootFolder, Collection, id));
                Assert.Equal(ExistenceCheckResult.FileExists, result);
                
            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolder);
                await rootFolder.DeleteAsync();
            }
        }

        public class WhenPushingPendingEvents : IDisposable
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolder;
            private const string Collection = "WhenPushingStoredEvents";

            public WhenPushingPendingEvents()
            {
                _rootFolder = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler(@"{""WhenPushingStoredEvents"": [ {""success"": true }]}");
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore, new FileEventStore(_rootFolder));
            }

            [Fact]
            public async Task It_should_have_success_http_status()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);

            }

            [Fact]
            public async Task It_should_have_success_batch_status()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);

            }

            [Fact]
            public async Task It_should_have_duplicate_event_status()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.ResponsesByCollection[Collection][0].Status);

            }

            [Fact]
            public async Task The_events_should_be_cleared()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];
                var existsResult = await rootFolder.CheckExistsAsync(string.Format("{0}/{1}/{2}.json", rootFolder, Collection, id));

                Assert.Equal(ExistenceCheckResult.NotFound, existsResult);

            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolder);
                await rootFolder.DeleteAsync();
            }
        }

        public class WhenPushingPendingEventsWithDuplicates : IDisposable
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolder;
            private const string Collection = "WhenPushingPendingEventsWithDuplicates";

            public WhenPushingPendingEventsWithDuplicates()
            {
                _rootFolder = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler(@"{""WhenPushingPendingEventsWithDuplicates"": [ {""success"": false, ""duplicate"": true }]}");
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore, new FileEventStore(_rootFolder));
            }

            [Fact]
            public async Task It_should_have_success_http_status()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);

            }

            [Fact]
            public async Task It_should_have_success_batch_status()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);

            }

            [Fact]
            public async Task It_should_have_success_event_status()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Duplicate, result.ResponsesByCollection[Collection][0].Status);

            }

            [Fact]
            public async Task The_events_should_be_cleared()
            {
                var id = Guid.NewGuid();
                var rootFolder = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];
                var existsResult = await rootFolder.CheckExistsAsync(string.Format("{0}/{1}/{2}.json", _rootFolder, Collection, id));

                Assert.Equal(ExistenceCheckResult.NotFound, existsResult);

            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolder);
                await rootFolder.DeleteAsync();
            }
        }
    }
}