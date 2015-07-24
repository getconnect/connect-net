namespace ConnectSdk.Querying
{
    public class AliasedAggregation
    {
        public string Alias { get; }
        public Aggregation Aggregation { get; }

        public AliasedAggregation(string alias, Aggregation aggregation)
        {
            Alias = alias;
            Aggregation = aggregation;
        }
    }
}