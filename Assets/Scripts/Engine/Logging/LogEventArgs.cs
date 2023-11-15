using System;

namespace BEBE.Engine.Logging
{
    public class LogEventArgs : EventArgs
    {
        public LogServerity LogServerity { get; }
        public string Message { get; }
        public Exception Ex { get; }
        public LogEventArgs(LogServerity logServerity, string message)
        {
            LogServerity = logServerity;
            Message = message;
        }

        public LogEventArgs(LogServerity logServerity, string message, Exception ex)
        {
            LogServerity = logServerity;
            Message = message;
            Ex = ex;
        }
    }
}