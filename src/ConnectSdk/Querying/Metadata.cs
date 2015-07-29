using System.Collections.Generic;

namespace ConnectSdk.Querying
{
    public class Metadata
    {
        public IEnumerable<string> Groups { get; }
        public Interval? Interval { get; }
        public string Timezone { get; }

        public Metadata(string[] groups = null, Interval? interval = null, string timezone = null)
        {
            Groups = groups;
            Interval = interval;
            Timezone = timezone;
        }
    }
}