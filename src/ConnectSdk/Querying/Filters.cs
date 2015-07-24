using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConnectSdk.Querying
{
    public enum FilterOperation
    {
        Gt,
        Gte,
        Lt,
        Lte,
        Exists,
        Contains,
        StartsWith,
        EndsWith,
        Eq,
        Neq,
        In
    }

    public static class Filters
    {
        public static Filter Gt(object value)
        {
            return WithFilter(FilterOperation.Gt, value);
        }

        public static Filter Gte(object value)
        {
            return WithFilter(FilterOperation.Gte, value);
        }

        public static Filter Lt(object value)
        {
            return WithFilter(FilterOperation.Lt, value);
        }

        public static Filter Lte(object value)
        {
            return WithFilter(FilterOperation.Lte, value);
        }

        public static Filter Exists(bool value)
        {
            return WithFilter(FilterOperation.Exists, value);
        }

        public static Filter Contains(string value)
        {
            return WithFilter(FilterOperation.Contains, value);
        }

        public static Filter StartsWith(string value)
        {
            return WithFilter(FilterOperation.StartsWith, value);
        }

        public static Filter EndsWith(string value)
        {
            return WithFilter(FilterOperation.EndsWith, value);
        }

        public static Filter Eq(object value)
        {
            return WithFilter(FilterOperation.Eq, value);
        }

        public static Filter Neq(object value)
        {
            return WithFilter(FilterOperation.Neq, value);
        }

        public static Filter In<TValue>(IEnumerable<TValue> values)
        {
            return WithFilter(FilterOperation.In, values);
        }

        public static Filter WithFilter(FilterOperation filterOperation, object value)
        {
            return new Filter(filterOperation.ToString(), value);
        }

        public static Filter WithFilter(string filterOperation, object value)
        {
            return new Filter(filterOperation, value);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, object filters)
        {
            var newFilters =
                filters.GetType()
                    .GetRuntimeProperties()
                    .ToDictionary(property => property.Name, property => property.GetValue(filters, null) as Filter);

            return query.Where(newFilters);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, string propertyPath, Filter filter)
        {
            var newFitlers = new Dictionary<string, Filter>
            {
                [propertyPath] = filter
            };

            return query.Where(newFitlers);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, string propertyPath, object value)
        {
            var newFitlers = new Dictionary<string, Filter>
            {
                [propertyPath] = Eq(value)
            };

            return query.Where(newFitlers);
        }

        public static IQuery<TResult> Where<TResult, TValue>(this IQuery<TResult> query, string propertyPath, IEnumerable<TValue> values)
        {
            var newFitlers = new Dictionary<string, Filter>
            {
                [propertyPath] = In(values)
            };

            return query.Where(newFitlers);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, IDictionary<string, Filter> newFilters)
        {
            var currentFilters = query.Filter ?? new Dictionary<string, Filter>();

            var allFilters = newFilters.Concat(currentFilters)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.First());

            return query.UpdateWith<TResult>(filters: allFilters);
        }
    }
}