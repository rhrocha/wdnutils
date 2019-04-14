using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Oracle metadata retrieval DAL
    /// </summary>
    internal sealed class DBOracleMetadataDAL : DBOracleBaseDAL
    {
        #region Constants

        /// <summary>
        /// Max length for an Oracle identifier (tablespace, owner, table, column, index or sequence name)
        /// </summary>
        private const int OracleIdentifierMaxLength = 30;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of DBOracleMetadataDAL
        /// </summary>
        private DBOracleMetadataDAL()
        {
        }

        #endregion

        #region Create instance

        /// <summary>
        /// Creates a new DBOracleMetadataDAL, opening a new connection if necessary
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        public new static DBOracleMetadataDAL Create(DBOracleConnection connection, string connectionStringName)
        {
            return Create<DBOracleMetadataDAL>(connection: connection, connectionStringName: connectionStringName);
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
                        SYSDATE
                    FROM
                        SYS.DUAL");
            }

            return RetrieveDataItem(
                dataFiller: dr => dr.GetDateTime(0).Value,
                commandText: GetDateTime_CommandText);
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
                        COALESCE(
                            DECODE(DATA_PRECISION, 0, CAST(NULL AS NUMBER), DATA_PRECISION),
                            DECODE(CHAR_LENGTH, 0, CAST(NULL AS NUMBER), CHAR_LENGTH),
                            DATA_LENGTH)
                    FROM
                        SYS.ALL_TAB_COLUMNS
                    WHERE
                        OWNER = :{nameof(owner)}
                        AND TABLE_NAME = :{nameof(tableName)}
                        AND COLUMN_NAME = :{nameof(columnName)}");
            }

            return RetrieveDataItem(
                dataFiller: dr => dr.GetInt32(0),
                commandText: GetColumnLength_CommandText,
                parameters: new[]{
                    ParamString(nameof(owner), owner.ToUpper(), OracleIdentifierMaxLength),
                    ParamString(nameof(tableName), tableName.ToUpper(), OracleIdentifierMaxLength),
                    ParamString(nameof(columnName), columnName.ToUpper(), OracleIdentifierMaxLength),
                });
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
                        COALESCE(
                            DECODE(DATA_PRECISION, 0, CAST(NULL AS NUMBER), DATA_PRECISION),
                            DECODE(CHAR_LENGTH, 0, CAST(NULL AS NUMBER), CHAR_LENGTH),
                            DATA_LENGTH)
                    FROM
                        SYS.ALL_TAB_COLUMNS
                    WHERE
                        OWNER = :{nameof(owner)}");
            }

            var result = RetrieveDataList(
                dataFiller: dr => new
                {
                    TableName = dr.GetString(0),
                    ColumnName = dr.GetString(1),
                    ColumnLength = dr.GetInt32(2)
                },
                commandText: GetColumnLengthTable_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), OracleIdentifierMaxLength));

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

        #region Get object creation script

        /// <summary>
        /// Get the creation script for an object
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="objectName">Object name</param>
        /// <param name="objectType">Object type (see Oracle documentation for valid values)</param>
        /// <returns>The creation script for the specified object</returns>
        public string GetCreationScript(string owner, string objectName, string objectType)
        {
            if (GetCreationScript_CommandText is null)
            {
                GetCreationScript_CommandText = TrimCommandText($@"
                    SELECT
                        DBMS_METADATA.GET_DDL(:{nameof(objectType)}, :{nameof(objectName)}, :{nameof(owner)})
                    FROM
                        SYS.DUAL");
            }

            return RetrieveDataItem(
                dataFiller: dr => dr.GetString(0),
                commandText: GetCreationScript_CommandText,
                nullValue: null,
                parameters: new[] {
                    ParamString(nameof(objectType), objectType.ToUpper(), OracleIdentifierMaxLength),
                    ParamString(nameof(objectName), objectName.ToUpper(), OracleIdentifierMaxLength),
                    ParamString(nameof(owner), owner.ToUpper(), OracleIdentifierMaxLength)
                });
        }

        private static string GetCreationScript_CommandText = null;

        public string GetCreationScriptSequence(string owner, string sequenceName)
        {
            return GetCreationScript(owner: owner.ToUpper(), objectName: sequenceName.ToUpper(), objectType: @"SEQUENCE");
        }

        public string GetCreationScriptTable(string owner, string tableName)
        {
            return GetCreationScript(owner: owner.ToUpper(), objectName: tableName.ToUpper(), objectType: @"TABLE");
        }

        public string GetCreationScriptIndex(string owner, string indexName)
        {
            return GetCreationScript(owner: owner.ToUpper(), objectName: indexName.ToUpper(), objectType: @"INDEX");
        }

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
                        UPPER(SEQUENCE_NAME)
                    FROM
                        SYS.ALL_SEQUENCES
                    WHERE
                        SEQUENCE_OWNER = :{nameof(owner)}");
            }

            return RetrieveDataList(
                dataFiller: dr => dr.GetString(0),
                commandText: GetSequences_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), OracleIdentifierMaxLength));
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
                        SYS.ALL_TABLES
                    WHERE
                        OWNER = :{nameof(owner)}");
            }

            return RetrieveDataList(
                dataFiller: dr => dr.GetString(0),
                commandText: GetTables_CommandText,
                parameters: ParamString(nameof(owner), owner.ToUpper(), OracleIdentifierMaxLength));
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
                        UPPER(INDEX_NAME),
                        UPPER(TABLE_NAME)
                    FROM
                        SYS.ALL_INDEXES
                    WHERE
                        OWNER = :{nameof(owner)}");
            }

            return RetrieveDataList(
                    dataFiller: dr => new { IndexName = dr.GetString(0), TableName = dr.GetString(1) },
                    commandText: GetIndexes_CommandText,
                    parameters: ParamString(nameof(owner), owner.ToUpper(), OracleIdentifierMaxLength))
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
                    SELECT
                        {0}.{1}.NEXTVAL
                    FROM
                        SYS.DUAL");
            }

            return RetrieveDataItem(
                dataFiller: dr => dr.GetDecimal(0).Value,
                commandText: string.Format(GetSequenceNextValue_CommandText, owner, sequenceName));
        }

        private static string GetSequenceNextValue_CommandText = null;

        #endregion

        #region Methods from DBOracleBaseDAL

        #region Execute command

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public new int Execute(string commandText, params DBOracleParameter[] parameters)
        {
            return base.Execute(commandText: commandText, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public new int ExecuteSP(string commandText, params DBOracleParameter[] parameters)
        {
            return base.ExecuteSP(commandText: commandText, parameters: parameters);
        }

        #endregion

        #region Retrieve DataTable

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a query command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        public new DataTable RetrieveDataTable(string commandText, params DBOracleParameter[] parameters)
        {
            return base.RetrieveDataTable(commandText: commandText, parameters: parameters);
        }

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        public new DataTable RetrieveDataTableSP(string commandText, params DBOracleParameter[] parameters)
        {
            return base.RetrieveDataTableSP(commandText: commandText, parameters: parameters);
        }

        #endregion

        #endregion
    }
}
