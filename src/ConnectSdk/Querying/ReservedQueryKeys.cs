using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectSdk.Querying
{
    internal class ReservedQueryKeys
    {
        public static string Collection = "collection";
        public static string Endpoint = "endpoint";
        public static string Select = "select";
        public static string Filter = "filter";
        public static string GroupBy = "groupBy";
        public static string Timezone = "timezone";
        public static string Timeframe = "timeframe";
        public static string Interval = "interval";

        private static readonly List<string> ReservedKeys = new List<string>
        {
            Collection,
            Endpoint,
            Select,
            Filter,
            GroupBy,
            Timezone,
            Timeframe,
            Interval
        };

        public static bool IsReservedKey(string key)
        {
            return ReservedKeys.Any(r => string.Equals(r, key, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
