using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectSdk
{
    public class EventDataValidationException : Exception
    {
        public EventDataValidationException(string reservedPrefix, IEnumerable<string> invalidPropertyNames)
            : base(invalidPropertyNames.Aggregate(
                string.Format("The following properties use the reserved prefix '{0}':{1}", reservedPrefix, Environment.NewLine),
                (message, propertyName) => string.Format("{0}->{1}{2}", message, propertyName, Environment.NewLine)
            ))
        {
            InvalidPropertyNames = invalidPropertyNames;
        }

        public IEnumerable<string> InvalidPropertyNames { get; private set; }
    }
}
