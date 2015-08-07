using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using Nito.AsyncEx;

namespace ConnectSdk.Tests
{
    public class ConnectTests
    {
        private static readonly AsyncLock Mutex = new AsyncLock();

        public class WhenCallingAnUnitializedConnect
        {
            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_push()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize((IConnect)null);
                    await Assert.ThrowsAsync<ConnectInitializationException>(() => Connect.Push("Test", new {}));
                }
            }

            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_add()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize((IConnect)null);
                    await Assert.ThrowsAsync<ConnectInitializationException>(() => Connect.Add("Test", new { }));
                }
            }

            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_push_batch()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize((IConnect)null);
                    await Assert.ThrowsAsync<ConnectInitializationException>(() => Connect.Push("Test", new string[0]));
                }
            }

            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_add_batch()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize((IConnect)null);
                    await Assert.ThrowsAsync<ConnectInitializationException>(() => Connect.Add("Test", new string[0]));
                }
            }

            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_push_pending()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize((IConnect)null);
                    await Assert.ThrowsAsync<ConnectInitializationException>(Connect.PushPending);
                }
            }

            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_query()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize((IConnect)null);
                    Assert.Throws<ConnectInitializationException>(() => Connect.Query("Test"));
                }
            }
        }

        public class WhenCallingAnInitializedConnect
        {
            private readonly IConnect _connect;
            private readonly string _collectionName;
            private readonly object _event;
            private readonly string[] _eventBatch;

            public WhenCallingAnInitializedConnect()
            {
                _connect = Substitute.For<IConnect>();
                _collectionName = "Test";
                _event = new { };
                _eventBatch = new string[0];
            }

            [Fact]
            public async Task It_should_call_push_on_client()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize(_connect);
                    await Connect.Push(_collectionName, _event);
                    _connect.Received().Push(_collectionName, _event);
                }
            }

            [Fact]
            public async Task It_should_call_add_on_client()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize(_connect);
                    await Connect.Add(_collectionName, _event);
                    _connect.Received().Add(_collectionName, _event);
                }
            }

            [Fact]
            public async Task It_should_call_push_batch_on_client()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize(_connect);
                    await Connect.Push(_collectionName, _eventBatch);
                    _connect.Received().Push(_collectionName, _eventBatch);
                }
            }

            [Fact]
            public async Task It_should_call_add_batch_on_client()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize(_connect);
                    await Connect.Add(_collectionName, _eventBatch);
                    _connect.Received().Add(_collectionName, _eventBatch);
                }
            }

            [Fact]
            public async Task It_should_call_push_pending_on_client()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize(_connect);
                    await Connect.PushPending();
                    _connect.Received().PushPending();
                }
            }

            [Fact]
            public async Task It_should_throw_an_initialization_exception_for_query()
            {
                using (await Mutex.LockAsync())
                {
                    Connect.Initialize(_connect);
                    Connect.Query(_collectionName);
                    _connect.Received().Query(_collectionName);
                }
            }
        }
    }
}