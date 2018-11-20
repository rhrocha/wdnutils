using System;
using System.Threading.Tasks;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Prepared command
    /// </summary>
    public sealed class DBSqlServerPreparedStatement : IDisposable
    {
        #region Properties

        /// <summary>
        /// Database command
        /// </summary>
        private DBSqlServerCommand Command { get; set; }

        /// <summary>
        /// Next element in the list of prepared statements to dispose when this object is disposed
        /// </summary>
        internal DBSqlServerPreparedStatement PreparedStatementListNext { get; set; } = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new prepared statement (used by DBSqlServerBaseDAL)
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        internal DBSqlServerPreparedStatement(DBSqlServerConnection connection, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            try
            {
                Command = DBSqlServerCommand.CreatePreparedStatement(
                    connection: connection,
                    commandText: commandText,
                    isStoredProcedure: isStoredProcedure,
                    parameters: parameters);
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Execute prepared statement

        /// <summary>
        /// Executes the prepared statement
        /// </summary>
        /// <returns>Number of affected rows</returns>
        public int Execute()
        {
            return Command.Execute();
        }


        /// <summary>
        /// Executes the prepared statement
        /// </summary>
        /// <returns>Number of affected rows</returns>
        public async Task<int> ExecuteAsync()
        {
            return await Command.ExecuteAsync().ConfigureAwait(false);
        }

        #endregion

        #region Close command

        /// <summary>
        /// Closes the database command
        /// </summary>
        private void Close()
        {
            // DBSqlServerCommand.Dispose() does not throw exceptions
            Command?.Dispose();

            Command = null;
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
