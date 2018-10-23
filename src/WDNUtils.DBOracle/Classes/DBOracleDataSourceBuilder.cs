using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Oracle data source string builder
    /// </summary>
    public sealed class DBOracleDataSourceBuilder
    {
        #region Constants

        /// <summary>Protocol IPC (Inter Process Communication)</summary>
        public const string ProtocolIPC = @"IPC";

        /// <summary>Protocol NMP (Named Pipe)</summary>
        public const string ProtocolNMP = @"NMP";

        /// <summary>Protocol SDP (Sockets Direct Protocol)</summary>
        public const string ProtocolSDP = @"SDP";

        /// <summary>Protocol TCP</summary>
        public const string ProtocolTCP = @"TCP";

        /// <summary>Protocol TCPS (TCP with SSL)</summary>
        public const string ProtocolTCPS = @"TCPS";

        #endregion

        #region Attributes

        /// <summary>
        /// List of DESCRIPTION sections of this DESCRIPTION_LIST section
        /// </summary>
        private readonly List<Description> _descriptions = new List<Description>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns this Oracle data source as a string.
        /// </summary>
        /// <returns>Data source string</returns>
        public string DataSourceString => BuildStringValue();

        /// <summary>
        /// Enable or disable connect-time failover for multiple protocol addresses
        /// </summary>
        public bool? Failover { get; set; } = null;

        /// <summary>
        /// Enable or disable client load balancing for multiple protocol addresses
        /// </summary>
        public bool? LoadBalance { get; set; } = null;

        /// <summary>
        /// Enable routing through multiple protocol addresses
        /// </summary>
        public bool? SourceRoute { get; set; } = null;

        #endregion

        #region Add a new DESCRIPTION section

        /// <summary>
        /// Adds a new DESCRIPTION section
        /// </summary>
        /// <returns>New empty DESCRIPTION section</returns>
        public Description AddDescription()
        {
            var description = new Description();

            _descriptions.Add(description);

            return description;
        }

        #endregion

        #region Create data source string

        /// <summary>
        /// Creates the data source string for this DESCRIPTION_LIST
        /// </summary>
        /// <returns>The data source string for this DESCRIPTION_LIST</returns>
        private string BuildStringValue()
        {
            var stringBuilder = new StringBuilder();

            if (_descriptions.Count > 1)
            {
                stringBuilder.Append(@"(DESCRIPTION_LIST=");
            }

            if (Failover.HasValue)
            {
                stringBuilder.Append(@"(FAILOVER=").Append(Failover.Value ? @"ON" : @"OFF").Append(@")");
            }

            if (LoadBalance.HasValue)
            {
                stringBuilder.Append(@"(LOAD_BALANCE=").Append(LoadBalance.Value ? @"ON" : @"OFF").Append(@")");
            }

            if (SourceRoute.HasValue)
            {
                stringBuilder.Append(@"(SOURCE_ROUTE=").Append(SourceRoute.Value ? @"ON" : @"OFF").Append(@")");
            }

            foreach (var description in _descriptions)
            {
                description.BuildStringValue(stringBuilder);
            }

            if (_descriptions.Count > 1)
            {
                stringBuilder.Append(@")");
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region Section DESCRIPTION

        /// <summary>
        /// Section DESCRIPTION
        /// </summary>
        public sealed class Description
        {
            #region Attributes

            /// <summary>
            /// List of ADDRESS_LIST sections of this DESCRIPTION section
            /// </summary>
            private readonly List<AddressList> _addressLists = new List<AddressList>();

            /// <summary>
            /// Oracle settings for CONNECT_DATA section
            /// </summary>
            public readonly ConnectData ConnectData = new ConnectData();

            #endregion

            #region Properties

            /// <summary>
            /// Enable or disable connect-time failover for multiple protocol addresses
            /// </summary>
            public bool? Failover { get; set; } = null;

            /// <summary>
            /// Enable or disable client load balancing for multiple protocol addresses
            /// </summary>
            public bool? LoadBalance { get; set; } = null;

            /// <summary>
            /// Enable routing through multiple protocol addresses
            /// </summary>
            public bool? SourceRoute { get; set; } = null;

            /// <summary>
            /// Enable keepalive feature, allowing the caller to detect a dead remote server
            /// </summary>
            public bool Enable { get; set; } = false;

            /// <summary>
            /// The buffer space in bytes for receive operations of sessions
            /// </summary>
            public int? ReceiveBufferSize { get; set; } = null;

            /// <summary>
            /// The buffer space in bytes for send operations of sessions
            /// </summary>
            public int? SendBufferSize { get; set; } = null;

            /// <summary>
            /// Session data unit in bytes
            /// </summary>
            public int? SessionDataUnit { get; set; } = null;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new DESCRIPTION section
            /// </summary>
            internal Description()
            {
            }

            #endregion

            #region Add new ADDRESS_LIST section

            /// <summary>
            /// Add new ADDRESS_LIST section
            /// </summary>
            /// <returns>New empty ADDRESS_LIST section</returns>
            public AddressList AddAddressList()
            {
                var addressList = new AddressList();

                _addressLists.Add(addressList);

                return addressList;
            }

            #endregion

            #region Create data source string

            /// <summary>
            /// Appends the contents of this DESCRIPTION section in the data source string
            /// </summary>
            /// <param name="stringBuilder">Data source string</param>
            internal void BuildStringValue(StringBuilder stringBuilder)
            {
                stringBuilder.Append(@"(DESCRIPTION=");

                if (Failover.HasValue)
                {
                    stringBuilder.Append(@"(FAILOVER=").Append(Failover.Value ? @"ON" : @"OFF").Append(@")");
                }

                if (LoadBalance.HasValue)
                {
                    stringBuilder.Append(@"(LOAD_BALANCE=").Append(LoadBalance.Value ? @"ON" : @"OFF").Append(@")");
                }

                if (SourceRoute.HasValue)
                {
                    stringBuilder.Append(@"(SOURCE_ROUTE=").Append(SourceRoute.Value ? @"ON" : @"OFF").Append(@")");
                }

                if (Enable)
                {
                    stringBuilder.Append(@"(ENABLE=BROKEN)");
                }

                if (ReceiveBufferSize > 0)
                {
                    stringBuilder.Append(@"(RECV_BUF_SIZE=").Append(ReceiveBufferSize.Value.ToString(NumberFormatInfo.InvariantInfo)).Append(@")");
                }

                if (SendBufferSize > 0)
                {
                    stringBuilder.Append(@"(SEND_BUF_SIZE=").Append(SendBufferSize.Value.ToString(NumberFormatInfo.InvariantInfo)).Append(@")");
                }

                if (SessionDataUnit > 0)
                {
                    stringBuilder.Append(@"(SDU=").Append(SessionDataUnit.Value.ToString(NumberFormatInfo.InvariantInfo)).Append(@")");
                }

                foreach (var addressList in _addressLists)
                {
                    addressList.BuildStringValue(stringBuilder);
                }

                ConnectData.BuildStringValue(stringBuilder);

                stringBuilder.Append(@")");
            }

            #endregion
        }

        #endregion

        #region Section ADDRESS_LIST

        /// <summary>
        /// Section ADDRESS_LIST
        /// </summary>
        public sealed class AddressList
        {
            #region Attributes

            /// <summary>
            /// List of ADDRESS sections of this ADDRESS_LIST
            /// </summary>
            private readonly List<Address> _addresses = new List<Address>();

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new ADDRESS_LIST
            /// </summary>
            internal AddressList()
            {
            }

            #endregion

            #region Properties

            /// <summary>
            /// Enable or disable connect-time failover for multiple protocol addresses
            /// </summary>
            public bool? Failover { get; set; } = null;

            /// <summary>
            /// Enable or disable client load balancing for multiple protocol addresses
            /// </summary>
            public bool? LoadBalance { get; set; } = null;

            /// <summary>
            /// Enable routing through multiple protocol addresses
            /// </summary>
            public bool? SourceRoute { get; set; } = null;

            #endregion

            #region Add new ADDRESS section

            /// <summary>
            /// Add new ADDRESS section
            /// </summary>
            /// <returns>New empty ADDRESS section</returns>
            public Address AddAddress()
            {
                var address = new Address();

                _addresses.Add(address);

                return address;
            }

            #endregion

            #region Create data source string

            /// <summary>
            /// Appends the contents of this ADDRESS_LIST section in the data source string
            /// </summary>
            /// <param name="stringBuilder">Data source string</param>
            internal void BuildStringValue(StringBuilder stringBuilder)
            {
                bool hasAddressList = (_addresses.Count > 1) || (Failover.HasValue) || (LoadBalance.HasValue) || (SourceRoute.HasValue);

                if (hasAddressList)
                {
                    stringBuilder.Append(@"(ADDRESS_LIST=");
                }

                if (Failover.HasValue)
                {
                    stringBuilder.Append(@"(FAILOVER=").Append(Failover.Value ? @"ON" : @"OFF").Append(@")");
                }

                if (LoadBalance.HasValue)
                {
                    stringBuilder.Append(@"(LOAD_BALANCE=").Append(LoadBalance.Value ? @"ON" : @"OFF").Append(@")");
                }

                if (SourceRoute.HasValue)
                {
                    stringBuilder.Append(@"(SOURCE_ROUTE=").Append(SourceRoute.Value ? @"ON" : @"OFF").Append(@")");
                }

                foreach (var address in _addresses)
                {
                    address.BuildStringValue(stringBuilder);
                }

                if (hasAddressList)
                {
                    stringBuilder.Append(@")");
                }
            }

            #endregion
        }

        #endregion

        #region Section ADDRESS

        /// <summary>
        /// Section ADDRESS
        /// </summary>
        public sealed class Address
        {
            #region Properties

            /// <summary>
            /// Protocol type (IPC, NMP, SDP, TCP, TCPS)
            /// </summary>
            public string Protocol { get; set; }

            /// <summary>
            /// Host name for protocols SDP, TCP and TCPS
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// Port number for protocols SDP, TCP and TCPS
            /// </summary>
            public int? Port { get; set; }

            /// <summary>
            /// Key for IPC protocol
            /// </summary>
            public string IpcKey { get; set; }

            /// <summary>
            /// Server for NMP protocol
            /// </summary>
            public string NamedPipeServer { get; set; }

            /// <summary>
            /// Pipe name for NMP protocol
            /// </summary>
            public string NamedPipeName { get; set; }

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new ADDRESS section
            /// </summary>
            internal Address()
            {
            }

            #endregion

            #region Create data source string

            /// <summary>
            /// Appends the contents of this ADDRESS section in the data source string
            /// </summary>
            /// <param name="stringBuilder">Data source string</param>
            internal void BuildStringValue(StringBuilder stringBuilder)
            {
                stringBuilder.Append(@"(ADDRESS=");

                if (!string.IsNullOrWhiteSpace(Protocol))
                {
                    stringBuilder.Append(@"(PROTOCOL=").Append(Protocol).Append(@")");

                    var protocol = Protocol?.Trim()?.ToUpper();

                    if (protocol.Equals(ProtocolIPC, StringComparison.Ordinal))
                    {
                        if (!string.IsNullOrWhiteSpace(IpcKey))
                        {
                            stringBuilder.Append(@"(KEY=").Append(IpcKey).Append(@")");
                        }
                    }
                    else if (protocol.Equals(ProtocolNMP, StringComparison.Ordinal))
                    {
                        if (!string.IsNullOrWhiteSpace(NamedPipeServer))
                        {
                            stringBuilder.Append(@"(SERVER=").Append(NamedPipeServer).Append(@")");
                        }

                        if (!string.IsNullOrWhiteSpace(NamedPipeName))
                        {
                            stringBuilder.Append(@"(PIPE=").Append(NamedPipeName).Append(@")");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(Host))
                        {
                            stringBuilder.Append(@"(HOST=").Append(Host).Append(@")");
                        }

                        if (Port > 0)
                        {
                            stringBuilder.Append(@"(PORT=").Append(Port.Value.ToString(NumberFormatInfo.InvariantInfo)).Append(@")");
                        }
                    }
                }

                stringBuilder.Append(@")");
            }

            #endregion
        }

        #endregion

        #region Section CONNECT_DATA

        /// <summary>
        /// Section CONNECT_DATA
        /// </summary>
        public sealed class ConnectData
        {
            #region Properties

            /// <summary>
            /// Oracle connect data failover mode information
            /// </summary>
            public readonly FailoverMode FailoverMode = new FailoverMode();

            /// <summary>
            /// Heterogeneous Services, used to connect to a non-Oracle system
            /// </summary>
            public bool HeterogeneousServices { get; set; } = false;

            /// <summary>
            /// Use shared server processes '(SERVER=SHARED)', otherwise uses '(SERVER=DEDICATED)'
            /// </summary>
            public bool ServerShared { get; set; } = false;

            /// <summary>
            /// Use server-side connection pool '(SERVER=POOLED)', otherwise uses '(SERVER=DEDICATED)'. This property is ignored if ServerShared is true.
            /// </summary>
            public bool ServerPooled { get; set; } = false;

            /// <summary>
            /// Identify the database service to access
            /// </summary>
            public string ServiceName { get; set; } = null;

            /// <summary>
            /// Identify the database instance to access
            /// </summary>
            public string InstanceName { get; set; } = null;

            /// <summary>
            /// Identify the database instance to access by its Oracle System Identifier (SID)
            /// </summary>
            public string SID { get; set; } = null;

            /// <summary>
            /// Specify the distinguished name (DN) of the database server
            /// </summary>
            public string Security_DistinguishedName { get; set; } = null;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new CONNECT_DATA section
            /// </summary>
            internal ConnectData()
            {
            }

            #endregion

            #region Create data source string

            /// <summary>
            /// Appends the contents of this CONNECT_DATA section in the data source string
            /// </summary>
            /// <param name="stringBuilder">Data source string</param>
            internal void BuildStringValue(StringBuilder stringBuilder)
            {
                stringBuilder.Append(@"(CONNECT_DATA=");

                FailoverMode.BuildStringValue(stringBuilder);

                if (HeterogeneousServices)
                {
                    stringBuilder.Append(@"(HS=OK)");
                }

                if (ServerShared)
                {
                    stringBuilder.Append(@"(SERVER=SHARED)");
                }
                else if (ServerPooled)
                {
                    stringBuilder.Append(@"(SERVER=POOLED)");
                }
                else
                {
                    stringBuilder.Append(@"(SERVER=DEDICATED)");
                }

                if (!string.IsNullOrWhiteSpace(ServiceName))
                {
                    stringBuilder.Append(@"(SERVICE_NAME=").Append(ServiceName).Append(@")");
                }

                if (!string.IsNullOrWhiteSpace(InstanceName))
                {
                    stringBuilder.Append(@"(INSTANCE_NAME=").Append(InstanceName).Append(@")");
                }

                if (!string.IsNullOrWhiteSpace(SID))
                {
                    stringBuilder.Append(@"(SID=").Append(SID).Append(@")");
                }

                if (!string.IsNullOrWhiteSpace(Security_DistinguishedName))
                {
                    stringBuilder.Append(@"(SECURITY=(SSL_SERVER_CERT_DN=").Append(Security_DistinguishedName).Append(@"))");
                }

                stringBuilder.Append(@")");
            }

            #endregion
        }

        #endregion

        #region Section FAILOVER_NODE

        /// <summary>
        /// Section FAILOVER_NODE
        /// </summary>
        public sealed class FailoverMode
        {
            #region Properties

            /// <summary>
            /// Run-time failover node service name
            /// </summary>
            public string Backup { get; set; } = null;

            /// <summary>
            /// Run-time failover type: true for select, false for session, null for none (default)
            /// </summary>
            public bool? Select { get; set; } = null;

            /// <summary>
            /// Run-time failover method: true for preconnect, false for basic, null for default
            /// </summary>
            public bool? Preconnect { get; set; } = null;

            /// <summary>
            /// Number of times to attempt to connect after a failover (default is 5 if only DELAY is specified)
            /// </summary>
            public int? Retries { get; set; } = null;

            /// <summary>
            /// Amount of time in seconds to wait between connect attempts (default is 1 if only RETRIES is specified)
            /// </summary>
            public int? Delay { get; set; } = null;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new FAILOVER_MODE section
            /// </summary>
            internal FailoverMode()
            {
            }

            #endregion

            #region Create data source string

            /// <summary>
            /// Appends the contents of this FAILOVER_MODE section in the data source string
            /// </summary>
            /// <param name="stringBuilder">Data source string</param>
            internal void BuildStringValue(StringBuilder stringBuilder)
            {
                var buffer = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Backup))
                {
                    buffer.Append(@"(BACKUP=").Append(Backup).Append(@")");
                }

                if (Select.HasValue)
                {
                    buffer.Append(@"(TYPE=").Append(Select.Value ? @"SELECT" : @"SESSION").Append(@")");
                }

                if (Preconnect.HasValue)
                {
                    buffer.Append(@"(METHOD=").Append(Preconnect.Value ? @"PRECONNECT" : @"BASIC").Append(@")");
                }

                if (Retries > 0)
                {
                    buffer.Append(@"(RETRIES=").Append(Retries.Value.ToString(NumberFormatInfo.InvariantInfo)).Append(@")");
                }

                if (Delay > 0)
                {
                    buffer.Append(@"(DELAY=").Append(Delay.Value.ToString(NumberFormatInfo.InvariantInfo)).Append(@")");
                }

                if (buffer.Length > 0)
                {
                    stringBuilder.Append(@"(FAILOVER_MODE=").Append(buffer.ToString()).Append(@")");
                }
            }

            #endregion
        }

        #endregion
    }
}
