namespace ConnectSdk.Querying
{
    public static class Timezones
    {
        public static IQuery<TResult> Timezone<TResult>(this IQuery<TResult> query, string timezone)
        {
            return query.UpdateWith<TResult>(timezone: timezone);
        }

        public static IQuery<TResult> Timezone<TResult>(this IQuery<TResult> query, decimal offset)
        {
            return query.UpdateWith<TResult>(timezone: offset);
        }
    }
}