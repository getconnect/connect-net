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
        public virtual IEnumerable<KeyValuePair<string, object>> Custom => _data;
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

        public Query(string collection, IEventEndpoint endpoint, AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null, IEnumerable<KeyValuePair<string, object>> custom = null)
        {
            Collection = collection;
            Endpoint = endpoint;
            Select = aggregations;
            Filter = filters;
            Timeframe = timeframe;
            GroupBy = groups;
            Interval = interval;
            Timezone = timezone;

            if (custom == null)
                return;

            _data = new Dictionary<string, object>();

            foreach (var pair in custom)
            {
                _data.Add(pair.Key, pair.Value);
            }
        }
        
        public virtual Query<TNewResultType> UpdateWith<TNewResultType>(AliasedAggregations aggregations = null, FilteredProperties filters = null, ITimeframe timeframe = null, string[] groups = null, Interval? interval = null, object timezone = null, IEnumerable<KeyValuePair<string, object>> custom = null)
        {
            return new Query<TNewResultType>(Collection, Endpoint, aggregations ?? this.Select, filters ?? this.Filter, timeframe ?? this.Timeframe, groups ?? this.GroupBy, interval ?? this.Interval, timezone ?? this.Timezone, custom ?? this.Custom);
        }

        public virtual async Task<QueryResponse<TResult>> Execute()
        {
             return await Endpoint.Query(Collection, this);
        }

        private void OverrideReservedValue(Dictionary<string, object> dict, string key, object value)
        {
            if (value == null)
                return;

            dict.Add(key, value);
        }

        public override string ToString()
        {
            if (_data == null)
                return JsonConvert.SerializeObject(this, Formatting.None, Serializer);

            // Create a new temporary dictionary so we don't actually put our other values into the CustomValue dictionary
            var tempDictionary = _data.ToDictionary(pair => pair.Key, pair => pair.Value);

            // If we don't add these properties to the dictionary the JSON Serializer won't because see them as they aren't part of the Enumerator
            OverrideReservedValue(tempDictionary, ReservedQueryKeys.Select, Select);
            OverrideReservedValue(tempDictionary, ReservedQueryKeys.Filter, Filter);
            OverrideReservedValue(tempDictionary, ReservedQueryKeys.Timeframe, Timeframe);
            OverrideReservedValue(tempDictionary, ReservedQueryKeys.GroupBy, GroupBy);
            OverrideReservedValue(tempDictionary, ReservedQueryKeys.Timezone, Timezone);
            OverrideReservedValue(tempDictionary, ReservedQueryKeys.Interval, Interval);

            return JsonConvert.SerializeObject(tempDictionary, Formatting.None, Serializer);
        }
    }
}