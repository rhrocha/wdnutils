using System;
using System.Diagnostics;

namespace WDNUtils.Common
{
    /// <summary>
    /// Trace log appender (writes all messages into debugger output)
    /// </summary>
    public sealed class TraceLogAppender : ILogAppender
    {
        /// <summary>
        /// Indicates if the debug log level is enabled
        /// </summary>
        public bool IsDebugEnabled => Debugger.IsAttached;

        /// <summary>
        /// Indicates if the info log level is enabled
        /// </summary>
        public bool IsInfoEnabled => Debugger.IsAttached;

        /// <summary>
        /// Indicates if the warn log level is enabled
        /// </summary>
        public bool IsWarnEnabled => Debugger.IsAttached;

        /// <summary>
        /// Indicates if the error log level is enabled
        /// </summary>
        public bool IsErrorEnabled => Debugger.IsAttached;

        /// <summary>
        /// Indicates if the fatal log level is enabled
        /// </summary>
        public bool IsFatalEnabled => Debugger.IsAttached;

        /// <summary>
        /// Write debug level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Debug(string message)
        {
            TraceMessage(@"DEBUG", message);
        }

        /// <summary>
        /// Write debug level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Debug(string message, Exception exception)
        {
            TraceMessage(@"DEBUG", message, exception);
        }

        /// <summary>
        /// Write info level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Info(string message)
        {
            TraceMessage(@"INFO ", message);
        }

        /// <summary>
        /// Write info level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Info(string message, Exception exception)
        {
            TraceMessage(@"INFO ", message, exception);
        }

        /// <summary>
        /// Write warn level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Warn(string message)
        {
            TraceMessage(@"WARN ", message);
        }

        /// <summary>
        /// Write warn level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Warn(string message, Exception exception)
        {
            TraceMessage(@"WARN ", message, exception);
        }

        /// <summary>
        /// Write error level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Error(string message)
        {
            TraceMessage(@"ERROR", message);
        }

        /// <summary>
        /// Write error level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Error(string message, Exception exception)
        {
            TraceMessage(@"ERROR", message, exception);
        }

        /// <summary>
        /// Write fatal level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Fatal(string message)
        {
            TraceMessage(@"FATAL", message);
        }

        /// <summary>
        /// Write fatal level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Fatal(string message, Exception exception)
        {
            TraceMessage(@"FATAL", message, exception);
        }

        /// <summary>
        /// Write message and exception into debugger output
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception (may be null)</param>
        private void TraceMessage(string level, string message, Exception exception = null)
        {
            if (!Debugger.IsAttached)
                return;

            System.Diagnostics.Debug.WriteLine($@"{DateTime.Now.ToString(DateUtils.DateTimeFormat.ISOShort)} [{level}] {message}");

            if (!(exception is null))
            {
                System.Diagnostics.Debug.WriteLine($@"{exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
            }
        }
    }
}
