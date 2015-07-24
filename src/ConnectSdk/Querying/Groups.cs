using System.Linq;

namespace ConnectSdk.Querying
{
    public static class Groups
    {
        public static IQuery<TResult> GroupBy<TResult>(this IQuery<TResult> query, params string[] groups)
        {
            var currentGroups = query.GroupBy ?? new string[0];
            var allGroups = currentGroups.Concat(groups).Distinct().ToArray();

            return query.UpdateWith<TResult>(groups: allGroups);
        }
    }
}