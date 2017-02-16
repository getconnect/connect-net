using System;
using System.Collections.Generic;

namespace ConnectSdk.Querying
{
    public static class CustomValues
    { 
        public static IQuery<TResult> Custom<TResult>(this IQuery<TResult> query, IEnumerable<KeyValuePair<string, object>> customValues)
        {
            var currentCustomValues = query.Custom ?? new Dictionary<string, object>();
            var newCustomValues = new Dictionary<string, object>();

            foreach (var pair in currentCustomValues)
            {
                newCustomValues.Add(pair.Key, pair.Value);
            }

            foreach (var pair in customValues)
            {
                if (ReservedQueryKeys.IsReservedKey(pair.Key))
                {
                    throw new ArgumentException($"{pair.Key} is a reserved key and cannot be used for a custom property.");
                }

                // Override existing value
                if (newCustomValues.ContainsKey(pair.Key))
                {
                    newCustomValues[pair.Key] = pair.Value;
                }
                else
                {
                    newCustomValues.Add(pair.Key, pair.Value);
                }
            }
            
            return query.UpdateWith<TResult>(custom: newCustomValues);
        }
    }
}