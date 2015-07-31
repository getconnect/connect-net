using System;
using System.Collections.Generic;

namespace ConnectSdk.Querying
{
    public enum RelativeTimeWindow
    {
        ThisMinute,
        LastMinute,
        ThisHour,
        LastHour,
        Today,
        Yesterday,
        ThisWeek,
        LastWeek,
        ThisMonth,
        LastMonth,
        ThisQuarter,
        LastQuarter,
        ThisYear,
        LastYear
    }

    public static class Timeframes
    {
        public static readonly Dictionary<RelativeTimeWindow, string> RelativeTimeWindowMap =
            new Dictionary<RelativeTimeWindow, string>
            {
                [RelativeTimeWindow.ThisMinute] = "this_minute",
                [RelativeTimeWindow.LastMinute] = "last_minute",
                [RelativeTimeWindow.ThisHour] = "this_hour",
                [RelativeTimeWindow.LastHour] = "last_hour",
                [RelativeTimeWindow.ThisWeek] = "this_week",
                [RelativeTimeWindow.LastWeek] = "last_week",
                [RelativeTimeWindow.Today] = "today",
                [RelativeTimeWindow.Yesterday] = "yesterday",
                [RelativeTimeWindow.ThisMonth] = "this_month",
                [RelativeTimeWindow.LastMonth] = "last_month",
                [RelativeTimeWindow.ThisQuarter] = "this_quarter",
                [RelativeTimeWindow.LastQuarter] = "last_quarter",
                [RelativeTimeWindow.ThisYear] = "this_year",
                [RelativeTimeWindow.LastYear] = "last_year",
            };

        public static IQuery<TResult> Between<TResult>(this IQuery<TResult> query, DateTime start, DateTime end)
        {
            return query.UpdateWith<TResult>(timeframe: new AbsoluteTimeframe(start, end));
        }

        public static IQuery<TResult> StartingAt<TResult>(this IQuery<TResult> query, DateTime start)
        {
            return query.UpdateWith<TResult>(timeframe: new AbsoluteTimeframe(start: start));
        }

        public static IQuery<TResult> EndingAt<TResult>(this IQuery<TResult> query, DateTime end)
        {
            return query.UpdateWith<TResult>(timeframe: new AbsoluteTimeframe(end: end));
        }

        public static IQuery<TResult> Previous<TResult>(this IQuery<TResult> query, int value, TimeType timeType)
        {
            return query.UpdateWith<TResult>(timeframe: new PeriodRelativeTimefame("previous", timeType, value));
        }

        public static IQuery<TResult> Current<TResult>(this IQuery<TResult> query, int value, TimeType timeType)
        {
            return query.UpdateWith<TResult>(timeframe: new PeriodRelativeTimefame("current", timeType, value));
        }

        public static IQuery<TResult> ThisMinute<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.ThisMinute);
        }

        public static IQuery<TResult> LastMinute<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.LastMinute);
        }

        public static IQuery<TResult> ThisHour<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.ThisHour);
        }

        public static IQuery<TResult> LastHour<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.LastHour);
        }

        public static IQuery<TResult> Today<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.Today);
        }

        public static IQuery<TResult> Yesterday<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.Yesterday);
        }

        public static IQuery<TResult> ThisWeek<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.ThisWeek);
        }

        public static IQuery<TResult> LastWeek<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.LastWeek);
        }

        public static IQuery<TResult> ThisMonth<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.ThisMonth);
        }

        public static IQuery<TResult> LastMonth<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.LastMonth);
        }

        public static IQuery<TResult> ThisQuarter<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.ThisQuarter);
        }

        public static IQuery<TResult> LastQuarter<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.LastQuarter);
        }

        public static IQuery<TResult> ThisYear<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.ThisYear);
        }

        public static IQuery<TResult> LastYear<TResult>(this IQuery<TResult> query)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindow.LastYear);
        }

        public static IQuery<TResult> WithRelativeTimeframe<TResult>(this IQuery<TResult> query, string window)
        {
            return query.UpdateWith<TResult>(timeframe: new RelativeTimefame(window));
        }

        public static IQuery<TResult> WithRelativeTimeframe<TResult>(this IQuery<TResult> query, RelativeTimeWindow window)
        {
            return query.WithRelativeTimeframe(RelativeTimeWindowMap[window]);
        }
    }
}