using System;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Disposable wrapper for a database connection (the connection will be closed if it was opened by this class, but will remain opened if it was already opened)
    /// </summary>
    public class DBOracleConnectionContainer : IDisposable
    {
        #region Properties

        /// <summary>
        /// Database connection
        /// </summary>
        protected DBOracleConnection Connection { get; set; }

        /// <summary>
        /// Indicates if the database connection should be closed and disposed when this object is disposed
        /// </summary>
        private bool DisposeConnection { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a disposable container with a database connection, creating a new connection if necessary
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="connectionStringName">Connection string name</param>
        public DBOracleConnectionContainer(ref DBOracleConnection connection, string connectionStringName = null)
        {
            if (connection is null)
            {
                if (connectionStringName is null)
                    throw new ArgumentNullException(nameof(connection));

                connection = new DBOracleConnection(connectionStringName);
            }

            Connection = connection;
            DisposeConnection = connection.OpenConnection();
        }

        #endregion

        #region Close database connection

        /// <summary>
        /// Dispose the database connection if applicable
        /// </summary>
        internal virtual void Close()
        {
            if (DisposeConnection)
            {
                // DBOracleConnection.Close wont throw exceptions
                Connection?.Close();
            }

            Connection = null;
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
