using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using WDNUtils.Common;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// SQL Server parameter base class
    /// </summary>
    public abstract class DBSqlServerParameter : IDisposable
    {
        #region Logger

        private static readonly CachedProperty<ILogAppender> _log = LogAppenderRepository.GetLogAppender(MethodBase.GetCurrentMethod().DeclaringType);
        private static ILogAppender Log => _log.Value;

        #endregion

        #region Properties

        /// <summary>
        /// SQL Server bind parameter
        /// </summary>
        internal SqlParameter Parameter { get; set; }

        /// <summary>
        /// Next element in the list of parameters to dispose when the DBSqlServerBaseDAL is disposed
        /// </summary>
        internal DBSqlServerParameter ParameterListNext { get; set; } = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new bind parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="sqlValue">Parameter initial value</param>
        /// <param name="type">SQL Server bind parameter type</param>
        /// <param name="maxSize">Maximum length for the value (if applicable), may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameter(string parameterName, INullable sqlValue, SqlDbType type, ParameterDirection direction, int? maxSize = null)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName), DBSqlServerLocalizedText.DBSqlServerParameter_InvalidName);

            Parameter = new SqlParameter();

            try
            {
                Parameter.ParameterName = parameterName?.Trim()?.ToUpper();
                Parameter.SqlDbType = type;
                Parameter.Direction = direction;

                Parameter.Size = maxSize ?? 0;
                Parameter.SqlValue = sqlValue;
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        #endregion

        #region Get parameter value

        /// <summary>
        /// Returns the current value of the parameter
        /// </summary>
        /// <returns>The current value of the parameter</returns>
        internal object GetValue()
        {
            var value = Parameter.SqlValue;

            return
                ((value is null) ||
                (value is DBNull) ||
                ((value is INullable nullableValue) && (nullableValue.IsNull)))
                    ? null
                    : value;
        }

        #endregion

        #region Set parameter value

        /// <summary>
        /// Sets the parameter value, disposing the previous value if necessary
        /// </summary>
        /// <param name="sqlValue">New value for the parameter</param>
        internal void SetValue(INullable sqlValue)
        {
            if ((Parameter.Direction != ParameterDirection.Input) && (Parameter.Direction != ParameterDirection.InputOutput))
                throw new InvalidOperationException(string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_InvalidParameterDirectionSetValue, Parameter.ParameterName));

            DisposeParameterValue();

            Parameter.SqlValue = sqlValue;
        }

        #endregion

        #region Release database resources

        /// <summary>
        /// Disposes and clean the bind parameter value
        /// </summary>
        private void DisposeParameterValue()
        {
            var parameter = Parameter;

            if (parameter is null)
                return;

            try
            {
                (parameter.Value as IDisposable)?.Dispose();
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_ValueDisposalError, parameter.ParameterName), ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }

            try
            {
                parameter.SqlValue = null;
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_ValueCleanupError, parameter.ParameterName), ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }
        }

        /// <summary>
        /// Disposes the bind parameter, and its current value if necessary
        /// </summary>
        private void Close()
        {
            DisposeParameterValue();

            Parameter = null;
        }

        #endregion

        #region IDisposable support

        /// <summary>
        /// Indicates if this instance was already disposed, to detect redundant calls.
        /// This is part of the default IDisposable implementation pattern.
        /// </summary>
        private bool DisposedValue = false;

        /// <summary>
        /// Internal implementation of the dispose method, to release managed and/or
        /// non-managed resources, as necessary.
        /// </summary>
        /// <param name="disposing">True if the Dispose(bool) method is being called
        /// during the diposal of this instance (by calling Dispose() or at the end
        /// of using clause), or false if this method is being called by the finalizer
        /// of this class. This is part of the default IDisposable implementation
        /// pattern.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "Parameter",
            Justification = "The field DBSqlServerParameter.Parameter is being disposed by the method DBSqlServerParameter.Close(); this is a bug in the code analysis rule.")]
        private void Dispose(bool disposing)
        {
            if (DisposedValue)
                return;

            try
            {
                // Dispose managed resources

                if (disposing)
                {
                    Close();
                }

                // We don't have non-managed resources to be released.
            }
            finally
            {
                DisposedValue = true;
            }
        }

        /// <summary>
        /// Public implementation of IDisposable.Dispose(), to be called explicity by
        /// the application, or implicity at the end of the using clause. This is part
        /// of the default IDisposable implementation pattern.
        /// </summary>
        public void Dispose()
        {
            // Dispose managed and non-managed resources
            Dispose(true);
        }

        #endregion
    }
}
