using System;
using System.Text;
using System.IO;
using UnityEngine;

namespace BEBE.Framework.Logging
{
    public static class Logger
    {
        public static LogServerity LogServerityLevel = LogServerity.Info | LogServerity.Warn | LogServerity.Error | LogServerity.Exception;
        public static event EventHandler<LogEventArgs> OnMessage = UnityLogHandler;

        static StringBuilder track_sb = new StringBuilder();

        public static void PureLogHandler(object sender, LogEventArgs args)
        {
            switch (args.LogServerity)
            {
                case LogServerity.Info:
                    Console.WriteLine($"Info --> {args.Message}");
                    break;
                case LogServerity.Warn:
                    Console.WriteLine($"Warn --> {args.Message}");
                    break;
                case LogServerity.Error:
                    Console.WriteLine($"Error --> {args.Message}");
                    break;
                case LogServerity.Exception:
                    Console.WriteLine($"Exception --> {args.Ex}");
                    break;
                case LogServerity.Trace:
                    track_sb.AppendLine(args.Message);
                    if (track_sb.Length > trace_dump_length)
                        flushTrace();
                    break;
            }
        }

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
        public static string TraceSavePath => "./tmp/Dump_1.txt";
        private static int trace_dump_length = 1024 * 128;
        private static Stream file_writer;
        public static void DoFlushTrace() => flushTrace();
        private static void flushTrace()
        {
            if (string.IsNullOrEmpty(TraceSavePath)) return;
            var dir = Path.GetDirectoryName(TraceSavePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (file_writer = File.Open(TraceSavePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] bytes = UTF8Encoding.Default.GetBytes(track_sb.ToString());
                file_writer.Write(bytes, 0, bytes.Length);
                file_writer.Flush();
                track_sb.Clear();
            }
        }

        public static void SetLogAllServerities()
        {
            LogServerityLevel = LogServerity.Info | LogServerity.Warn | LogServerity.Error | LogServerity.Exception |
            LogServerity.Trace;
        }

        public static void ResetLogServerities()
        {
            LogServerityLevel = LogServerity.Info | LogServerity.Warn | LogServerity.Error | LogServerity.Exception;
        }

        private static void log_message(object sender, LogServerity sev, string format, params object[] args)
        {
            if (OnMessage != null && (LogServerityLevel & sev) != 0)
            {
                var message = (args != null && args.Length > 0) ? string.Format(format, args) : format;
                OnMessage.Invoke(sender, new LogEventArgs(sev, message));
            }
        }

        private static void log_exception(object sender, LogServerity sev, Exception e)
        {
            if (OnMessage != null && (LogServerityLevel & sev) != 0)
            {
                OnMessage.Invoke(sender, new LogEventArgs(sev, null, e));
            }
        }

        public static void Info(object sender, string message, params object[] args)
        {
            log_message(sender, LogServerity.Info, message, args);
        }

        public static void Warn(object sender, string message, params object[] args)
        {
            log_message(sender, LogServerity.Warn, message, args);
        }

        public static void Error(object sender, string message, params object[] args)
        {
            log_message(sender, LogServerity.Error, message, args);
        }

        public static void Exception(object sender, Exception e)
        {
            log_exception(sender, LogServerity.Exception, e);
        }

        public static void Trace(object sender, string message, params object[] args)
        {
            log_message(sender, LogServerity.Trace, message, args);
        }
    }
}