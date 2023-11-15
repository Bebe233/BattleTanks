using System;

namespace BEBE.Engine.Logging
{
    [Flags]
    public enum LogServerity
    {
        Exception = 1,
        Error = 2,
        Warn = 4,
        Info = 8,
        Trace = 16
    }
}