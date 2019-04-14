using System;
using System.Collections.Generic;
using System.Data;
using WDNUtils.Common;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Oracle metadata retrieval BLL
    /// </summary>
    public static class DBOracleMetadataBLL
    {
        #region Get database date/time

        /// <summary>
        /// Get the current database date/time
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The current database date/time</returns>
        public static DateTime GetDateTime(DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetDateTime();
            }
        }

        #endregion

        #region Get length of a column

        /// <summary>
        /// Get the length of a database column
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <param name="useCache">Use column length cache (default is true, use false only to detect changes in the database structure)</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The length of the database column; null if the owner, the table or the column does not exist</returns>
        public static int? GetColumnLength(string owner, string tableName, string columnName, bool useCache = true, DBOracleConnection connection = null, string connectionStringName = null)
        {
            if (!useCache)
            {
                using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
                {
                    return dbOracleMedatadataDAL.GetColumnLength(owner: owner, tableName: tableName, columnName: columnName);
                }
            }

            owner = owner.ToUpper();

            if (!GetColumnLength_Cache.TryGetValue(owner, out var ownerCache))
            {
                using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
                {
                    ownerCache = dbOracleMedatadataDAL.GetColumnLengthTable(owner);
                }

                lock (GetColumnLength_Lock)
                {
                    if (!GetColumnLength_Cache.TryGetValue(owner, out var ownerCacheAux))
                    {
                        var newCache = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>(GetColumnLength_Cache);

                        newCache[owner] = ownerCache;

                        GetColumnLength_Cache = newCache;
                    }
                }
            }

            return ownerCache.GetOrDefault(tableName.ToUpper())
                ?.GetOrDefault(columnName.ToUpper());
        }

        private static readonly object GetColumnLength_Lock = new object();

        private static Dictionary<string, Dictionary<string, Dictionary<string, int>>> GetColumnLength_Cache = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();

        /// <summary>
        /// Clears the database column length cache (use to force reloading all column lengths after a change in the database structure)
        /// </summary>
        public static void ClearColumnLengthCache()
        {
            GetColumnLength_Cache = new Dictionary<string, Dictionary<string, Dictionary<string, int>>>();
        }

        #endregion

        #region Get object creation script

        /// <summary>
        /// Get the creation script for an object
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="objectName">Object name</param>
        /// <param name="objectType">Object type (see Oracle documentation for valid values)</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The creation script for the specified object</returns>
        public static string GetCreationScript(string owner, string objectName, string objectType, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetCreationScript(owner: owner, objectName: objectName, objectType: objectType);
            }
        }

        /// <summary>
        /// Get the creation script for a sequence
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="sequenceName">Sequence name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The creation script for the specified sequence</returns>
        public static string GetCreationScriptSequence(string owner, string sequenceName, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetCreationScriptSequence(owner: owner, sequenceName: sequenceName);
            }
        }

        /// <summary>
        /// Get the creation script for a table
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="tableName">Table name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The creation script for the specified table</returns>
        public static string GetCreationScriptTable(string owner, string tableName, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetCreationScriptTable(owner: owner, tableName: tableName);
            }
        }

        /// <summary>
        /// Get the creation script for an index
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="indexName">Index name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The creation script for the specified index</returns>
        public static string GetCreationScriptIndex(string owner, string indexName, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetCreationScriptIndex(owner: owner, indexName: indexName);
            }
        }

        #endregion

        #region Get sequence names

        /// <summary>
        /// Get the names of the sequences for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of sequence names for the specified owner</returns>
        public static List<string> GetSequences(string owner, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetSequences(owner: owner);
            }
        }

        #endregion

        #region Get table names

        /// <summary>
        /// Get the names of the tables for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of table names for the specified owner</returns>
        public static List<string> GetTables(string owner, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetTables(owner: owner);
            }
        }

        #endregion

        #region Get index names

        /// <summary>
        /// Get the names of the indexes for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of index names for the specified owner</returns>
        public static Dictionary<string, string> GetIndexes(string owner, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetIndexes(owner: owner);
            }
        }

        #endregion

        #region Get next value from a database sequence

        /// <summary>
        /// Get next value from a database sequence
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="sequenceName">Sequence name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The next value from a database sequence</returns>
        public static decimal GetSequenceNextValue(string owner, string sequenceName, DBOracleConnection connection = null, string connectionStringName = null)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.GetSequenceNextValue(owner: owner, sequenceName: sequenceName);
            }
        }

        #endregion

        #region Methods from DBOracleBaseDAL

        #region Execute command

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public static int Execute(string commandText, DBOracleConnection connection = null, string connectionStringName = null, params DBOracleParameter[] parameters)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.Execute(commandText: commandText, parameters: parameters);
            }
        }

        /// <summary>
        /// Creates and runs a command to execute a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public static int ExecuteSP(string commandText, DBOracleConnection connection = null, string connectionStringName = null, params DBOracleParameter[] parameters)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.ExecuteSP(commandText: commandText, parameters: parameters);
            }
        }

        #endregion

        #region Retrieve DataTable

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a query command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        public static DataTable RetrieveDataTable(string commandText, DBOracleConnection connection = null, string connectionStringName = null, params DBOracleParameter[] parameters)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.RetrieveDataTable(commandText: commandText, parameters: parameters);
            }
        }

        /// <summary>
        /// Creates and runs a command to retrieve a DataTable using a stored procedure
        /// </summary>
        /// <param name="commandText">Stored procedure name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>DataTable with the query results</returns>
        public static DataTable RetrieveDataTableSP(string commandText, DBOracleConnection connection = null, string connectionStringName = null, params DBOracleParameter[] parameters)
        {
            using (var dbOracleMedatadataDAL = new DBOracleMetadataDAL(connection: ref connection, connectionStringName: connectionStringName))
            {
                return dbOracleMedatadataDAL.RetrieveDataTableSP(commandText: commandText, parameters: parameters);
            }
        }

        #endregion

        #endregion
    }
}
