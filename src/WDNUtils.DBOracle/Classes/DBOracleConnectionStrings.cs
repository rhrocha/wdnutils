using System;
using System.Collections.Generic;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Connection string collection class
    /// </summary>
    public static class DBOracleConnectionStrings
    {
        #region Attributes

        /// <summary>
        /// Lock object for the connection string list
        /// </summary>
        private readonly static object _lockConnectionStringList = new object();

        /// <summary>
        /// Locked connection string list (used only to add or remove connection strings)
        /// </summary>
        private readonly static Dictionary<string, string> _connectionStringListLocked = new Dictionary<string, string>();

        /// <summary>
        /// Unlocked connection string list (fully replaced when the locked connection list is changed)
        /// </summary>
        private static Dictionary<string, string> _connectionStringListUnlocked = new Dictionary<string, string>();

        #endregion

        #region Adds a connection string

        /// <summary>
        /// Adds a connection string
        /// </summary>
        /// <param name="connectionStringName">Connection string name (case sensitive)</param>
        /// <param name="connectionString">Connection string</param>
        public static void Add(string connectionStringName, string connectionString)
        {
            if (connectionStringName is null)
                throw new ArgumentNullException(nameof(connectionStringName));

            if (connectionString is null)
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(connectionStringName))
                throw new ArgumentOutOfRangeException(nameof(connectionStringName));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentOutOfRangeException(nameof(connectionString));

            lock (_lockConnectionStringList)
            {
                if (_connectionStringListLocked.ContainsKey(connectionStringName))
                    throw new InvalidOperationException();

                _connectionStringListLocked[connectionStringName] = connectionString;

                _connectionStringListUnlocked = new Dictionary<string, string>(_connectionStringListLocked);
            }
        }

        #endregion

        #region Removes a connection string

        /// <summary>
        /// Removes a connection string
        /// </summary>
        /// <param name="connectionStringName">Connection string name (case sensitive)</param>
        /// <returns>True if the connection string was removed, false if the connection string name does not exist in the collection</returns>
        public static bool Remove(string connectionStringName)
        {
            if (connectionStringName is null)
                throw new ArgumentNullException(nameof(connectionStringName));

            lock (_lockConnectionStringList)
            {
                if (!_connectionStringListLocked.Remove(connectionStringName))
                    return false;

                _connectionStringListUnlocked = new Dictionary<string, string>(_connectionStringListLocked);

                return true;
            }
        }

        #endregion

        #region Gets a connection string by name

        /// <summary>
        /// Gets a connection string by name
        /// </summary>
        /// <param name="connectionStringName">Connection string name (case sensitive)</param>
        /// <returns>The connection string with the specified name, or null if the connection string name does not exist in the collection</returns>
        public static string Get(string connectionStringName)
        {
            if (connectionStringName is null)
                throw new ArgumentNullException(nameof(connectionStringName));

            if (!_connectionStringListUnlocked.TryGetValue(connectionStringName, out string connectionString))
                throw new ArgumentOutOfRangeException(nameof(connectionStringName));

            return connectionString;
        }

        #endregion

        #region Get all connection strings

        /// <summary>
        /// Gets all connection strings
        /// </summary>
        /// <returns>A dictionary with all connection strings</returns>
        public static Dictionary<string, string> GetAll()
        {
            return new Dictionary<string, string>(_connectionStringListUnlocked);
        }

        #endregion
    }
}
