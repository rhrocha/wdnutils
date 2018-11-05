using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Wrapper class for SqlConnectionStringBuilder
    /// </summary>
    public sealed class DBSqlServerConnectionStringBuilder
    {
        #region Attributes

        private readonly SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the connection string with the settings of this instance
        /// </summary>
        public string ConnectionString => _builder.ConnectionString;

        /// <summary>
        /// Gets or sets the user ID to be used when connecting to SQL Server
        /// </summary>
        public string UserID { get => _builder.UserID; set { if (value == null) { RemoveProperties(@"User ID", @"user", @"uid"); } else { _builder.UserID = value; } } }

        /// <summary>
        /// Gets or sets a string value that indicates the type system the application expects
        /// </summary>
        public string TypeSystemVersion { get => _builder.TypeSystemVersion; set => _builder.TypeSystemVersion = value; }

        /// <summary>
        /// Gets or sets a value that indicates whether the channel will be encrypted while bypassing walking the certificate chain to validate trust
        /// </summary>
        public bool TrustServerCertificate { get => _builder.TrustServerCertificate; set => _builder.TrustServerCertificate = value; }

        /// <summary>
        /// Gets or sets a string value that indicates how the connection maintains its association with an enlisted System.Transactions transaction
        /// </summary>
        public bool? TransactionBindingExplicit
        {
            get => string.Equals(_builder.TransactionBinding, @"Explicit Unbind", StringComparison.OrdinalIgnoreCase) ? true :
                string.Equals(_builder.TransactionBinding, @"Implicit Unbind", StringComparison.OrdinalIgnoreCase) ? (bool?)false :
                null;
            set { if (value == null) { RemoveProperties(@"Transaction Binding"); } else { _builder.TransactionBinding = value.Value ? @"Explicit Unbind" : @"Implicit Unbind"; } }
        }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether replication is supported using the connection
        /// </summary>
        public bool Replication { get => _builder.Replication; set => _builder.Replication = value; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the connection will be pooled or explicitly opened every time that the connection is requested
        /// </summary>
        public bool Pooling { get => _builder.Pooling; set => _builder.Pooling = value; }

        /// <summary>
        /// Gets or sets the password for the SQL Server account
        /// </summary>
        public string Password { get => _builder.Password; set => _builder.Password = value; }

        /// <summary>
        /// Gets or sets the size in bytes of the network packets used to communicate with an instance of SQL Server
        /// </summary>
        public int PacketSize { get => _builder.PacketSize; set => _builder.PacketSize = value; }

        /// <summary>
        /// Gets or sets a Boolean value that enables faster detection of and connection to the active server is enabled for AlwaysOn availability group on different subnets
        /// </summary>
        public bool MultiSubnetFailover { get => _builder.MultiSubnetFailover; set => _builder.MultiSubnetFailover = value; }

        /// <summary>
        /// Gets or sets a Boolean value that enables multiple active result sets (MARS)
        public bool MultipleActiveResultSets { get => _builder.MultipleActiveResultSets; set => _builder.MultipleActiveResultSets = value; }

        /// <summary>
        /// Gets or sets the minimum number of connections allowed in the connection pool for this specific connection string
        /// </summary>
        public int MinPoolSize { get => _builder.MinPoolSize; set => _builder.MinPoolSize = value; }

        /// <summary>
        /// Gets or sets the maximum number of connections allowed in the connection pool for this specific connection string
        /// </summary>
        public int MaxPoolSize { get => _builder.MaxPoolSize; set => _builder.MaxPoolSize = value; }

        /// <summary>
        /// Gets or sets the minimum time, in seconds, for the connection to live in the connection pool before being destroyed
        /// </summary>
        public int LoadBalanceTimeout { get => _builder.LoadBalanceTimeout; set => _builder.LoadBalanceTimeout = value; }

        /// <summary>
        /// Gets or sets a value that indicates whether to redirect the connection from the default SQL Server Express instance to a runtime-initiated instance running under the account of the caller
        /// </summary>
        public bool? UserInstance { get => _builder.UserInstance; set { if (value == null) { RemoveProperties(@"User Instance"); } else { _builder.UserInstance = value.Value; } } }

        /// <summary>
        /// Gets or sets a Boolean value that indicates if security-sensitive information, such as the password, is not returned as part of the connection if the connection is open or has ever been in an open state
        /// </summary>
        public bool PersistSecurityInfo { get => _builder.PersistSecurityInfo; set => _builder.PersistSecurityInfo = value; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true)
        /// </summary>
        public bool IntegratedSecurity { get => _builder.IntegratedSecurity; set => _builder.IntegratedSecurity = value; }

        /// <summary>
        /// Gets or sets the name of the database associated with the connection
        /// </summary>
        public string InitialCatalog { get => _builder.InitialCatalog; set { if (value == null) { RemoveProperties(@"Initial Catalog", @"database"); } else { _builder.InitialCatalog = value; } } }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the application workload type will be read-only or read/write (default is false, for read/write workload)
        /// </summary>
        public bool ReadOnly { get => _builder.ApplicationIntent == ApplicationIntent.ReadOnly; set => _builder.ApplicationIntent = value ? ApplicationIntent.ReadOnly : ApplicationIntent.ReadWrite; }

        /// <summary>
        /// Gets or sets the name of the application associated with the connection string
        /// </summary>
        public string ApplicationName { get => _builder.ApplicationName; set { if (value == null) { RemoveProperties(@"Application Name", @"app"); } else { _builder.ApplicationName = value; } } }

        /// <summary>
        /// Gets or sets the number of reconnections attempted after identifying that there was an idle connection failure
        /// </summary>
        public int ConnectRetryCount { get => _builder.ConnectRetryCount; set => _builder.ConnectRetryCount = value; }

        /// <summary>
        /// Gets or sets the amount of time (in seconds) between each reconnection attempt after identifying that there was an idle connection failure
        /// </summary>
        public int ConnectRetryInterval { get => _builder.ConnectRetryInterval; set => _builder.ConnectRetryInterval = value; }

        /// <summary>
        /// Gets or sets a string that contains the name of the primary data file (includes the full path name of an attachable database)
        /// </summary>
        public string AttachDBFilename { get => _builder.AttachDBFilename; set { if (value == null) { RemoveProperties(@"AttachDBFilename", @"extended properties", @"initial file name"); } else { _builder.AttachDBFilename = value; } } }

        /// <summary>
        /// Gets or sets the SQL Server Language record name
        /// </summary>
        public string CurrentLanguage { get => _builder.CurrentLanguage; set { if (value == null) { RemoveProperties(@"Current Language", @"language"); } else { _builder.CurrentLanguage = value; } } }

        /// <summary>
        /// Gets or sets the name or network address of the instance of SQL Server to connect to
        /// </summary>
        public string DataSource { get => _builder.DataSource; set { if (value == null) { RemoveProperties(@"Data Source", @"server", @"address", @"addr", @"network address"); } else { _builder.DataSource = value; } } }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed
        /// </summary>
        public bool Encrypt { get => _builder.Encrypt; set => _builder.Encrypt = value; }

        /// <summary>
        /// Gets or sets a Boolean value that indicates whether the SQL Server connection pooler automatically enlists the connection in the creation thread's current transaction context
        /// </summary>
        public bool Enlist { get => _builder.Enlist; set => _builder.Enlist = value; }

        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down
        /// </summary>
        public string FailoverPartner { get => _builder.FailoverPartner; set { if (value == null) { RemoveProperties(@"Failover Partner"); } else { _builder.FailoverPartner = value; } } }

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error
        /// </summary>
        public int ConnectTimeout { get => _builder.ConnectTimeout; set => _builder.ConnectTimeout = value; }

        /// <summary>
        /// Gets or sets the name of the workstation connecting to SQL Server
        /// </summary>
        public string WorkstationID { get => _builder.WorkstationID; set { if (value == null) { RemoveProperties(@"Workstation ID", @"wsid"); } else { _builder.WorkstationID = value; } } }

        #endregion

        #region Remove properties from string builder

        /// <summary>
        /// Remove properties from string builder
        /// </summary>
        /// <param name="properties">Properties to be removed from string builder</param>
        private void RemoveProperties(params string[] properties)
        {
            if (!(properties?.Length > 0))
                return;

            var propertyList = new HashSet<string>(properties.Select(item => item.ToUpperInvariant()));

            var keyList = _builder.Keys.OfType<string>().Where(key => propertyList.Contains(key.ToUpperInvariant()));

            foreach (var key in keyList)
            {
                _builder.Remove(key);
            }
        }

        #endregion
    }
}
