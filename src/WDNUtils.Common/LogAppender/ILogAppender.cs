using System;

namespace WDNUtils.Common
{
    /// <summary>
    /// Default log interface
    /// </summary>
    public interface ILogAppender
    {
        /// <summary>
        /// Indicates if the debug log level is enabled
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Indicates if the info log level is enabled
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Indicates if the warn log level is enabled
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Indicates if the error log level is enabled
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Indicates if the fatal log level is enabled
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Write debug level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        void Debug(string message);

        /// <summary>
        /// Write debug level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Write info level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        void Info(string message);

        /// <summary>
        /// Write info level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        void Info(string message, Exception exception);

        /// <summary>
        /// Write warn level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        void Warn(string message);

        /// <summary>
        /// Write warn level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        void Warn(string message, Exception exception);

        /// <summary>
        /// Write error level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        void Error(string message);

        /// <summary>
        /// Write error level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Write fatal level message into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        void Fatal(string message);

        /// <summary>
        /// Write fatal level message and an exception into log appender
        /// </summary>
        /// <param name="message">Message to be written into the log appender</param>
        /// <param name="exception">Exception to be dumped into the log appender</param>
        void Fatal(string message, Exception exception);
    }
}
