using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConnectSdk.Querying
{
    public static class Filters
    {
        public static Filter Gt(object value)
        {
            return By(FilterOperation.Gt, value);
        }

        public static Filter Gte(object value)
        {
            return By(FilterOperation.Gte, value);
        }

        public static Filter Lt(object value)
        {
            return By(FilterOperation.Lt, value);
        }

        public static Filter Lte(object value)
        {
            return By(FilterOperation.Lte, value);
        }

        public static Filter Exists(bool value)
        {
            return By(FilterOperation.Exists, value);
        }

        public static Filter Contains(string value)
        {
            return By(FilterOperation.Contains, value);
        }

        public static Filter StartsWith(string value)
        {
            return By(FilterOperation.StartsWith, value);
        }

        public static Filter EndsWith(string value)
        {
            return By(FilterOperation.EndsWith, value);
        }

        public static Filter Eq(object value)
        {
            return By(FilterOperation.Eq, value);
        }

        public static Filter Neq(object value)
        {
            return By(FilterOperation.Neq, value);
        }

        public static Filter In<TValue>(IEnumerable<TValue> values)
        {
            return By(FilterOperation.In, values);
        }

        public static Filter By(FilterOperation filterOperation, object value)
        {
            return new Filter(filterOperation.ToString(), value);
        }

        public static Filter By(string filterOperation, object value)
        {
            return new Filter(filterOperation, value);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, object filters)
        {
            var newFilters = filters.GetType()
                                .GetRuntimeProperties()
                                .ToDictionary(property => property.Name,
                                    property =>
                                    {
                                        var filter = property.GetValue(filters, null) as Filter;
                                        var andFilters = property.GetValue(filters, null) as Filter[];

                                        if (filter != null)
                                            return new [] {filter}.AsEnumerable();

                                        return andFilters.AsEnumerable();
                                    });

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

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, string propertyPath, string value)
        {
            var newFitlers = new Dictionary<string, Filter>
            {
                [propertyPath] = Eq(value)
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
            var parsedFilters = newFilters
                .ToDictionary(propFilter => propFilter.Key, propFilter => new[] {propFilter.Value}.AsEnumerable());

            return query.Where(parsedFilters);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, IDictionary<string, Filter[]> newFilters)
        {
            var parsedFilters = newFilters
                .ToDictionary(propFilter => propFilter.Key, propFilter => propFilter.Value.AsEnumerable());

            return query.Where(parsedFilters);
        }

        public static IQuery<TResult> Where<TResult>(this IQuery<TResult> query, IDictionary<string, IEnumerable<Filter>> newFilters)
        {
            var currentFilters = query.Filter ?? new Dictionary<string, IEnumerable<Filter>>();

            var allFilters = newFilters.Concat(currentFilters)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(group => group.Key, group =>
                {
                    return group.SelectMany(filters => filters.ToArray())
                        .ToLookup(filter => filter.Operator, filter => filter.Value)
                        .Select(keyedValues => new Filter(keyedValues.Key, keyedValues.First()));
                });

            return query.UpdateWith<TResult>(filters: allFilters);
        }
    }
}