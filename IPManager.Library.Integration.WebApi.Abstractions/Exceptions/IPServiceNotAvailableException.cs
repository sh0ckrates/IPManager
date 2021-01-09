using System;
using System.Collections.Generic;
using System.Text;

namespace IPManager.Library.Integration.ExternalApi.Abstractions.Exceptions
{
    public class IPServiceNotAvailableException : Exception
    {
        public IPServiceNotAvailableException(string methodName)
                : base($"{methodName} method failed.") { }
    }
}
