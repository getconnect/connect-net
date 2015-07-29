using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ConnectSdk.Querying
{
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