using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ConnectSdk.Querying
{
    public static class SelectsExtensions
    {
        private static IEnumerable<string> GetSelects<TResult>(Metadata metadata, TResult result)
        {
            var nonSelects = metadata.Groups.Concat(new[] {"_count"});
            return JObject.FromObject(result).Properties()
                .Select(prop => prop.Name)
                .Except(nonSelects)
                .ToArray();
        }

        public static IEnumerable<string> Selects<TResult>(this QueryResponse<QueryIntervalResult<TResult>> queryResult)
        {
            var intervalResultWithResults = queryResult.Results?.FirstOrDefault(result => result.Results.Any());

            if (intervalResultWithResults == null)
                return new string[0];

            var firstResult = intervalResultWithResults.Results.ElementAt(0);
            return GetSelects(queryResult.Metadata, firstResult);
        }

        public static IEnumerable<string> Selects<TResult>(this QueryResponse<TResult> queryResult)
        {
            if (queryResult.Results == null || !queryResult.Results.Any())
                return new string[0];

            var firstResult = queryResult.Results.ElementAt(0);
            return GetSelects(queryResult.Metadata, firstResult);
        }
    }

    public class QueryResponse<TResult>
    {
        public Metadata Metadata { get; }
        public IEnumerable<TResult> Results { get; }
        public string ErrorMessage { get; }
        public IDictionary<string, string> FieldErrors { get; }
        public ResponseStatus? Status { get; set; }
        public HttpStatusCode? HttpStatusCode { get; set; }

        public QueryResponse(Metadata metadata = null, IEnumerable<TResult> results = null, string errorMessage = null, IEnumerable<FieldError> errors = null)
        {
            Metadata = metadata;
            Results = results;
            ErrorMessage = errorMessage;
            FieldErrors = errors?.ToDictionary(fieldError => fieldError.Field, fieldError => fieldError.Description);
        }
    }
}