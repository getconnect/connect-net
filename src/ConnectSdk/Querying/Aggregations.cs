using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConnectSdk.Querying
{
    public static class Aggregations
    {
        public static Aggregation Sum(string propertyPath)
        {
            return Use(AggregationOperation.Sum, propertyPath);
        }

        public static Aggregation Min(string propertyPath)
        {
            return Use(AggregationOperation.Min, propertyPath);
        }

        public static Aggregation Max(string propertyPath)
        {
            return Use(AggregationOperation.Max, propertyPath);
        }

        public static Aggregation Avg(string propertyPath)
        {
            return Use(AggregationOperation.Avg, propertyPath);
        }
        
        public static Aggregation Count()
        {
            return Use(AggregationOperation.Count);
        }
        
        public static Aggregation Use(string aggregationOPeration, string propertyPath = null)
        {
            return new Aggregation(aggregationOPeration, propertyPath);
        }
        
        public static Aggregation Use(AggregationOperation aggregationOperation, string propertyPath = null)
        {
            return Use(aggregationOperation.ToString().ToLowerInvariant(), propertyPath);
        }

        public static IQuery<TResult> Select<TResult>(this IQuery<TResult> query, object aggregations)
        {
            var newAggregations = aggregations.GetType()
                                    .GetRuntimeProperties()
                                    .ToDictionary(property => property.Name, 
                                                  property => property.GetValue(aggregations, null) as Aggregation);

            var currentAggregations = query.Select ?? new Dictionary<string, Aggregation>();

            var allAggregations = currentAggregations.Concat(newAggregations)
                                    .ToLookup(pair => pair.Key, pair => pair.Value)
                                    .ToDictionary(group => group.Key, group => group.First());

            return query.UpdateWith<TResult>(aggregations: allAggregations);
        }
    }
}