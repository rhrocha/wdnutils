using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using WDNUtils.Common;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Wrapper class for SqlServerCommand
    /// </summary>
    internal sealed class DBSqlServerCommand : IDisposable
    {
        #region Logger

        private static CachedProperty<ILog> _log = new CachedProperty<ILog>(() => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
        private static ILog Log => _log.Value;

        #endregion

        #region Properties

        /// <summary>
        /// Database command
        /// </summary>
        private SqlCommand Command { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a database command
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private DBSqlServerCommand(DBSqlServerConnection connection, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            if (connection is null)
                throw new ArgumentNullException(DBSqlServerLocalizedText.DBSqlServerCommand_NullConnection);

            try
            {
                Command = connection.CreateCommand();

                Command.Transaction = connection.Transaction;
                Command.CommandType = (isStoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
                Command.CommandText = commandText;
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Debug(DBSqlServerLocalizedText.DBSqlServerCommand_Error, ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }

                Close();
                throw;
            }

            string lastParameterName = null;

            try
            {
                foreach (DBSqlServerParameter parameter in parameters)
                {
                    if (parameter is null)
                    {
                        try
                        {
                            Log.Error(string.Concat(DBSqlServerLocalizedText.DBSqlServerCommand_NullParameter, Environment.NewLine, Environment.StackTrace));
                        }
                        catch (Exception)
                        {
                            // Nothing to do
                        }

                        continue;
                    }

                    lastParameterName = parameter.Parameter.ParameterName;

                    Command.Parameters.Add(parameter.Parameter);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Debug(string.Format(DBSqlServerLocalizedText.DBSqlServerCommand_BindError, lastParameterName), ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }

                Close();
                throw;
            }
        }

        #endregion

        #region Execute command

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>Number of affected rows</returns>
        internal int Execute()
        {
            try
            {
                return Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Debug(DBSqlServerLocalizedText.DBSqlServerCommand_ExecuteNonQuery_Error, ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }

                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>Number of affected rows</returns>
        internal async Task<int> ExecuteAsync()
        {
            try
            {
                return await Command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Debug(DBSqlServerLocalizedText.DBSqlServerCommand_ExecuteNonQuery_Error, ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }

                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Create prepared statement

        /// <summary>
        /// Creates a new prepared statement (used by DBSqlServerPreparedStatement)
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        internal static DBSqlServerCommand CreatePreparedStatement(DBSqlServerConnection connection, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            DBSqlServerCommand command = null;

            try
            {
                command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters);

                command.Command.Prepare();

                return command;
            }
            catch (Exception ex)
            {
                command?.Close();
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Execute query to retrieve multiple rows

        /// <summary>
        /// Creates and runs a command to execute a command text that returns multiple rows
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="connection">Database connection</param>
        /// <param name="dataFiller">Function to convert the returned rows into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>List of objects of the return type</returns>
        internal static List<T> RetrieveDataList<T>(DBSqlServerConnection connection, Func<DBSqlServerDataReader, T> dataFiller, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    var sqlDataReader = command.Command.ExecuteReader();

                    DBSqlServerDataReader dataReader = null;
                    List<T> rowList = null;

                    try
                    {
                        dataReader = new DBSqlServerDataReader(sqlDataReader);

                        rowList = new List<T>();

                        while (sqlDataReader.Read())
                        {
                            rowList.Add(dataFiller(dataReader));
                        }

                        return rowList;
                    }
                    catch (Exception)
                    {
                        rowList?.Clear(); // This method fills the internal array with zeros to help the gc
                        throw;
                    }
                    finally
                    {
                        dataReader?.Cleanup();

                        try
                        {
                            sqlDataReader?.Close();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_CloseDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }

                        try
                        {
                            sqlDataReader?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_DisposeDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        /// <summary>
        /// Creates and runs a command to execute a command text that returns multiple rows
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="connection">Database connection</param>
        /// <param name="dataFiller">Function to convert the returned rows into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>List of objects of the return type</returns>
        internal async static Task<List<T>> RetrieveDataListAsync<T>(DBSqlServerConnection connection, Func<DBSqlServerDataReader, T> dataFiller, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    var sqlDataReader = await command.Command.ExecuteReaderAsync().ConfigureAwait(false);

                    DBSqlServerDataReader dataReader = null;
                    List<T> rowList = null;

                    try
                    {
                        dataReader = new DBSqlServerDataReader(sqlDataReader);

                        rowList = new List<T>();

                        while (await sqlDataReader.ReadAsync().ConfigureAwait(false))
                        {
                            rowList.Add(dataFiller(dataReader));
                        }

                        return rowList;
                    }
                    catch (Exception)
                    {
                        rowList?.Clear(); // This method fills the internal array with zeros to help the gc
                        throw;
                    }
                    finally
                    {
                        dataReader?.Cleanup();

                        try
                        {
                            sqlDataReader?.Close();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_CloseDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }

                        try
                        {
                            sqlDataReader?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_DisposeDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Execute query to retrieve one row

        /// <summary>
        /// Creates and runs a command to execute a query command text that returns a single row
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="connection">Database connection</param>
        /// <param name="dataFiller">Function to convert the returned row into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="nullValue">Value to be returned if the query command text does not return any row</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Command text result, or nullValue if none</returns>
        internal static T RetrieveDataItem<T>(DBSqlServerConnection connection, Func<DBSqlServerDataReader, T> dataFiller, string commandText, bool isStoredProcedure, T nullValue = default(T), params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    var sqlDataReader = command.Command.ExecuteReader(behavior: CommandBehavior.SingleRow);

                    DBSqlServerDataReader dataReader = null;

                    try
                    {
                        dataReader = new DBSqlServerDataReader(sqlDataReader);

                        return (sqlDataReader.Read())
                            ? dataFiller(dataReader)
                            : nullValue;
                    }
                    finally
                    {
                        dataReader?.Cleanup();

                        try
                        {
                            sqlDataReader?.Close();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_CloseDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }

                        try
                        {
                            sqlDataReader?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_DisposeDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        /// <summary>
        /// Creates and runs a command to execute a query command text that returns a single row
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="connection">Database connection</param>
        /// <param name="dataFiller">Function to convert the returned row into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="nullValue">Value to be returned if the query command text does not return any row</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Command text result, or nullValue if none</returns>
        internal async static Task<T> RetrieveDataItemAsync<T>(DBSqlServerConnection connection, Func<DBSqlServerDataReader, T> dataFiller, string commandText, bool isStoredProcedure, T nullValue = default(T), params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    var sqlDataReader = await command.Command.ExecuteReaderAsync(behavior: CommandBehavior.SingleRow).ConfigureAwait(false);

                    DBSqlServerDataReader dataReader = null;

                    try
                    {
                        dataReader = new DBSqlServerDataReader(sqlDataReader);

                        return (await sqlDataReader.ReadAsync().ConfigureAwait(false))
                            ? dataFiller(dataReader)
                            : nullValue;
                    }
                    finally
                    {
                        dataReader?.Cleanup();

                        try
                        {
                            sqlDataReader?.Close();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_CloseDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }

                        try
                        {
                            sqlDataReader?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_DisposeDataReader, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Execute non-query command

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        internal static int ExecuteNonQuery(DBSqlServerConnection connection, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    return command.Command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        internal async static Task<int> ExecuteNonQueryAsync(DBSqlServerConnection connection, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    return await command.Command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Retrieve DataTable

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a command text
        /// </summary>
        /// <param name="connection">Database connection</param>
        /// <param name="commandText">Command text</param>
        /// <param name="isStoredProcedure">Indicates if the command text is a stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the command text results</returns>
        internal static DataTable GetDataTable(DBSqlServerConnection connection, string commandText, bool isStoredProcedure, params DBSqlServerParameter[] parameters)
        {
            try
            {
                using (var command = new DBSqlServerCommand(connection: connection, commandText: commandText, isStoredProcedure: isStoredProcedure, parameters: parameters))
                {
                    SqlDataAdapter dataAdapter;

                    try
                    {
                        dataAdapter = new SqlDataAdapter(command.Command);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Log.Debug(DBSqlServerLocalizedText.DBSqlServerCommand_GetDataSet_DataAdapterError, ex);
                        }
                        catch (Exception)
                        {
                            // Nothing to do
                        }

                        throw;
                    }

                    bool suppressDisposeException = false;

                    try
                    {
                        var dataTable = new DataTable();

                        dataAdapter.Fill(dataTable);

                        return dataTable;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Log.Debug(DBSqlServerLocalizedText.DBSqlServerCommand_GetDataSet_DataRetrieveError, ex);
                        }
                        catch (Exception)
                        {
                            // Nothing to do
                        }

                        suppressDisposeException = true;
                        throw;
                    }
                    finally
                    {
                        try
                        {
                            dataAdapter.Dispose();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_DataAdapter_Disposal_Error, ex);
                            }
                            catch (Exception)
                            {
                                // Nothing to do
                            }

                            if (!suppressDisposeException)
                                throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ConvertSqlServerException(isConnecting: false);
                throw;
            }
        }

        #endregion

        #region Close Command

        /// <summary>
        /// Closes the database command
        /// </summary>
        public void Close()
        {
            if (Command is null)
                return;

            try
            {
                Command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_ParameterCollectionClear_Error, ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }

            try
            {
                Command.Connection = null;
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_ConnectionClear_Error, ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }

            try
            {
                Command.Dispose();
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(DBSqlServerLocalizedText.DBSqlServerCommand_Disposal_Error, ex);
                }
                catch (Exception)
                {
                    // Nothing to do
                }
            }

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
