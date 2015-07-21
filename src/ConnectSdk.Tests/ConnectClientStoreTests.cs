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
            private string _rootFolderName;
            private const string Collection = "WhenAddingSingleEvent";

            public WhenAddingSingleEvent()
            {
                _rootFolderName = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler();
                _connect = TestConfigurator.GetTestableClient(_testHandler, _rootFolderName);
            }

            [Fact]
            public async Task It_should_add_event_file()
            {
                var id = Guid.NewGuid();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new { id });

                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var result = await localStorageRoot.CheckExistsAsync(filePathForEvent);
                Assert.Equal(ExistenceCheckResult.FileExists, result);
                
            }

            [Fact]
            public async Task It_should_serialize_id_to_file()
            {
                var id = Guid.NewGuid();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new { id });

                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var file = await localStorageRoot.GetFileAsync(filePathForEvent);
                var contents = await file.ReadAllTextAsync();
                Assert.Contains(string.Format("\"id\": \"{0}\"", id), contents);

            }

            [Fact]
            public async Task It_should_serialize_local_dates_in_iso_utc_format()
            {
                var id = Guid.NewGuid();
                var utcDateTime = new DateTime(2015, 07, 17, 11, 11, 11, DateTimeKind.Utc);
                var localTime = utcDateTime.ToLocalTime();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new { id, SomeDate = localTime });

                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var file = await localStorageRoot.GetFileAsync(filePathForEvent);
                var contents = await file.ReadAllTextAsync();

                Assert.Contains("\"SomeDate\": \"2015-07-17T11:11:11Z\"", contents);
            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolderName);
                await rootFolder.DeleteAsync();
            }
        }

        public class WhenAddingBatchEvent : IDisposable
        {            
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolderName;
            private const string Collection = "WhenAddingBatchEvent";

            public WhenAddingBatchEvent()
            {
                _rootFolderName = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler();
                _connect = TestConfigurator.GetTestableClient(_testHandler, _rootFolderName);
            }

            [Fact]
            public async Task It_should_add_event_file()
            {
                var id = Guid.NewGuid();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });

                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var result = await localStorageRoot.CheckExistsAsync(filePathForEvent);
                Assert.Equal(ExistenceCheckResult.FileExists, result);

            }

            [Fact]
            public async Task It_should_serialize_id_to_file()
            {
                var id = Guid.NewGuid();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });

                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var file = await localStorageRoot.GetFileAsync(filePathForEvent);
                var contents = await file.ReadAllTextAsync();
                Assert.Contains(string.Format("\"id\": \"{0}\"", id), contents);

            }

            [Fact]
            public async Task It_should_serialize_local_dates_in_iso_utc_format()
            {
                var id = Guid.NewGuid();
                var utcDateTime = new DateTime(2015, 07, 17, 11, 11, 11, DateTimeKind.Utc);
                var localTime = utcDateTime.ToLocalTime();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id, SomeDate = localTime } });

                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var file = await localStorageRoot.GetFileAsync(filePathForEvent);
                var contents = await file.ReadAllTextAsync();

                Assert.Contains("\"SomeDate\": \"2015-07-17T11:11:11Z\"", contents);
            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolderName);
                await rootFolder.DeleteAsync();
            }
        }

        public class WhenPushingPendingEvents : IDisposable
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolderName;
            private const string Collection = "WhenPushingStoredEvents";

            public WhenPushingPendingEvents()
            {
                _rootFolderName = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler(@"{""WhenPushingStoredEvents"": [ {""success"": true }]}");
                _connect = TestConfigurator.GetTestableClient(_testHandler, _rootFolderName);
            }

            [Fact]
            public async Task It_should_have_success_http_status()
            {
                var id = Guid.NewGuid();

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);

            }

            [Fact]
            public async Task It_should_have_success_batch_status()
            {
                var id = Guid.NewGuid();

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);

            }

            [Fact]
            public async Task It_should_have_duplicate_event_status()
            {
                var id = Guid.NewGuid();

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.ResponsesByCollection[Collection][0].Status);

            }

            [Fact]
            public async Task The_events_should_be_cleared()
            {
                var id = Guid.NewGuid();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];
                var existsResult = await localStorageRoot.CheckExistsAsync(TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id));

                Assert.Equal(ExistenceCheckResult.NotFound, existsResult);

            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolderName);
                await rootFolder.DeleteAsync();
            }
        }

        public class WhenPushingPendingEventsWithDuplicates : IDisposable
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _rootFolderName;
            private const string Collection = "WhenPushingPendingEventsWithDuplicates";

            public WhenPushingPendingEventsWithDuplicates()
            {
                _rootFolderName = Guid.NewGuid().ToString();
                _testHandler = new CaptureHttpHandler(@"{""WhenPushingPendingEventsWithDuplicates"": [ {""success"": false, ""duplicate"": true }]}");
                _connect = TestConfigurator.GetTestableClient(_testHandler, _rootFolderName);
            }

            [Fact]
            public async Task It_should_have_success_http_status()
            {
                var id = Guid.NewGuid();

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);

            }

            [Fact]
            public async Task It_should_have_success_batch_status()
            {
                var id = Guid.NewGuid();

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);

            }

            [Fact]
            public async Task It_should_have_success_event_status()
            {
                var id = Guid.NewGuid();

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];

                Assert.Equal(EventPushResponseStatus.Duplicate, result.ResponsesByCollection[Collection][0].Status);

            }

            [Fact]
            public async Task The_events_should_be_cleared()
            {
                var id = Guid.NewGuid();
                var localStorageRoot = FileSystem.Current.LocalStorage;

                await _connect.Add(Collection, new[] { new { id } });
                var result = (await TaskEx.WhenAll(_connect.PushPending(), _connect.PushPending()))[0];
                var filePathForEvent = TestConfigurator.GetFilePathForEvent(_rootFolderName, Collection, id);
                var existsResult = await localStorageRoot.CheckExistsAsync(filePathForEvent);

                Assert.Equal(ExistenceCheckResult.NotFound, existsResult);

            }

            public async void Dispose()
            {
                var rootFolder = await FileSystem.Current.LocalStorage.GetFolderAsync(_rootFolderName);
                await rootFolder.DeleteAsync();
            }
        }
    }
}