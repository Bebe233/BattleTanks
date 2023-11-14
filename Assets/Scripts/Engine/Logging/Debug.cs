using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BEBE.Framework.Logging
{
    public static class Debug
    {
        public static string prefix { get; set; }
        public static string TraceSavePath { get { return Logger.TraceSavePath; } }
        private static bool isTraceMode = false;
        public static void TraceModeOn()
        {
            Logger.SetLogAllServerities();
            isTraceMode = true;
        }

        public static void TraceModeOff()
        {
            isTraceMode = false;
        }

        public static void Log(string format, params object[] args)
        {
            Logger.Info(0, prefix + format, args);
            if (isTraceMode) Trace(" Log --> " + format, args);
        }

        public static void LogWarning(string format, params object[] args)
        {
            Logger.Warn(0, prefix + format, args);
            if (isTraceMode) Trace(" Warn --> " + format, args);
        }

        public static void LogError(string format, params object[] args)
        {
            Logger.Error(0, prefix + format, args);
            if (isTraceMode) Trace(" Error --> " + format, args);
        }

        public static void LogException(Exception e)
        {
            Logger.Exception(0, e);
            if (isTraceMode) Trace(" Exception --> " + e);
        }

        public static void Trace(string format, params object[] args)
        {
            Logger.Trace(0, prefix + format, args);
        }

        public static void FlushTrace() => Logger.DoFlushTrace();
    }
}