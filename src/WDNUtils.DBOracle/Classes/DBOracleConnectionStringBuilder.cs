using Oracle.ManagedDataAccess.Client;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Wrapper class for OracleConnectionStringBuilder
    /// </summary>
    public sealed class DBOracleConnectionStringBuilder
    {
        #region Constants

        /// <summary>
        /// SYSDBA privilege
        /// </summary>
        public const string DBAPrivilege_SYSDBA = @"SYSDBA";

        /// <summary>
        /// SYSOPER privilege
        /// </summary>
        public const string DBAPrivilege_SYSOPER = @"SYSOPER";

        #endregion

        #region Attributes

        private readonly OracleConnectionStringBuilder _builder = new OracleConnectionStringBuilder();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the connection string with the settings of this instance
        /// </summary>
        public string ConnectionString => _builder.ConnectionString;

        /// <summary>
        /// Minimum life time (in seconds) of the connection (default is zero)
        /// </summary>
        public int ConnectionLifeTime { get { return _builder.ConnectionLifeTime; } set { _builder.ConnectionLifeTime = value; } }

        /// <summary>
        /// Maximum time (in seconds) to wait for a free connection from the pool (default is 15)
        /// </summary>
        public int ConnectionTimeout { get { return _builder.ConnectionTimeout; } set { _builder.ConnectionTimeout = value; } }

        /// <summary>
        /// Returns an implicit database connection if set to true (default is false).
        /// An implicit database connection can only be obtained from within a .NET stored procedure.
        /// </summary>
        public bool ContextConnection { get { return _builder.ContextConnection; } set { _builder.ContextConnection = value; } }

        /// <summary>
        /// Administrative privileges SYSDBA or SYSOPER (case insensitive, default is an empty string)
        /// </summary>
        public string DBAPrivilege { get { return _builder.DBAPrivilege; } set { _builder.DBAPrivilege = value; } }

        /// <summary>
        /// Oracle Net Services Name, Connect Descriptor, or an easy connect naming that identifies the database to which to connect (default is empty string)
        /// </summary>
        public string DataSource { get { return _builder.DataSource; } set { _builder.DataSource = value; } }

        /// <summary>
        /// Number of connections that are closed when an excessive amount of established connections are unused (default is 1)
        /// </summary>
        public int DecrPoolSize { get { return _builder.DecrPoolSize; } set { _builder.DecrPoolSize = value; } }

        /// <summary>
        /// Controls the enlistment behavior and capabilities of a connection in context of COM+ transactions or System.Transactions; this attribute can be set to 'true', 'false', 'yes', 'no', or 'dynamic' (default is 'true')
        /// </summary>
        public string Enlist { get { return _builder.Enlist; } set { _builder.Enlist = value; } }

        /// <summary>
        /// Enables ODP.NET connection pool to proactively remove connections from the pool when an Oracle database service, service member, or node goes down (default is true)
        /// </summary>
        public bool HAEvents { get { return _builder.HAEvents; } set { _builder.HAEvents = value; } }

        /// <summary>
        /// Number of new connections to be created when all connections in the pool are in use (default is 5)
        /// </summary>
        public int IncrPoolSize { get { return _builder.IncrPoolSize; } set { _builder.IncrPoolSize = value; } }

        /// <summary>
        /// Enables ODP.NET connection pool to balance work requests across Oracle database instances based on the load balancing advisory and service goal (default is true)
        /// </summary>
        public bool LoadBalancing { get { return _builder.LoadBalancing; } set { _builder.LoadBalancing = value; } }

        /// <summary>
        /// Maximum number of connections in a pool (default is 100)
        /// </summary>
        public int MaxPoolSize { get { return _builder.MaxPoolSize; } set { _builder.MaxPoolSize = value; } }

        /// <summary>
        /// Caches metadata information (default is true)
        /// </summary>
        public bool MetadataPooling { get { return _builder.MetadataPooling; } set { _builder.MetadataPooling = value; } }

        /// <summary>
        /// Minimum number of connections in a pool (default is 1)
        /// </summary>
        public int MinPoolSize { get { return _builder.MinPoolSize; } set { _builder.MinPoolSize = value; } }

        /// <summary>
        /// Password for the user specified by User Id (default is an empty string)
        /// </summary>
        public string Password { get { return _builder.Password; } set { _builder.Password = value; } }

        /// <summary>
        /// Retrieval of the password in the connection string (default is false)
        /// </summary>
        public bool PersistSecurityInfo { get { return _builder.PersistSecurityInfo; } set { _builder.PersistSecurityInfo = value; } }

        /// <summary>
        /// Connection pooling (default is true)
        /// </summary>
        public bool Pooling { get { return _builder.Pooling; } set { _builder.Pooling = value; } }

        /// <summary>
        /// Password of the proxy user (default is an empty string)
        /// </summary>
        public string ProxyPassword { get { return _builder.ProxyPassword; } set { _builder.ProxyPassword = value; } }

        /// <summary>
        /// User name of the proxy user (default is an empty string)
        /// </summary>
        public string ProxyUserId { get { return _builder.ProxyUserId; } set { _builder.ProxyUserId = value; } }

        /// <summary>
        /// Enables or disables self-tuning for the connection, to ignore the value of StatementCacheSize (default is true)
        /// </summary>
        public bool SelfTuning { get { return _builder.SelfTuning; } set { _builder.SelfTuning = value; } }

        /// <summary>
        /// Statement cache purged when the connection goes back to the pool (default is false)
        /// </summary>
        public bool StatementCachePurge { get { return _builder.StatementCachePurge; } set { _builder.StatementCachePurge = value; } }

        /// <summary>
        /// Statement cache enabled and cache size set size, that is, the maximum number of statements that can be cached (default is zero)
        /// </summary>
        public int StatementCacheSize { get { return _builder.StatementCacheSize; } set { _builder.StatementCacheSize = value; } }

        /// <summary>
        /// Oracle user name, the case of the value is preserved if it is surrounded by double quotes
        /// </summary>
        public string UserID { get { return _builder.UserID; } set { _builder.UserID = value; } }

        /// <summary>
        /// Validation of connections coming from the pool (default is false)
        /// </summary>
        public bool ValidateConnection { get { return _builder.ValidateConnection; } set { _builder.ValidateConnection = value; } }

        #endregion
    }
}
