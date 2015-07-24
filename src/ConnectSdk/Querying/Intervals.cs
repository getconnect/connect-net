namespace ConnectSdk.Querying
{
    public enum Interval
    {
        Minutely,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    public static class Intervals
    {
        public static IQuery<QueryIntervalResult<TResult>> Minutely<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Minutely);
        }

        public static IQuery<QueryIntervalResult<TResult>> Hourly<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Hourly);
        }

        public static IQuery<QueryIntervalResult<TResult>> Daily<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Daily);
        }

        public static IQuery<QueryIntervalResult<TResult>> Weekly<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Weekly);
        }

        public static IQuery<QueryIntervalResult<TResult>> Monthly<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Monthly);
        }

        public static IQuery<QueryIntervalResult<TResult>> Quarterly<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Quarterly);
        }

        public static IQuery<QueryIntervalResult<TResult>> Yearly<TResult>(this IQuery<TResult> query)
        {
            return query.WithInterval(Interval.Yearly);
        }

        public static IQuery<QueryIntervalResult<TResult>> WithInterval<TResult>(this IQuery<TResult> query, Interval interval)
        {
            return query.UpdateWith<QueryIntervalResult<TResult>>(interval: interval);
        }
    }
}