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
    internal sealed class DBOracleConnectionInstance : IDisposable
    {
        #region Logger

        private static readonly CachedProperty<ILogAppender> _log = LogAppenderRepository.GetLogAppender(MethodBase.GetCurrentMethod().DeclaringType);
        private static ILogAppender Log => _log.Value;

        #endregion

        #region Properties and attributes

        /// <summary>
        /// Reference to the database connection, that is managed by this wrapper class.
        /// </summary>
        public OracleConnection Connection { get; set; } = null;

        /// <summary>
        /// Indicates if the database connection is opened
        /// </summary>
        public bool Connected => ((Connection?.State)?.HasFlag(ConnectionState.Open) == true);

        /// <summary>
        /// Indicates if there is an active transaction for this database connection. Returns false if the connection is not opened.
        /// </summary>
        public bool HasTransaction => ((Connected) && (!(Transaction is null)));

        /// <summary>
        /// Reference to the database transaction, that is managed by this wrapper class.
        /// </summary>
        public OracleTransaction Transaction { get; set; } = null;

        /// <summary>
        /// Lock used for the connection and transaction
        /// </summary>
        public readonly object _connectionLock = new object();

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
        /// Creates a new instance of DBOracleConnectionInstance
        /// </summary>
        /// <param name="databaseConnection">Database connection</param>
        private DBOracleConnectionInstance(OracleConnection databaseConnection)
        {
            Connection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        #endregion

        #region Open connection

        /// <summary>
        /// Opens a new database connection
        /// </summary>
        /// <param name="connectionStringName">Connection string name</param>
        /// <returns>New database connection</returns>
        public static DBOracleConnectionInstance Open(string connectionStringName)
        {
            var connectionString = DBOracleConnectionStrings.Get(connectionStringName: connectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(DBOracleLocalizedText.DBOracleConnection_EmptyConnectionString);

            DBOracleConnectionInstance connection;

            try { }
            finally
            {
                var databaseConnection = new OracleConnection(connectionString);

                try
                {
                    try
                    {
                        databaseConnection.Open();

                        connection = new DBOracleConnectionInstance(databaseConnection: databaseConnection);

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
                    }
                    catch (Exception ex)
                    {
                        ex.ConvertOracleException(isConnecting: true);
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
                        databaseConnection?.Dispose();
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

                    throw;
                }
            }

            return connection;
        }

        #endregion

        #region Create transaction

        /// <summary>
        /// Creates a transaction wrapper, beginning a new transaction if necessary
        /// </summary>
        /// <param name="createSavepoint">Indicates if a savepoint should be created if there is already an active transaction</param>
        /// <param name="isolationLevel">Transaction isolation level, only <see cref="IsolationLevel.ReadCommitted"/> and <see cref="IsolationLevel.Serializable"/> are supported by Oracle managed client (default is <see cref="IsolationLevel.ReadCommitted"/>)</param>
        /// <returns>New transaction wrapper</returns>
        public DBOracleTransaction BeginTransaction(bool createSavepoint = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
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
                            Transaction = Connection.BeginTransaction(isolationLevel);
                        }
                        else
                        {
                            savepointName = $@"{nameof(WDNUtils)}{Interlocked.Increment(ref _savepointCounter).ToString()}";
                            Transaction.Save(savepointName: savepointName);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ConvertOracleException(isConnecting: false);
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

        #region Close connection

        /// <summary>
        /// Closes the database connection, automatically rolling back any pending transactions
        /// </summary>
        public void Close()
        {
            try { }
            finally
            {
                if (!(Connection is null))
                {
                    lock (_connectionLock)
                    {
                        if (!(Connection is null))
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

                            try
                            {
                                if (Connected)
                                {
                                    Connection?.Close();

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
                                Connection?.Dispose();
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

                            Connection = null;
                        }
                    }
                }
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
