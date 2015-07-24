namespace ConnectSdk.Querying
{
    public class Filter
    {
        public string Operator { get; }
        public object Value { get; }

        public Filter(string @operator, object value)
        {
            Operator = @operator;
            Value = value;
        }
    }
}