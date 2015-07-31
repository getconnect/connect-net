using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ConnectSdk.Querying;
using ConnectSdk.Tests.Api;
using Xunit;

namespace ConnectSdk.Tests
{
    public class ConnectClientQueryTests
    {
        public class Result
        {
            public string hello { get; set; }
            public decimal total { get; set; }
        }

        public class WhenQueryingCollection
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _collection = "my-coll";

            public WhenQueryingCollection()
            {
                _testHandler = new CaptureHttpHandler("{}");
                _connect = TestConfigurator.GetTestableClient(_testHandler);
            }

            [Fact]
            public async Task It_should_get_using_collection_and_query()
            {
                var results = await _connect.Query(_collection).Execute();
                var uri = _testHandler.Uri;

                Assert.Equal($"https://api.getconnect.io/events/{_collection}?query={{}}", uri.ToString());
            }

            [Fact]
            public async Task It_should_url_encode_collection_and_query()
            {
                var results = await _connect.Query("my coll").Select(new
                {
                    Test = Aggregations.Sum("Url Encoding")
                }).Execute();
                var uri = _testHandler.Uri;

                Assert.Equal("https://api.getconnect.io/events/my+coll?query={\"select\":{\"Test\":{\"sum\":\"Url+Encoding\"}}}", uri.ToString());
            }
        }

        public class WhenQueryingWithoutAnInterval
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _collection;

            public WhenQueryingWithoutAnInterval()
            {
                var responseText =
                    @"{""metadata"": {""timezone"":""UTC"", ""groups"": [""hello""]},results:[{""hello"": ""world"", ""total"": 10, ""_count"": 1}]}";

                _testHandler = new CaptureHttpHandler(responseText);
                _connect = TestConfigurator.GetTestableClient(_testHandler);
            }

            [Fact]
            public async Task It_should_populate_groups_metadata()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal("hello", results.Metadata.Groups.ElementAt(0));
            }

            [Fact]
            public async Task It_should_generate_the_original_selects()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal(new [] { "total" }, results.Selects().ToArray());
            }

            [Fact]
            public async Task It_should_generate_no_selects_for_empty_results()
            {
                var responseText = @"{""metadata"": {""interval"":""minutely"", ""timezone"":""UTC"", ""groups"": [""hello""]},results:[]}";

                var testHandler = new CaptureHttpHandler(responseText);
                var connect = TestConfigurator.GetTestableClient(testHandler);
                var results = await connect.Query(_collection).Execute();

                Assert.Equal(new string[0], results.Selects().ToArray());
            }

            [Fact]
            public async Task It_should_populate_timezone_metadata()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal("UTC", results.Metadata.Timezone);
            }

            [Fact]
            public async Task It_should_populate_expando_string_results()
            {
                var results = await _connect.Query<ExpandoObject>(_collection).Execute();
                dynamic result = results.Results.ElementAt(0);
                Assert.Equal("world", result.hello);
            }

            [Fact]
            public async Task It_should_populate_dynamic_string_results()
            {
                var results = await _connect.Query<dynamic>(_collection).Execute();
                dynamic result = results.Results.ElementAt(0);
                Assert.Equal("world", result.hello.Value);
            }

            [Fact]
            public async Task It_should_populate_dynamic_number_results()
            {
                var results = await _connect.Query<dynamic>(_collection).Execute();
                dynamic result = results.Results.ElementAt(0);
                Assert.Equal(10, result.total.Value);
            }

            [Fact]
            public async Task It_should_populate_expando_number_results()
            {
                var results = await _connect.Query<ExpandoObject>(_collection).Execute();
                dynamic result = results.Results.ElementAt(0);
                Assert.Equal(10, result.total);
            }

            [Fact]
            public async Task It_should_default_to_dictionary_results()
            {
                var results = await _connect.Query(_collection).Execute();
                var result = results.Results.ElementAt(0);
                Assert.Equal("world", result["hello"]);
            }

            [Fact]
            public async Task It_should_populate_dictionary_string_results()
            {
                var results = await _connect.Query<Dictionary<string, object>>(_collection).Execute();
                var result = results.Results.ElementAt(0);
                Assert.Equal("world", result["hello"]);
            }

            [Fact]
            public async Task It_should_populate_dictionary_number_results()
            {
                var results = await _connect.Query<Dictionary<string, object>>(_collection).Execute();
                var result = results.Results.ElementAt(0);
                Assert.Equal(10L, result["total"]);
            }

            [Fact]
            public async Task It_should_populate_dictionary_poco_results()
            {
                var results = await _connect.Query<Result>(_collection).Execute();
                var result = results.Results.ElementAt(0);
                Assert.Equal("world", result.hello);
            }

            [Fact]
            public async Task It_should_populate_poco_number_results()
            {
                var results = await _connect.Query<Result>(_collection).Execute();
                var result = results.Results.ElementAt(0);
                Assert.Equal(10m, result.total);
            }
        }

        public class WhenQueryingWithInterval
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _collection;

            public WhenQueryingWithInterval()
            {
                var responseText =
                    @"{""metadata"": {""interval"":""minutely"", ""timezone"":""UTC"", ""groups"": [""hello""]},results:[{""interval"":{""start"":""2015-07-01T00:00:00Z"",""end"":""2015-07-01T00:00:00Z""},results:[{""hello"": ""world"", ""total"": 10, ""_count"": 1}]}]}";

                _testHandler = new CaptureHttpHandler(responseText);
                _connect = TestConfigurator.GetTestableClient(_testHandler);
            }

            [Fact]
            public async Task It_should_populate_interval_metadata()
            {
                var results = await _connect.Query(_collection).Minutely().Execute();

                Assert.Equal(Interval.Minutely, results.Metadata.Interval);
            }

            [Fact]
            public async Task It_should_generate_the_original_selects()
            {
                var results = await _connect.Query(_collection).Minutely().Execute();

                Assert.Equal(new[] { "total" }, results.Selects().ToArray());
            }

            [Fact]
            public async Task It_should_generate_no_selects_for_empty_interval_results()
            {
                var responseText = @"{""metadata"": {""interval"":""minutely"", ""timezone"":""UTC"", ""groups"": [""hello""]},results:[{""interval"":{""start"":""2015-07-01T00:00:00Z"",""end"":""2015-07-01T00:00:00Z""},results:[]}]}";

                var testHandler = new CaptureHttpHandler(responseText);
                var connect = TestConfigurator.GetTestableClient(testHandler);
                var results = await connect.Query(_collection).Minutely().Execute();

                Assert.Equal(new string[0], results.Selects().ToArray());
            }

            [Fact]
            public async Task It_should_generate_no_selects_for_empty_results()
            {
                var responseText = @"{""metadata"": {""interval"":""minutely"", ""timezone"":""UTC"", ""groups"": [""hello""]},results:[]}";

                var testHandler = new CaptureHttpHandler(responseText);
                var connect = TestConfigurator.GetTestableClient(testHandler);
                var results = await connect.Query(_collection).Minutely().Execute();

                Assert.Equal(new string[0], results.Selects().ToArray());
            }

            [Fact]
            public async Task It_should_populate_expando_string_results()
            {
                var results = await _connect.Query<ExpandoObject>(_collection).Minutely().Execute();
                dynamic result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal("world", result.hello);
            }

            [Fact]
            public async Task It_should_populate_expando_number_results()
            {
                var results = await _connect.Query<ExpandoObject>(_collection).Minutely().Execute();
                dynamic result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal(10, result.total);
            }

            [Fact]
            public async Task It_should_default_to_dictionary_results()
            {
                var results = await _connect.Query(_collection).Minutely().Execute();
                var result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal("world", result["hello"]);
            }

            [Fact]
            public async Task It_should_populate_dictionary_string_results()
            {
                var results = await _connect.Query<Dictionary<string, object>>(_collection).Minutely().Execute();
                var result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal("world", result["hello"]);
            }

            [Fact]
            public async Task It_should_populate_dictionary_number_results()
            {
                var results = await _connect.Query<Dictionary<string, object>>(_collection).Minutely().Execute();
                var result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal(10L, result["total"]);
            }

            [Fact]
            public async Task It_should_populate_dictionary_poco_results()
            {
                var results = await _connect.Query<Result>(_collection).Minutely().Execute();
                var result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal("world", result.hello);
            }

            [Fact]
            public async Task It_should_populate_poco_number_results()
            {
                var results = await _connect.Query<Result>(_collection).Minutely().Execute();
                var result = results.Results.ElementAt(0).Results.ElementAt(0);
                Assert.Equal(10m, result.total);
            }

            [Fact]
            public async Task It_should_populate_start_date()
            {
                var july = new DateTime(2015, 07, 01, 0, 0, 0, DateTimeKind.Utc);
                var results = await _connect.Query(_collection).Minutely().Execute();
                var start = results.Results.ElementAt(0).Start;
                Assert.Equal(july, start);
            }

            [Fact]
            public async Task It_should_populate_end_date()
            {
                var july = new DateTime(2015, 07, 01, 0, 0, 0, DateTimeKind.Utc);
                var results = await _connect.Query(_collection).Minutely().Execute();
                var end = results.Results.ElementAt(0).End;
                Assert.Equal(july, end);
            }
        }

        public class WhenTheResponseHasAnErrorMessage
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _collection;

            public WhenTheResponseHasAnErrorMessage()
            {
                var responseText = @"{""errorMessage"": ""Hello World""}";
                _testHandler = new CaptureHttpHandler(responseText);
                _connect = TestConfigurator.GetTestableClient(_testHandler);
            }

            [Fact]
            public async Task It_should_populate_the_error_message()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal("Hello World", results.ErrorMessage);
            }
        }

        public class WhenTheResponseHasErrors
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _collection;

            public WhenTheResponseHasErrors()
            {
                var responseText = @"{""errors"":[{""field"":""Hello"",""description"":""World""}]}";
                _testHandler = new CaptureHttpHandler(responseText);
                _connect = TestConfigurator.GetTestableClient(_testHandler);
            }

            [Fact]
            public async Task It_should_populate_the_error_message()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal("World", results.FieldErrors["Hello"]);
            }
        }

        public class WhenTheResponseHasSuccessStatus
        {
            private CaptureHttpHandler _testHandler;
            private ConnectClient _connect;
            private string _collection;

            public WhenTheResponseHasSuccessStatus()
            {
                var responseText = @"{""errors"":[{""field"":""Hello"",""description"":""World""}]}";
                _testHandler = new CaptureHttpHandler(responseText);
                _connect = TestConfigurator.GetTestableClient(_testHandler);
            }

            [Fact]
            public async Task It_should_populate_the_http_status()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal(HttpStatusCode.OK, results.HttpStatusCode);
            }

            [Fact]
            public async Task It_should_be_successful_status()
            {
                var results = await _connect.Query(_collection).Execute();

                Assert.Equal(QueryResponseStatus.Successful, results.Status);
            }
        }
    }
}