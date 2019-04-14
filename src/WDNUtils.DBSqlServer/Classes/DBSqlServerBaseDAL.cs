using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Base class for DAL implementations
    /// </summary>
    public abstract class DBSqlServerBaseDAL : DBSqlServerConnection
    {
        #region Properties

        /// <summary>
        /// List of parameters to dispose when the DBSqlServerBaseDAL is disposed
        /// </summary>
        private DBSqlServerParameter ParameterListHead { get; set; } = null;

        /// <summary>
        /// List of prepared statements to dispose when this object is disposed
        /// </summary>
        private DBSqlServerPreparedStatement PreparedStatementListHead { get; set; } = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of DBSqlServerBaseDAL
        /// </summary>
        protected DBSqlServerBaseDAL()
        {
        }

        #endregion

        #region Add parameters

        /// <summary>
        /// Creates a boolean bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterBoolean ParamBoolean(string name, bool? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterBoolean(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a byte bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterByte ParamByte(string name, byte? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterByte(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a short bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterInt16 ParamInt16(string name, short? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterInt16(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates an int bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterInt32 ParamInt32(string name, int? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterInt32(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a long bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterInt64 ParamInt64(string name, long? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterInt64(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a decimal bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterDecimal ParamDecimal(string name, decimal? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterDecimal(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a BigInteger bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterBigInteger ParamBigInteger(string name, BigInteger? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterBigInteger(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a DateTime bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterDateTime ParamDateTime(string name, DateTime? value, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterDateTime(name: name, value: value, direction: direction));
        }

        /// <summary>
        /// Creates a string bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="maxSize">Maximum length for the value, may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterString ParamString(string name, string value, int? maxSize, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterString(name: name, value: value, maxSize: maxSize, direction: direction));
        }

        /// <summary>
        /// Creates a byte array bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="maxSize">Maximum length for the value, may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterBinary ParamBinary(string name, byte[] value, int? maxSize, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterBinary(name: name, value: value, maxSize: maxSize, direction: direction));
        }

        /// <summary>
        /// Creates a byte array bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="maxSize">Maximum length for the value, may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        protected DBSqlServerParameterVarBinary ParamVarBinary(string name, byte[] value, int? maxSize, ParameterDirection direction = ParameterDirection.Input)
        {
            return AddParameter(new DBSqlServerParameterVarBinary(name: name, value: value, maxSize: maxSize, direction: direction));
        }

        #endregion

        #region Execute query to retrieve multiple rows

        /// <summary>
        /// Creates and runs a command to execute a query command text that returns multiple rows
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned rows into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>List of objects of the return type</returns>
        protected List<T> RetrieveDataList<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.RetrieveDataList(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: false, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a query command text that returns multiple rows
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned rows into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>List of objects of the return type</returns>
        protected async Task<List<T>> RetrieveDataListAsync<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, params DBSqlServerParameter[] parameters)
        {
            return await DBSqlServerCommand.RetrieveDataListAsync(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: false, parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure that returns multiple rows
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned rows into the return type object</param>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>List of objects of the return type</returns>
        protected List<T> RetrieveDataListSP<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.RetrieveDataList(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: true, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure that returns multiple rows
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned rows into the return type object</param>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>List of objects of the return type</returns>
        protected async Task<List<T>> RetrieveDataListSPAsync<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, params DBSqlServerParameter[] parameters)
        {
            return await DBSqlServerCommand.RetrieveDataListAsync(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: true, parameters: parameters).ConfigureAwait(false);
        }

        #endregion

        #region Execute query to retrieve one row

        /// <summary>
        /// Creates and runs a command to execute a query command text that returns a single row
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned row into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="nullValue">Value to be returned if the query command text does not return any row</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Query result, or nullValue if none</returns>
        protected T RetrieveDataItem<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, T nullValue = default(T), params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.RetrieveDataItem(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: false, nullValue: nullValue, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a query command text that returns a single row
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned row into the return type object</param>
        /// <param name="commandText">Command text</param>
        /// <param name="nullValue">Value to be returned if the query command text does not return any row</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Query result, or nullValue if none</returns>
        protected async Task<T> RetrieveDataItemAsync<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, T nullValue = default(T), params DBSqlServerParameter[] parameters)
        {
            return await DBSqlServerCommand.RetrieveDataItemAsync(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: false, nullValue: nullValue, parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure that returns a single row
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned row into the return type object</param>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="nullValue">Value to be returned if the stored procedure does not return any row</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Stored procedure result, or nullValue if none</returns>
        protected T RetrieveDataItemSP<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, T nullValue = default(T), params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.RetrieveDataItem(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: true, nullValue: nullValue, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure that returns a single row
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dataFiller">Function to convert the returned row into the return type object</param>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="nullValue">Value to be returned if the stored procedure does not return any row</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Stored procedure result, or nullValue if none</returns>
        protected async Task<T> RetrieveDataItemSPAsync<T>(Func<DBSqlServerDataReader, T> dataFiller, string commandText, T nullValue = default(T), params DBSqlServerParameter[] parameters)
        {
            return await DBSqlServerCommand.RetrieveDataItemAsync(connection: this, dataFiller: dataFiller, commandText: commandText, isStoredProcedure: true, nullValue: nullValue, parameters: parameters).ConfigureAwait(false);
        }

        #endregion

        #region Execute command

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        protected int Execute(string commandText, params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.ExecuteNonQuery(connection: this, commandText: commandText, isStoredProcedure: false, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        protected async Task<int> ExecuteAsync(string commandText, params DBSqlServerParameter[] parameters)
        {
            return await DBSqlServerCommand.ExecuteNonQueryAsync(connection: this, commandText: commandText, isStoredProcedure: false, parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        protected int ExecuteSP(string commandText, params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.ExecuteNonQuery(connection: this, commandText: commandText, isStoredProcedure: true, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        protected async Task<int> ExecuteSPAsync(string commandText, params DBSqlServerParameter[] parameters)
        {
            return await DBSqlServerCommand.ExecuteNonQueryAsync(connection: this, commandText: commandText, isStoredProcedure: true, parameters: parameters).ConfigureAwait(false);
        }

        #endregion

        #region Create prepared statement

        /// <summary>
        /// Creates a prepared statement for a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>New prepared statement</returns>
        protected DBSqlServerPreparedStatement CreatePreparedStatement(string commandText, params DBSqlServerParameter[] parameters)
        {
            return AddPreparedStatement(new DBSqlServerPreparedStatement(connection: this, commandText: commandText, isStoredProcedure: false, parameters: parameters));
        }

        /// <summary>
        /// Creates a prepared statement for a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>New prepared statement</returns>
        protected DBSqlServerPreparedStatement CreatePreparedStatementSP(string commandText, params DBSqlServerParameter[] parameters)
        {
            return AddPreparedStatement(new DBSqlServerPreparedStatement(connection: this, commandText: commandText, isStoredProcedure: true, parameters: parameters));
        }

        #endregion

        #region Retrieve DataTable

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a query command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        protected DataTable RetrieveDataTable(string commandText, params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.GetDataTable(connection: this, commandText: commandText, isStoredProcedure: false, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        protected DataTable RetrieveDataTableSP(string commandText, params DBSqlServerParameter[] parameters)
        {
            return DBSqlServerCommand.GetDataTable(connection: this, commandText: commandText, isStoredProcedure: true, parameters: parameters);
        }

        #endregion

        #region Add items to disposal linked lists

        /// <summary>
        /// Adds a parameter to the disposal linked list
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="parameter">New parameter</param>
        /// <returns>A reference to the new parameter</returns>
        private T AddParameter<T>(T parameter) where T : DBSqlServerParameter
        {
            try
            {
                // Set new parameter as the head of the linked list
                parameter.ParameterListNext = ParameterListHead;
                ParameterListHead = parameter;

                return parameter;
            }
            catch (Exception)
            {
                parameter?.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Adds a prepared statement to the disposal linked list
        /// </summary>
        /// <param name="preparedStatement">New prepared statement</param>
        /// <returns>A reference to the new prepared statement</returns>
        private DBSqlServerPreparedStatement AddPreparedStatement(DBSqlServerPreparedStatement preparedStatement)
        {
            try
            {
                // Set new prepared statement as the head of the linked list
                preparedStatement.PreparedStatementListNext = PreparedStatementListHead;
                PreparedStatementListHead = preparedStatement;

                return preparedStatement;
            }
            catch (Exception)
            {
                preparedStatement?.Dispose();
                throw;
            }
        }

        #endregion

        #region Trim spaces and line breaks from command text

        /// <summary>
        /// Trim spaces and line breaks from command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <returns>Trimmed command text</returns>
        protected static string TrimCommandText(string commandText)
        {
            return (commandText is null) ? null :
                Regex.Replace(commandText.Trim(' ', '\r', '\n'), @" *[\r\n]+ *", @" ");
        }

        #endregion

        #region Dispose database resources

        /// <summary>
        /// Disposes the parameters and prepared statements, and also the database connection if applicable
        /// </summary>
        internal override void Close()
        {
            while (!(PreparedStatementListHead is null))
            {
                var next = PreparedStatementListHead.PreparedStatementListNext;

                PreparedStatementListHead.PreparedStatementListNext = null;

                // DBSqlServerPreparedCommand.Dispose() does not throw exceptions
                PreparedStatementListHead.Dispose();

                PreparedStatementListHead = next;
            }

            while (!(ParameterListHead is null))
            {
                var next = ParameterListHead.ParameterListNext;

                ParameterListHead.ParameterListNext = null;

                // DBSqlServerParameter.Dispose() does not throw exceptions
                ParameterListHead.Dispose();

                ParameterListHead = next;
            }

            // Dispose the database connection if applicable
            base.Close();
        }

        #endregion
    }
}
