using System;
using WDNUtils.Common;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Console log appender
    /// </summary>
    public sealed class ConsoleLogAppender : ILogAppender
    {
        /// <summary>
        /// Indicates if the debug log level is enabled
        /// </summary>
        public bool IsDebugEnabled { get; set; } = false;

        /// <summary>
        /// Indicates if the info log level is enabled
        /// </summary>
        public bool IsInfoEnabled { get; set; } = true;

        /// <summary>
        /// Indicates if the warn log level is enabled
        /// </summary>
        public bool IsWarnEnabled { get; set; } = true;

        /// <summary>
        /// Indicates if the error log level is enabled
        /// </summary>
        public bool IsErrorEnabled { get; set; } = true;

        /// <summary>
        /// Indicates if the fatal log level is enabled
        /// </summary>
        public bool IsFatalEnabled { get; set; } = true;

        /// <summary>
        /// Write debug level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Debug(string message)
        {
            ConsoleWrite(@"DEBUG", message);
        }

        /// <summary>
        /// Write debug level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Debug(string message, Exception exception)
        {
            ConsoleWrite(@"DEBUG", message, exception);
        }

        /// <summary>
        /// Write info level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Info(string message)
        {
            ConsoleWrite(@"INFO ", message);
        }

        /// <summary>
        /// Write info level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Info(string message, Exception exception)
        {
            ConsoleWrite(@"INFO ", message, exception);
        }

        /// <summary>
        /// Write warn level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Warn(string message)
        {
            ConsoleWrite(@"WARN ", message);
        }

        /// <summary>
        /// Write warn level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Warn(string message, Exception exception)
        {
            ConsoleWrite(@"WARN ", message, exception);
        }

        /// <summary>
        /// Write error level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Error(string message)
        {
            ConsoleWrite(@"ERROR", message);
        }

        /// <summary>
        /// Write error level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Error(string message, Exception exception)
        {
            ConsoleWrite(@"ERROR", message, exception);
        }

        /// <summary>
        /// Write fatal level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        public void Fatal(string message)
        {
            ConsoleWrite(@"FATAL", message);
        }

        /// <summary>
        /// Write fatal level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        public void Fatal(string message, Exception exception)
        {
            ConsoleWrite(@"FATAL", message, exception);
        }

        /// <summary>
        /// Write message and exception into debugger output
        /// </summary>
        /// <param name="level">Log level</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Exception (may be null)</param>
        private void ConsoleWrite(string level, string message, Exception exception = null)
        {
            Console.WriteLine($@"{DateTime.Now.ToString(DateUtils.DateTimeFormat.ISOShort)} [{level}] {message}");

            if (!(exception is null))
            {
                Console.WriteLine($@"{exception.GetType().Name}: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
            }
        }
    }
}
