namespace ConnectSdk.Querying
{
    public class Aggregation
    {
        public string Operator { get; }
        public string PropertyPath { get; }

        public Aggregation(string @operator)
        {
            Operator = @operator;
        }

        public Aggregation(string @operator, string propertyPath)
        {
            PropertyPath = propertyPath;
            Operator = @operator;
        }
    }
}