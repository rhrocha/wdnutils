using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WDNUtils.Common;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// SQL Server metadata retrieval BLL
    /// </summary>
    public static class DBSqlServerMetadataBLL
    {
        #region Get database date/time

        /// <summary>
        /// Get the current database date/time
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The current database date/time</returns>
        public static DateTime GetDateTime(DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.GetDateTime();
            }
        }

        /// <summary>
        /// Get the current database date/time
        /// </summary>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The current database date/time</returns>
        public static async Task<DateTime> GetDateTimeAsync(DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.GetDateTimeAsync().ConfigureAwait(false);
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
        public static int? GetColumnLength(string owner, string tableName, string columnName, bool useCache = true, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            if (!useCache)
            {
                using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
                {
                    return dbSqlServerMedatadataDAL.GetColumnLength(owner: owner, tableName: tableName, columnName: columnName);
                }
            }

            owner = owner.ToUpper();

            if (!GetColumnLength_Cache.TryGetValue(owner, out var ownerCache))
            {
                using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
                {
                    ownerCache = dbSqlServerMedatadataDAL.GetColumnLengthTable(owner);
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
        public static async Task<int?> GetColumnLengthAsync(string owner, string tableName, string columnName, bool useCache = true, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            if (!useCache)
            {
                using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
                {
                    return await dbSqlServerMedatadataDAL.GetColumnLengthAsync(owner: owner, tableName: tableName, columnName: columnName).ConfigureAwait(false);
                }
            }

            owner = owner.ToUpper();

            if (!GetColumnLength_Cache.TryGetValue(owner, out var ownerCache))
            {
                using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
                {
                    ownerCache = await dbSqlServerMedatadataDAL.GetColumnLengthTableAsync(owner).ConfigureAwait(false);
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

        #region Get sequence names

        /// <summary>
        /// Get the names of the sequences for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of sequence names for the specified owner</returns>
        public static List<string> GetSequences(string owner, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.GetSequences(owner: owner);
            }
        }

        /// <summary>
        /// Get the names of the sequences for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of sequence names for the specified owner</returns>
        public static async Task<List<string>> GetSequencesAsync(string owner, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.GetSequencesAsync(owner: owner).ConfigureAwait(false);
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
        public static List<string> GetTables(string owner, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.GetTables(owner: owner);
            }
        }

        /// <summary>
        /// Get the names of the tables for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of table names for the specified owner</returns>
        public static async Task<List<string>> GetTablesAsync(string owner, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.GetTablesAsync(owner: owner).ConfigureAwait(false);
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
        public static Dictionary<string, string> GetIndexes(string owner, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.GetIndexes(owner: owner);
            }
        }

        /// <summary>
        /// Get the names of the indexes for a specified owner
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>List of index names for the specified owner</returns>
        public static async Task<Dictionary<string, string>> GetIndexesAsync(string owner, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.GetIndexesAsync(owner: owner).ConfigureAwait(false);
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
        public static decimal GetSequenceNextValue(string owner, string sequenceName, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.GetSequenceNextValue(owner: owner, sequenceName: sequenceName);
            }
        }

        /// <summary>
        /// Get next value from a database sequence
        /// </summary>
        /// <param name="owner">Database owner</param>
        /// <param name="sequenceName">Sequence name</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <returns>The next value from a database sequence</returns>
        public static async Task<decimal> GetSequenceNextValueAsync(string owner, string sequenceName, DBSqlServerConnection connection = null, string connectionStringName = null)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.GetSequenceNextValueAsync(owner: owner, sequenceName: sequenceName).ConfigureAwait(false);
            }
        }

        #endregion

        #region Methods from DBSqlServerBaseDAL

        #region Execute command

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public static int Execute(string commandText, DBSqlServerConnection connection = null, string connectionStringName = null, params DBSqlServerParameter[] parameters)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.Execute(commandText: commandText, parameters: parameters);
            }
        }

        /// <summary>
        /// Creates and runs a command to execute a command text
        /// </summary>
        /// <param name="commandText">Command text</param>
        /// <param name="connection">Database connection (null for a new connection)</param>
        /// <param name="connectionStringName">Connection string name (must not be null if connection is null)</param>
        /// <param name="parameters">Bind parameters</param>
        /// <returns>Number of affected rows</returns>
        public static async Task<int> ExecuteAsync(string commandText, DBSqlServerConnection connection = null, string connectionStringName = null, params DBSqlServerParameter[] parameters)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.ExecuteAsync(commandText: commandText, parameters: parameters).ConfigureAwait(false);
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
        public static int ExecuteSP(string commandText, DBSqlServerConnection connection = null, string connectionStringName = null, params DBSqlServerParameter[] parameters)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.ExecuteSP(commandText: commandText, parameters: parameters);
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
        public static async Task<int> ExecuteSPAsync(string commandText, DBSqlServerConnection connection = null, string connectionStringName = null, params DBSqlServerParameter[] parameters)
        {
            using (var dbSqlServerMedatadataDAL = await DBSqlServerMetadataDAL.CreateAsync(connection: connection, connectionStringName: connectionStringName).ConfigureAwait(false))
            {
                return await dbSqlServerMedatadataDAL.ExecuteSPAsync(commandText: commandText, parameters: parameters).ConfigureAwait(false);
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
        public static DataTable RetrieveDataTable(string commandText, DBSqlServerConnection connection = null, string connectionStringName = null, params DBSqlServerParameter[] parameters)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.RetrieveDataTable(commandText: commandText, parameters: parameters);
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
        public static DataTable RetrieveDataTableSP(string commandText, DBSqlServerConnection connection = null, string connectionStringName = null, params DBSqlServerParameter[] parameters)
        {
            using (var dbSqlServerMedatadataDAL = DBSqlServerMetadataDAL.Create(connection: connection, connectionStringName: connectionStringName))
            {
                return dbSqlServerMedatadataDAL.RetrieveDataTableSP(commandText: commandText, parameters: parameters);
            }
        }

        #endregion

        #endregion
    }
}
