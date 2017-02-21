using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectSdk.Api;
using ConnectSdk.Querying.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ConnectSdk.Querying
{
    using AliasedAggregations = IDictionary<string, Aggregation>;
    using FilteredProperties = IDictionary<string, IEnumerable<Filter>>;

    public class Query<TResult> : IQuery<TResult>
    {
        private readonly Dictionary<string, object> _data;
        public virtual AliasedAggregations Select { get; }
        public virtual FilteredProperties Filter { get; }
        public virtual ITimeframe Timeframe { get; }
        public virtual string[] GroupBy { get; }
        public virtual object Timezone { get; }
        public virtual Interval? Interval { get; }
        [JsonExtensionData]
        // Setter is required by JsonExtensionData otherwise a null exception will be thrown on serialization
        public virtual IDictionary<string, object> Custom { get; private set; }
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
            Serializer.Converters.Add(new AggregationsConverter());
            Serializer.Converters.Add(new FilterConverter());
            Serializer.Converters.Add(new FiltersConverter());
            Serializer.Converters.Add(new PeriodRelativeTimefameConverter());
            Serializer.Converters.Add(new RelativeTimeframeConverter());
        }

        public Query(string collection, IEventEndpoint endpoint, AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null, IDictionary<string, object> custom = null)
        {
            Collection = collection;
            Endpoint = endpoint;
            Select = aggregations;
            Filter = filters;
            Timeframe = timeframe;
            GroupBy = groups;
            Interval = interval;
            Timezone = timezone;
            Custom = custom;
        }
        
        public virtual Query<TNewResultType> UpdateWith<TNewResultType>(AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null, IDictionary<string, object> custom = null)
        {
            return new Query<TNewResultType>(Collection, Endpoint, aggregations ?? this.Select, filters ?? this.Filter, timeframe ?? this.Timeframe, groups ?? this.GroupBy, interval ?? this.Interval, timezone ?? this.Timezone, custom ?? this.Custom);
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