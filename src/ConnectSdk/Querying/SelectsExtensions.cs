using System.Collections.Generic;
using System.Linq;
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
}