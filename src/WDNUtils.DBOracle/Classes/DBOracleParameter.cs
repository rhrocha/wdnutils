using log4net;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Reflection;
using WDNUtils.Common;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Oracle parameter base class
    /// </summary>
    public abstract class DBOracleParameter : IDisposable
    {
        #region Logger

        private static CachedProperty<ILog> _log = new CachedProperty<ILog>(() => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
        private static ILog Log => _log.Value;

        #endregion

        #region Properties

        /// <summary>
        /// Oracle bind parameter
        /// </summary>
        internal OracleParameter Parameter { get; set; }

        /// <summary>
        /// Next element in the list of parameters to dispose when the DBOracleBaseDAL is disposed
        /// </summary>
        internal DBOracleParameter ParameterListNext { get; set; } = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new bind parameter
        /// </summary>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="type">Oracle bind parameter type</param>
        /// <param name="maxSize">Maximum length for the value (if applicable), may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameter(string parameterName, object value, OracleDbType type, ParameterDirection direction, int? maxSize = null)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName), DBOracleLocalizedText.DBOracleParameter_InvalidName);

            Parameter = new OracleParameter();

            try
            {
                Parameter.ParameterName = parameterName?.Trim()?.ToUpper();
                Parameter.OracleDbType = type;
                Parameter.Direction = direction;

                Parameter.Size = maxSize ?? 0;

                if ((direction == ParameterDirection.Input) || (direction == ParameterDirection.InputOutput))
                {
                    Parameter.Value = value ?? DBNull.Value;
                }
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
            var value = Parameter.Value;

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
        /// <param name="value">New value for the parameter</param>
        internal void SetValue(object value)
        {
            if ((Parameter.Direction != ParameterDirection.Input) && (Parameter.Direction != ParameterDirection.InputOutput))
                throw new InvalidOperationException(string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidParameterDirectionSetValue, Parameter.ParameterName));

            DisposeParameterValue();

            Parameter.Value = value ?? DBNull.Value;
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
                    Log.Error(string.Format(DBOracleLocalizedText.DBOracleParameter_ValueDisposalError, parameter.ParameterName), ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }

            try
            {
                parameter.Value = null;
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(string.Format(DBOracleLocalizedText.DBOracleParameter_ValueCleanupError, parameter.ParameterName), ex);
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

            try
            {
                Parameter?.Dispose();
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(string.Format(DBOracleLocalizedText.DBOracleParameter_ParameterDisposalError, Parameter.ParameterName), ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }

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
            Justification = "The field DBOracleParameter.Parameter is being disposed by the method DBOracleParameter.Close(); this is a bug in the code analysis rule.")]
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
