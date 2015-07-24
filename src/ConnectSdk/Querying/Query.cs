using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ConnectSdk.Api;
using ConnectSdk.Querying.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ConnectSdk.Querying
{
    using AliasedAggregations = IDictionary<string, Aggregation>;
    using FilteredProperties = IDictionary<string, Filter>;

    public class Metadata
    {
        public string[] Groups { get; }
        public Interval? Interval { get; }
        public string Timezone { get; }

        public Metadata(string[] groups = null, Interval? interval = null, string timezone = null)
        {
            Groups = groups;
            Interval = interval;
            Timezone = timezone;
        }
    }

    public class FieldError
    {
        public string Field { get; }
        public string Description { get; }

        public FieldError(string field, string description)
        {
            Field = field;
            Description = description;
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

    public class QueryIntervalResult<TResult>
    {
        public DateTime Start { get; }
        public DateTime End { get; }
        public IEnumerable<TResult> Results { get; }

        public QueryIntervalResult(DateTime start, DateTime end, IEnumerable<TResult> results = null)
        {
            Start = start;
            End = end;
            Results = results;
        }
    }

    public interface IQuery<TResult>
    {
        AliasedAggregations Select { get; }
        FilteredProperties Filter { get; }
        ITimeframe Timeframe { get; }
        string[] GroupBy { get; }
        object Timezone { get; }
        Interval? Interval { get; }
        Query<TNewResultType> UpdateWith<TNewResultType>(AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null);
        Task<QueryResponse<TResult>> Execute();
    }

    public class Query<TResult> : IQuery<TResult>
    {
        public virtual AliasedAggregations Select { get; }
        public virtual FilteredProperties Filter { get; }
        public virtual ITimeframe Timeframe { get; }
        public virtual string[] GroupBy { get; }
        public virtual object Timezone { get; }
        public virtual Interval? Interval { get; }
        protected IEventEndpoint Endpoint;
        protected string Collection;

        protected static readonly JsonSerializerSettings Serializer = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            NullValueHandling = NullValueHandling.Ignore
        };

        static Query ()
        {
            Serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            Serializer.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            Serializer.Converters.Add(new AggregationConverter());
            Serializer.Converters.Add(new DictionaryKeysAsIsConverter<Aggregation>());
            Serializer.Converters.Add(new DictionaryKeysAsIsConverter<Filter>());
            Serializer.Converters.Add(new FilterConverter());
            Serializer.Converters.Add(new PeriodRelativeTimefameConverter());
            Serializer.Converters.Add(new RelativeTimeframeConverter());
        }

        public Query(string collection, IEventEndpoint endpoint, AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null)
        {
            Collection = collection;
            Endpoint = endpoint;
            Select = aggregations;
            Filter = filters;
            Timeframe = timeframe;
            GroupBy = groups;
            Interval = interval;
            Timezone = timezone;
        }

        public virtual Query<TNewResultType> UpdateWith<TNewResultType>(AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null)
        {
            return new  Query<TNewResultType>(Collection, Endpoint, aggregations ?? this.Select, filters ?? this.Filter, timeframe ?? this.Timeframe, groups ?? this.GroupBy, interval ?? this.Interval, timezone ?? this.Timezone);
        }

        public virtual async Task<QueryResponse<TResult>> Execute()
        {
             return await Endpoint.Query(Collection, this);
        }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, Serializer);
        }
    }
}