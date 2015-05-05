using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ConnectSdk.Config;
using ConnectSdk.Tests.Api;
using Xunit;

namespace ConnectSdk.Tests
{
    public class ConnectClientPushTests
    {
        public class WhenPushingSingleSuccessfullEvent
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingSingleSuccessfullEvent()
            {
                _testHandler = new CaptureHttpHandler();
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_serialize_event()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal("World", _testHandler.RequestBody["Hello"]);
            }

            [Fact]
            public async Task It_should_assign_an_id_to_event()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.NotNull(_testHandler.RequestBody["id"]);
            }

            [Fact]
            public async Task It_should_assign_a_timestamp_to_event()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.NotNull(_testHandler.RequestBody["timestamp"]);
            }

            [Fact]
            public async Task It_should_have_successfull_http_status()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_have_successfull_event_status()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);
            }
            
        }

        public class WhenPushingSingleDuplicateEvent
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingSingleDuplicateEvent()
            {
                var responseText = @"{""errorMessage"": ""Error""}";
                _testHandler = new CaptureHttpHandler(responseText, HttpStatusCode.Conflict);
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_have_conflict_http_status()
            {
                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal(HttpStatusCode.Conflict, result.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_have_duplicate_event_status()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal(EventPushResponseStatus.Duplicate, result.Status);
            }

            [Fact]
            public async Task It_should_have_error_message()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal("Error", result.ErrorMessage);
            }

        }
        
        public class WhenPushingSingleUnprocessableEvent
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingSingleUnprocessableEvent()
            {
                var responseText = @"{""errors"": [ { ""field"":""Hello"", ""description"":""Invalid Greeting"" } ]}";
                _testHandler = new CaptureHttpHandler(responseText, (HttpStatusCode)422);
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_have_unprocessable_http_status()
            {
                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal((HttpStatusCode)422, result.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_have_duplicate_event_status()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal(EventPushResponseStatus.EventFormatError, result.Status);
            }

            [Fact]
            public async Task It_should_have_field_error()
            {

                var result = await _connect.Push("test", new { Hello = "World" });

                Assert.Equal("Invalid Greeting", result.FieldErrors["Hello"]);
            }

        }
        
        public class WhenPushingAnEventWithReservedProperties
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingAnEventWithReservedProperties()
            {
                _testHandler = new CaptureHttpHandler();
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_throw_validation_exception()
            {
                EventDataValidationException e = await Assert.ThrowsAsync<EventDataValidationException>(() => _connect.Push("test", new { TP_Hello = "World" }));
                Assert.Equal("TP_Hello", e.InvalidPropertyNames.ElementAt(0));
            }
        }

        public class WhenPushingSuccessfullBatchEvent
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingSuccessfullBatchEvent()
            {
                _testHandler = new CaptureHttpHandler(@"{""test"": [ {""success"": true }]}");
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_serialize_event()
            {
                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal("World", firstTestResponse["Hello"]);
            }

            [Fact]
            public async Task It_should_assign_an_id_to_event()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.NotNull(firstTestResponse["id"]);
            }

            [Fact]
            public async Task It_should_assign_a_timestamp_to_event()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.NotNull(firstTestResponse["id"]);
            }

            [Fact]
            public async Task It_should_have_successfull_http_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_have_successfull_event_batch_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);
            }

            [Fact]
            public async Task It_should_have_successfull_event_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.ResponsesByCollection["test"][0].Status);
            }
        }

        public class WhenPushingBatchEventWithDuplicates
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingBatchEventWithDuplicates()
            {
                _testHandler = new CaptureHttpHandler(@"{""test"": [ {""success"": false, ""duplicate"": true, ""message"": ""Error"" }]}");
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_have_successfull_http_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_have_successfull_event_batch_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);
            }

            [Fact]
            public async Task It_should_have_duplicate_event_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(EventPushResponseStatus.Duplicate, result.ResponsesByCollection["test"][0].Status);
            }

            [Fact]
            public async Task It_should_have_event_error_message()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal("Error", result.ResponsesByCollection["test"][0].ErrorMessage);
            }
        }

        public class WhenPushingBatchEventWithError
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;

            public WhenPushingBatchEventWithError()
            {
                _testHandler = new CaptureHttpHandler(@"{""test"": [ {""success"": false, ""message"": ""Error"" }]}");
                var captureHttpEventStore = new CaptureHttpEventEndpoint(new BasicConfiguration(string.Empty), _testHandler);
                _connect = new ConnectClient(captureHttpEventStore);
            }

            [Fact]
            public async Task It_should_have_successfull_http_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_have_successfull_event_batch_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(EventPushResponseStatus.Successfull, result.Status);
            }

            [Fact]
            public async Task It_should_have_duplicate_event_status()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal(EventPushResponseStatus.GeneralError, result.ResponsesByCollection["test"][0].Status);
            }

            [Fact]
            public async Task It_should_have_event_error_message()
            {

                var result = await _connect.Push("test", new[] { new { Hello = "World" } });

                var firstTestResponse = _testHandler.RequestBody["test"][0];

                Assert.Equal("Error", result.ResponsesByCollection["test"][0].ErrorMessage);
            }
        }
    }
}