using log4net;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using WDNUtils.Common;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Database connection wrapper, supporting transactions, with a custom IDisposable
    /// implementation to prevent exception overriding at the end of the 'using' clause.
    /// </summary>
    public sealed class DBOracleConnection : IDisposable
    {
        #region Logger

        private static CachedProperty<ILog> _log = new CachedProperty<ILog>(() => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
        private static ILog Log => _log.Value;

        #endregion

        #region Properties and attributes

        /// <summary>
        /// Connection string name
        /// </summary>
        public string ConnectionStringName { get; private set; }

        /// <summary>
        /// Reference to the database connection, that is managed by this wrapper class.
        /// </summary>
        private OracleConnection OracleConnection { get; set; } = null;

        /// <summary>
        /// Indicates if the database connection is opened
        /// </summary>
        /// <remarks>ODP.NET only supports Open and Close for ConnectionState</remarks>
        public bool Connected => (OracleConnection?.State == ConnectionState.Open);

        /// <summary>
        /// Indicates if there is an active transaction for this database connection. Returns false if the connection is not opened.
        /// </summary>
        public bool HasTransaction => ((Connected) && (!(Transaction is null)));

        /// <summary>
        /// Reference to the database transaction, that is managed by this wrapper class.
        /// </summary>
        internal OracleTransaction Transaction { get; set; } = null;

        /// <summary>
        /// Lock used for the connection and transaction
        /// </summary>
        internal readonly object _connectionLock = new object();

        /// <summary>
        /// Counter used to create unique savepoint names
        /// </summary>
        private long _savepointCounter = 0;

#if DEBUG_ORACLE
        /// <summary>
        /// Connection counter, for debugging only
        /// </summary>
        private static long _connectionCounter = 0;
#endif

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new database connection (does not open it, used by DBOracleConnectionContainer)
        /// </summary>
        /// <param name="connectionStringName">Connection string name</param>
        internal DBOracleConnection(string connectionStringName)
        {
            ConnectionStringName = connectionStringName ?? throw new ArgumentNullException(nameof(connectionStringName));
        }

        #endregion

        #region Open connection

        /// <summary>
        /// Opens the database connection if necessary
        /// </summary>
        /// <returns>True if a new connection was opened, false if the connection was already opened</returns>
        internal bool OpenConnection()
        {
            if (Connected)
                return false;

            lock (_connectionLock)
            {
                if (Connected)
                    return false;

                var connectionString = DBOracleConnectionStrings.Get(connectionStringName: ConnectionStringName);

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException(DBOracleLocalizedText.DBOracleConnection_EmptyConnectionString);

                OracleConnection = new OracleConnection(connectionString);

                try
                {
                    try
                    {
                        OracleConnection.Open();
                    }
                    catch (Exception ex)
                    {
                        ex.ConvertOracleException();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        Log.Error(DBOracleLocalizedText.DBOracleConnection_Constructor_Error, ex);
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }

                    try
                    {
                        OracleConnection?.Dispose();
                    }
                    catch (Exception ex2)
                    {
                        try
                        {
                            Log.Error(DBOracleLocalizedText.DBOracleConnection_Disposal_Error, ex2);
                        }
                        catch (Exception)
                        {
                            // Nothing to do
                        }
                    }

                    OracleConnection = null;
                    throw;
                }

#if DEBUG_ORACLE
            try
            {
                Log.Debug($@"Database connection opened - total: {System.Threading.Interlocked.Increment(ref _connectionCounter)}");
            }
            catch (Exception)
            {
                // Nothing to do
            }
#endif

                return true;
            }
        }

        #endregion

        #region Create transaction

        /// <summary>
        /// Creates a transaction wrapper, beginning a new transaction if necessary
        /// </summary>
        /// <param name="createSavepoint">Indicates if a savepoint should be created if there is already an active transaction</param>
        /// <param name="serializableIsolationLevel">Indicates if the transaction isolation level should be 'SERIALIZABLE' instead of 'READ COMMITTED' (default is false, to use 'READ COMMITTED')</param>
        /// <returns>New transaction wrapper</returns>
        public DBOracleTransaction BeginTransaction(bool createSavepoint = false, bool serializableIsolationLevel = false)
        {
            if (!Connected)
                throw new InvalidOperationException(DBOracleLocalizedText.DBOracleConnection_BeginTransaction_Closed);

            lock (_connectionLock)
            {
                if ((!(Transaction is null)) && (!createSavepoint))
                    return new DBOracleTransaction(connection: null, savepointName: null);

                string savepointName = null;

                try
                {
                    try
                    {
                        if (Transaction is null)
                        {
                            // Oracle supports only 'READ COMMITTED' and 'SERIALIZABLE' isolation levels
                            Transaction = (serializableIsolationLevel)
                                ? OracleConnection.BeginTransaction(IsolationLevel.Serializable)
                                : OracleConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                        }
                        else
                        {
                            savepointName = $@"{nameof(WDNUtils)}{Interlocked.Increment(ref _savepointCounter).ToString()}";
                            Transaction.Save(savepointName: savepointName);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ConvertOracleException();
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (savepointName is null)
                        {
                            Log.Debug(DBOracleLocalizedText.DBOracleConnection_BeginTransaction_Error, ex);
                        }
                        else
                        {
                            Log.Debug(string.Format(DBOracleLocalizedText.DBOracleConnection_CreateSavepoint_Error, savepointName), ex);
                        }
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }

                    throw;
                }

                return new DBOracleTransaction(connection: this, savepointName: savepointName);
            }
        }

        #endregion

        #region Create command

        /// <summary>
        /// Creates a database command (used by DBOracleCommand)
        /// </summary>
        /// <returns>New database command</returns>
        internal OracleCommand CreateCommand()
        {
            return OracleConnection.CreateCommand();
        }

        #endregion

        #region Close connection

        /// <summary>
        /// Closes the database connection, automatically rolling back any pending transactions
        /// </summary>
        internal void Close()
        {
            if (OracleConnection is null)
                return;

            lock (_connectionLock)
            {
                if (OracleConnection is null)
                    return;

                if (!(Transaction is null))
                {
                    lock (_connectionLock)
                    {
                        if (!(Transaction is null))
                        {
                            try
                            {
                                Transaction.Rollback();

                                try
                                {
                                    Log.Error(DBOracleLocalizedText.DBOracleConnection_ClosedWithActiveTransaction);
                                }
                                catch (Exception)
                                {
                                    // Nothing to do
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    Log.Error(DBOracleLocalizedText.DBOracleConnection_Rollback_Error, ex);
                                }
                                catch (Exception)
                                {
                                    // Nothing to do
                                }
                            }

                            Transaction = null;
                        }
                    }
                }

                try
                {
                    if (Connected)
                    {
                        OracleConnection?.Close();

#if DEBUG_ORACLE
                    try
                    {
                        Log.Debug($@"Database connection closed - total: {System.Threading.Interlocked.Decrement(ref _connectionCounter)}");
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }
#endif
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        Log.Error(DBOracleLocalizedText.DBOracleConnection_Close_Error, ex);
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }
                }

                try
                {
                    OracleConnection?.Dispose();
                }
                catch (Exception ex)
                {
                    try
                    {
                        Log.Error(DBOracleLocalizedText.DBOracleConnection_Disposal_Error, ex);
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }
                }

                OracleConnection = null;
            }
        }

        #endregion

        #region IDisposable Support

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
