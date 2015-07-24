namespace ConnectSdk.Querying
{
    public class PropertyFilter
    {
        public Filter Filter { get; }

        public PropertyFilter(Filter filter)
        {
            Filter = filter;
        }
    }
}