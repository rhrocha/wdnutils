using System;

namespace WDNUtils.Common
{
    /// <summary>
    /// Log appender repository
    /// </summary>
    public static class LogAppenderRepository
    {
        #region Constants

        private static readonly ILogAppender DefaultLogAppender = new TraceLogAppender();

        #endregion

        #region Properties

        private static CachedProperty<Func<Type, ILogAppender>> _logAppenderFactory =
            new CachedProperty<Func<Type, ILogAppender>>(() => DefaultLogAppenderFactory);

        /// <summary>
        /// ILogAppender factory
        /// </summary>
        public static Func<Type, ILogAppender> LogAppenderFactory
        {
            private get => _logAppenderFactory.Value;
            set => _logAppenderFactory.Value = value ?? DefaultLogAppenderFactory;
        }

        #endregion

        #region Get log appender

        /// <summary>
        /// Get ILogAppender for a given type
        /// </summary>
        /// <param name="type">Type that is requesting the log appender</param>
        /// <returns>ILogAppender assigned to the type</returns>
        public static CachedProperty<ILogAppender> GetLogAppender(Type type)
        {
            return new CachedProperty<ILogAppender>(
                valueFactory: () => LogAppenderFactory(type),
                references: () => _logAppenderFactory);
        }

        #endregion

        #region Default log appender factory

        /// <summary>
        /// Default log appender factory
        /// </summary>
        /// <param name="type">Type that is requesting the log appender</param>
        /// <returns>ILogAppender assigned to the type</returns>
        private static ILogAppender DefaultLogAppenderFactory(Type type)
        {
            return DefaultLogAppender;
        }

        #endregion
    }
}
