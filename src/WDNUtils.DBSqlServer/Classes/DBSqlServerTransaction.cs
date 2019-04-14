using System;
using System.Reflection;
using WDNUtils.Common;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// SQL Server transaction/savepoint wrapper class
    /// </summary>
    public sealed class DBSqlServerTransaction
    {
        #region Logger

        private static readonly CachedProperty<ILogAppender> _log = LogAppenderRepository.GetLogAppender(MethodBase.GetCurrentMethod().DeclaringType);
        private static ILogAppender Log => _log.Value;

        #endregion

        #region Attributes

        /// <summary>
        /// Connection whose transaction or savepoint is handled by this object; null if the transaction or savepoint is
        /// already committed or rolled back, or if it's a dummy wrapper (nested transaction without creating a savepoint)
        /// </summary>
        private DBSqlServerConnectionInstance Connection { get; set; }

        /// <summary>
        /// Name of the savepoint that is handled by this object; null if this class handles a transaction instead of a savepoint, the
        /// savepoint is already committed or rolled back, or if it's a dummy wrapper (nested transaction without creating a savepoint)
        /// </summary>
        private string SavepointName { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new transaction/savepoint wrapper class
        /// </summary>
        /// <param name="connection">Database connection, null if it's a dummy wrapper (nested transaction without creating a savepoint)</param>
        /// <param name="savepointName">Savepoint name, null if it's a transaction instead of a savepoint</param>
        internal DBSqlServerTransaction(DBSqlServerConnectionInstance connection, string savepointName)
        {
            if ((connection is null) && (!(savepointName is null)))
                throw new ArgumentNullException(nameof(connection));

            Connection = connection;
            SavepointName = savepointName;
        }

        #endregion

        #region Commit transaction

        /// <summary>
        /// Commit a transaction
        /// </summary>
        /// <returns>True if the transaction was committed successfully, false if this object contains a savepoint or if there was no transaction to commit</returns>
        public bool Commit()
        {
            // Connection._transactionLock is never null, actually this is checking if Connection is null and retrieving _transactionLock value
            if (!(Connection?._connectionLock is object transactionLock))
                return false;

            lock (transactionLock)
            {
                if (Connection is null)
                    return false;

                try
                {
                    if (Connection.Transaction is null)
                        throw new InvalidOperationException(DBSqlServerLocalizedText.DBSqlServerTransaction_Commit_AlreadyFinished);

                    try
                    {
                        if (SavepointName is null)
                        {
                            Connection.Transaction.Commit();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ConvertSqlServerException(isConnecting: false);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        Log.Error(DBSqlServerLocalizedText.DBSqlServerTransaction_Commit_Error, ex);
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }

                    throw;
                }
                finally
                {
                    if (SavepointName is null)
                    {
                        Connection.Transaction = null;
                    }
                    else
                    {
                        SavepointName = null;
                    }

                    Connection = null;
                }
            }
        }

        #endregion

        #region Rollback transaction or savepoint

        /// <summary>
        /// Rollback transaction or savepoint
        /// </summary>
        /// <param name="throwException">If true, the exceptions will be catched, logged and re-throwed; if false, the exceptions will be catched, logged and returned to the caller without re-throwing, to prevent exception swallowing</param>
        /// <returns>Null if the rollback was performed successfully, false if there was no transaction or savepoint to roll back to</returns>
        public Exception Rollback(bool throwException)
        {
            // Connection._transactionLock is never null, actually this is checking if Connection is null and retrieving _transactionLock value
            if (!(Connection?._connectionLock is object transactionLock))
                return null;

            lock (transactionLock)
            {
                if (Connection is null)
                    return null;

                try
                {
                    if (Connection.Transaction is null)
                        throw new InvalidOperationException((SavepointName is null)
                            ? DBSqlServerLocalizedText.DBSqlServerTransaction_RollbackTransaction_AlreadyFinished
                            : string.Format(DBSqlServerLocalizedText.DBSqlServerTransaction_RollbackSavepoint_AlreadyFinished, SavepointName));

                    try
                    {
                        if (SavepointName is null)
                        {
                            Connection.Transaction.Rollback();
                        }
                        else
                        {
                            Connection.Transaction.Rollback(transactionName: SavepointName);
                        }

                        return null;
                    }
                    catch (Exception ex)
                    {
                        ex.ConvertSqlServerException(isConnecting: false);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (SavepointName is null)
                        {
                            Log.Error(DBSqlServerLocalizedText.DBSqlServerConnection_Rollback_Error, ex);
                        }
                        else
                        {
                            Log.Error(string.Format(DBSqlServerLocalizedText.DBSqlServerTransaction_RollbackSavepoint_Error, SavepointName), ex);
                        }
                    }
                    catch (Exception)
                    {
                        // Nothing to do
                    }

                    if (throwException)
                        throw;

                    return ex;
                }
                finally
                {
                    if (SavepointName is null)
                    {
                        Connection.Transaction = null;
                    }
                    else
                    {
                        SavepointName = null;
                    }

                    Connection = null;
                }
            }
        }

        #endregion
    }
}
