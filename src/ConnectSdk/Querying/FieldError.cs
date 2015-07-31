namespace ConnectSdk.Querying
{
    public class FieldError
    {
        public string Field { get; }
        public string Description { get; }

        public FieldError(string field, string description)
        {
            Field = field;
            Description = description;
        }
    }
}