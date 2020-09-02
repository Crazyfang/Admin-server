using System;
using System.Collections.Generic;
using Admin.Core.Common.Attributes;

namespace Admin.Core.Common.Helpers
{
    [SingleInstance]
    public class SignalRDictionary
    {
        public Dictionary<long, string> connections = new Dictionary<long, string>();
    }
}
