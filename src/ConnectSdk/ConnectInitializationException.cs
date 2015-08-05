using System;

namespace ConnectSdk
{
    public class ConnectInitializationException : Exception
    {
        public ConnectInitializationException(string message) : base(message){}
    }
}
