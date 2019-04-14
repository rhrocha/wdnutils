using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// SQL Server metadata retrieval DAL
    /// </summary>
    internal sealed class DBSqlServerMetadataDAL : DBSqlServerBaseDAL
    {
        #region Constants

        /// <summary>
        /// Max length for an SQL Server identifier (tablespace, owner, table, column, index or sequence name)
        /// </summary>
        private const int SqlServerIdentifierMaxLength = 128;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of DBSqlServerMetadataDAL
        /// </summary>
        private DBSqlServerMetadataDAL()
        {
        }

        #endregion

        #region Create instance

        /// <summary>
        /// Creates a new DBSqlServerMetadataDAL, opening a new connection if necessary
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        public new static DBSqlServerMetadataDAL Create(DBSqlServerConnection connection, string connectionStringName)
        {
            return Create<DBSqlServerMetadataDAL>(connection: connection, connectionStringName: connectionStringName);
        }

        #endregion

        #region Create instance (async)

        /// <summary>
        /// Creates a new DBSqlServerMetadataDAL, opening a new connection if necessary
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        public new static async Task<DBSqlServerMetadataDAL> CreateAsync(DBSqlServerConnection connection, string connectionStringName)
        {
            return await CreateAsync<DBSqlServerMetadataDAL>(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false);
        }

        #endregion

        #region Get database date/time

        /// <summary>
        /// Get the current database date/time
        /// </summary>
        /// <returns>The current database date/time</returns>
        public DateTime GetDateTime()
        {
            if (GetDateTime_CommandText is null)
            {
                GetDateTime_CommandText = TrimCommandText(@"
                    SELECT
                        GETDATE()");
            }

            return RetrieveDataItem(
                dataFiller: dr => dr.GetDateTime(0).Value,
                commandText: GetDateTime_CommandText);
        }

        /// <summary>
        /// Get the current database date/time
        /// </summary>
        /// <returns>The current database date/time</returns>
        public async Task<DateTime> GetDateTimeAsync()
        {
            if (GetDateTime_CommandText is null)
            {
                GetDateTime_CommandText = TrimCommandText(@"
                    SELECT
                        GETDATE()");
            }

            return await RetrieveDataItemAsync(
                dataFiller: dr => dr.GetDateTime(0).Value,
                commandText: GetDateTime_CommandText).ConfigureAwait(false);
        }

        private static string GetDateTime_CommandText = null;

        #endregion

        #region Get length of a column

        /// <summary>
        /// Get the length of a database column
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The length of the database column; null if the owner, the table or the column does not exist</returns>
        public int? GetColumnLength(string owner, string tableName, string columnName)
        {
            if (GetColumnLength_CommandText is null)
            {
                GetColumnLength_CommandText = TrimCommandText($@"
                    SELECT
                        COALESCE(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, IIF(DATA_TYPE = 'bit', 1, IIF(DATA_TYPE = 'uniqueidentifier', 36, -1)))
                    FROM
                        INFORMATION_SCHEMA.COLUMNS
                    WHERE
                        UPPER(TABLE_SCHEMA) = @{nameof(owner)}
                        AND UPPER(TABLE_NAME) = @{nameof(tableName)}
                        AND UPPER(COLUMN_NAME) = @{nameof(columnName)}");
            }

            return RetrieveDataItem(
                dataFiller: dr => dr.GetInt32(0),
                commandText: GetColumnLength_CommandText,
                parameters: new[]{
                    ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength),
                    ParamString(nameof(tableName), tableName.ToUpper(), SqlServerIdentifierMaxLength),
                    ParamString(nameof(columnName), columnName.ToUpper(), SqlServerIdentifierMaxLength),
                });
        }

        /// <summary>
        /// Get the length of a database column
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <returns>The length of the database column; null if the owner, the table or the column does not exist</returns>
        public async Task<int?> GetColumnLengthAsync(string owner, string tableName, string columnName)
        {
            if (GetColumnLength_CommandText is null)
            {
                GetColumnLength_CommandText = TrimCommandText($@"
                    SELECT
                        COALESCE(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, IIF(DATA_TYPE = 'bit', 1, IIF(DATA_TYPE = 'uniqueidentifier', 36, -1)))
                    FROM
                        INFORMATION_SCHEMA.COLUMNS
                    WHERE
                        UPPER(TABLE_SCHEMA) = @{nameof(owner)}
                        AND UPPER(TABLE_NAME) = @{nameof(tableName)}
                        AND UPPER(COLUMN_NAME) = @{nameof(columnName)}");
            }

            return await RetrieveDataItemAsync(
                dataFiller: dr => dr.GetInt32(0),
                commandText: GetColumnLength_CommandText,
                parameters: new[]{
                    ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength),
                    ParamString(nameof(tableName), tableName.ToUpper(), SqlServerIdentifierMaxLength),
                    ParamString(nameof(columnName), columnName.ToUpper(), SqlServerIdentifierMaxLength),
                }).ConfigureAwait(false);
        }

        private static string GetColumnLength_CommandText = null;

        #endregion

        #region Get length of all columns

        /// <summary>
        /// Get the length of all columns for an owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>The length of the all database columns for the specified owner</returns>
        public Dictionary<string, Dictionary<string, int>> GetColumnLengthTable(string owner)
        {
            if (GetColumnLengthTable_CommandText is null)
            {
                GetColumnLengthTable_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(TABLE_NAME),
                        UPPER(COLUMN_NAME),
                        COALESCE(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, IIF(DATA_TYPE = 'bit', 1, IIF(DATA_TYPE = 'uniqueidentifier', 36, -1)))
                    FROM
                        INFORMATION_SCHEMA.COLUMNS
                    WHERE
                        TABLE_SCHEMA = @{nameof(owner)}");
            }

            var result = RetrieveDataList(
                dataFiller: dr => new
                {
                    TableName = dr.GetString(0),
                    ColumnName = dr.GetString(1),
                    ColumnLength = dr.GetInt32(2)
                },
                commandText: GetColumnLengthTable_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength));

            return result
                .Where(item => item.ColumnLength.HasValue)
                .GroupBy(item => item.TableName)
                .Select(group => new
                {
                    TableName = group.Key,
                    ColumnLengthList = group.ToDictionary(item => item.ColumnName, item => item.ColumnLength.Value)
                })
                .ToDictionary(item => item.TableName, item => item.ColumnLengthList);
        }

        /// <summary>
        /// Get the length of all columns for an owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>The length of the all database columns for the specified owner</returns>
        public async Task<Dictionary<string, Dictionary<string, int>>> GetColumnLengthTableAsync(string owner)
        {
            if (GetColumnLengthTable_CommandText is null)
            {
                GetColumnLengthTable_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(TABLE_NAME),
                        UPPER(COLUMN_NAME),
                        COALESCE(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, IIF(DATA_TYPE = 'bit', 1, IIF(DATA_TYPE = 'uniqueidentifier', 36, -1)))
                    FROM
                        INFORMATION_SCHEMA.COLUMNS
                    WHERE
                        TABLE_SCHEMA = @{nameof(owner)}");
            }

            var result = await RetrieveDataListAsync(
                dataFiller: dr => new
                {
                    TableName = dr.GetString(0),
                    ColumnName = dr.GetString(1),
                    ColumnLength = dr.GetInt32(2)
                },
                commandText: GetColumnLengthTable_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength)).ConfigureAwait(false);

            return result
                .Where(item => item.ColumnLength.HasValue)
                .GroupBy(item => item.TableName)
                .Select(group => new
                {
                    TableName = group.Key,
                    ColumnLengthList = group.ToDictionary(item => item.ColumnName, item => item.ColumnLength.Value)
                })
                .ToDictionary(item => item.TableName, item => item.ColumnLengthList);
        }

        private static string GetColumnLengthTable_CommandText = null;

        #endregion

        #region Get sequence names

        /// <summary>
        /// Get the names of the sequences for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>List of sequence names for the specified owner</returns>

        public List<string> GetSequences(string owner)
        {
            if (GetSequences_CommandText is null)
            {
                GetSequences_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(NAME)
                    FROM
                        SYS.SEQUENCES
                    WHERE
                        SCHEMA_ID = SCHEMA_ID(@{nameof(owner)})");
            }

            return RetrieveDataList(
                dataFiller: dr => dr.GetString(0),
                commandText: GetSequences_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength));
        }

        /// <summary>
        /// Get the names of the sequences for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>List of sequence names for the specified owner</returns>

        public async Task<List<string>> GetSequencesAsync(string owner)
        {
            if (GetSequences_CommandText is null)
            {
                GetSequences_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(NAME)
                    FROM
                        SYS.SEQUENCES
                    WHERE
                        SCHEMA_ID = SCHEMA_ID(@{nameof(owner)})");
            }

            return await RetrieveDataListAsync(
                dataFiller: dr => dr.GetString(0),
                commandText: GetSequences_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength)).ConfigureAwait(false);
        }

        private static string GetSequences_CommandText = null;

        #endregion

        #region Get table names

        /// <summary>
        /// Get the names of the tables for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>List of table names for the specified owner</returns>
        public List<string> GetTables(string owner)
        {
            if (GetTables_CommandText is null)
            {
                GetTables_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(TABLE_NAME)
                    FROM
                        INFORMATION_SCHEMA.TABLES
                    WHERE
                        TABLE_SCHEMA = @{nameof(owner)}");
            }

            return RetrieveDataList(
                dataFiller: dr => dr.GetString(0),
                commandText: GetTables_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength));
        }

        /// <summary>
        /// Get the names of the tables for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>List of table names for the specified owner</returns>
        public async Task<List<string>> GetTablesAsync(string owner)
        {
            if (GetTables_CommandText is null)
            {
                GetTables_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(TABLE_NAME)
                    FROM
                        INFORMATION_SCHEMA.TABLES
                    WHERE
                        TABLE_SCHEMA = @{nameof(owner)}");
            }

            return await RetrieveDataListAsync(
                dataFiller: dr => dr.GetString(0),
                commandText: GetTables_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength)).ConfigureAwait(false);
        }

        private static string GetTables_CommandText = null;

        #endregion

        #region Get index names

        /// <summary>
        /// Get the names of the indexes for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>List of index names for the specified owner</returns>
        public Dictionary<string, string> GetIndexes(string owner)
        {
            if (GetIndexes_CommandText is null)
            {
                GetIndexes_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(I.NAME),
                        UPPER(T.NAME)
                    FROM
                        SYS.TABLES T
                    INNER JOIN
                        SYS.INDEXES I
                    ON
                        T.OBJECT_ID = I.OBJECT_ID
                    WHERE
                        T.SCHEMA_ID = SCHEMA_ID(@{nameof(owner)})
                        AND I.NAME IS NOT NULL");
            }

            return RetrieveDataList(
                    dataFiller: dr => new { IndexName = dr.GetString(0), TableName = dr.GetString(1) },
                    commandText: GetIndexes_CommandText,
                    parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength))
                .ToDictionary(item => item.IndexName, item => item.TableName);
        }

        /// <summary>
        /// Get the names of the indexes for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <returns>List of index names for the specified owner</returns>
        public async Task<Dictionary<string, string>> GetIndexesAsync(string owner)
        {
            if (GetIndexes_CommandText is null)
            {
                GetIndexes_CommandText = TrimCommandText($@"
                    SELECT
                        UPPER(I.NAME),
                        UPPER(T.NAME)
                    FROM
                        SYS.TABLES T
                    INNER JOIN
                        SYS.INDEXES I
                    ON
                        T.OBJECT_ID = I.OBJECT_ID
                    WHERE
                        T.SCHEMA_ID = SCHEMA_ID(@{nameof(owner)})
                        AND I.NAME IS NOT NULL");
            }

            return (await RetrieveDataListAsync(
                    dataFiller: dr => new { IndexName = dr.GetString(0), TableName = dr.GetString(1) },
                    commandText: GetIndexes_CommandText,
                    parameters: ParamString(nameof(owner), owner.ToUpper(), SqlServerIdentifierMaxLength)).ConfigureAwait(false))
                .ToDictionary(item => item.IndexName, item => item.TableName);
        }

        private static string GetIndexes_CommandText = null;

        #endregion

        #region Get next value from a database sequence

        /// <summary>
        /// Get next value from a database sequence
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="sequenceName">Sequence name</param>
        /// <returns>The next value from a database sequence</returns>
        public decimal GetSequenceNextValue(string owner, string sequenceName)
        {
            if (GetSequenceNextValue_CommandText is null)
            {
                GetSequenceNextValue_CommandText = TrimCommandText(@"
                    SELECT @result = (NEXT VALUE FOR {0}.{1})");
            }

            var output = ParamInt64(@"result", 0, ParameterDirection.Output);

            Execute(
                commandText: string.Format(GetSequenceNextValue_CommandText, owner, sequenceName),
                parameters: output);

            return output.Value.Value;
        }

        /// <summary>
        /// Get next value from a database sequence
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="sequenceName">Sequence name</param>
        /// <returns>The next value from a database sequence</returns>
        public async Task<decimal> GetSequenceNextValueAsync(string owner, string sequenceName)
        {
            if (GetSequenceNextValue_CommandText is null)
            {
                GetSequenceNextValue_CommandText = TrimCommandText(@"
                    SELECT @result = (NEXT VALUE FOR {0}.{1})");
            }

            var output = ParamInt64(@"result", 0, ParameterDirection.Output);

            await ExecuteAsync(
                commandText: string.Format(GetSequenceNextValue_CommandText, owner, sequenceName),
                parameters: output).ConfigureAwait(false);

            return output.Value.Value;
        }

        private static string GetSequenceNextValue_CommandText = null;

        #endregion

        #region Methods from DBSqlServerBaseDAL

        #region Execute command

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public new int Execute(string commandText, params DBSqlServerParameter[] parameters)
        {
            return base.Execute(commandText: commandText, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public new async Task<int> ExecuteAsync(string commandText, params DBSqlServerParameter[] parameters)
        {
            return await base.ExecuteAsync(commandText: commandText, parameters: parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public new int ExecuteSP(string commandText, params DBSqlServerParameter[] parameters)
        {
            return base.ExecuteSP(commandText: commandText, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public new async Task<int> ExecuteSPAsync(string commandText, params DBSqlServerParameter[] parameters)
        {
            return await base.ExecuteSPAsync(commandText: commandText, parameters: parameters).ConfigureAwait(false);
        }

        #endregion

        #region Retrieve DataTable

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a query command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        public new DataTable RetrieveDataTable(string commandText, params DBSqlServerParameter[] parameters)
        {
            return base.RetrieveDataTable(commandText: commandText, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        public new DataTable RetrieveDataTableSP(string commandText, params DBSqlServerParameter[] parameters)
        {
            return base.RetrieveDataTableSP(commandText: commandText, parameters: parameters);
        }

        #endregion

        #endregion
    }
}
