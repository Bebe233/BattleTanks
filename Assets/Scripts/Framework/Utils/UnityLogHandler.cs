using System;
using BEBE.Engine.Logging;

namespace BEBE.Engine.Logging
{
    public partial class Logger
    {
        public static void UnityLogHandler(object sender, LogEventArgs args)
        {
            switch (args.LogServerity)
            {
                case LogServerity.Info:
                    UnityEngine.Debug.Log(args.Message);
                    break;
                case LogServerity.Warn:
                    UnityEngine.Debug.LogWarning(args.Message);
                    break;
                case LogServerity.Error:
                    UnityEngine.Debug.LogError(args.Message);
                    break;
                case LogServerity.Exception:
                    UnityEngine.Debug.LogException(args.Ex);
                    break;
                case LogServerity.Trace:
                    track_sb.AppendLine(args.Message);
                    if (track_sb.Length > trace_dump_length)
                        flushTrace();
                    break;
            }
        }
    }
}