using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WDNUtils.DBOracle.Test
{
    [TestClass]
    public class DBOracleDataSourceBuilderTest
    {
        [TestMethod]
        public void TestDBOracleDataSourceBuilderDescriptionList()
        {
            var builder = new DBOracleDataSourceBuilder();

            var description1 = builder.AddDescription();

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description1.ConnectData.InstanceName = @"<instance1>";

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))", builder.DataSourceString);

            var description2 = builder.AddDescription();

            Assert.AreEqual(@"(DESCRIPTION_LIST=(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED))))", builder.DataSourceString);

            description2.ConnectData.InstanceName = @"<instance2>";

            Assert.AreEqual(@"(DESCRIPTION_LIST=(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.Failover = false;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(FAILOVER=OFF)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.Failover = true;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(FAILOVER=ON)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.LoadBalance = false;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(FAILOVER=ON)(LOAD_BALANCE=OFF)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.LoadBalance = true;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(FAILOVER=ON)(LOAD_BALANCE=ON)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.SourceRoute = false;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(FAILOVER=ON)(LOAD_BALANCE=ON)(SOURCE_ROUTE=OFF)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.SourceRoute = true;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(FAILOVER=ON)(LOAD_BALANCE=ON)(SOURCE_ROUTE=ON)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.Failover = null;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(LOAD_BALANCE=ON)(SOURCE_ROUTE=ON)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.SourceRoute = null;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(LOAD_BALANCE=ON)(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);

            builder.LoadBalance = null;

            Assert.AreEqual(@"(DESCRIPTION_LIST=(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance1>)))(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance2>))))", builder.DataSourceString);
        }

        [TestMethod]
        public void TestDBOracleDataSourceBuilderDescription()
        {
            var builder = new DBOracleDataSourceBuilder();
            var description = builder.AddDescription();

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.Failover = false;

            Assert.AreEqual(@"(DESCRIPTION=(FAILOVER=OFF)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.Failover = true;

            Assert.AreEqual(@"(DESCRIPTION=(FAILOVER=ON)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.LoadBalance = false;

            Assert.AreEqual(@"(DESCRIPTION=(FAILOVER=ON)(LOAD_BALANCE=OFF)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.LoadBalance = true;

            Assert.AreEqual(@"(DESCRIPTION=(FAILOVER=ON)(LOAD_BALANCE=ON)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SourceRoute = false;

            Assert.AreEqual(@"(DESCRIPTION=(FAILOVER=ON)(LOAD_BALANCE=ON)(SOURCE_ROUTE=OFF)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SourceRoute = true;

            Assert.AreEqual(@"(DESCRIPTION=(FAILOVER=ON)(LOAD_BALANCE=ON)(SOURCE_ROUTE=ON)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.Failover = null;

            Assert.AreEqual(@"(DESCRIPTION=(LOAD_BALANCE=ON)(SOURCE_ROUTE=ON)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SourceRoute = null;

            Assert.AreEqual(@"(DESCRIPTION=(LOAD_BALANCE=ON)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.LoadBalance = null;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.Enable = true;

            Assert.AreEqual(@"(DESCRIPTION=(ENABLE=BROKEN)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.ReceiveBufferSize = 128;

            Assert.AreEqual(@"(DESCRIPTION=(ENABLE=BROKEN)(RECV_BUF_SIZE=128)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SendBufferSize = 256;

            Assert.AreEqual(@"(DESCRIPTION=(ENABLE=BROKEN)(RECV_BUF_SIZE=128)(SEND_BUF_SIZE=256)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SessionDataUnit = 512;

            Assert.AreEqual(@"(DESCRIPTION=(ENABLE=BROKEN)(RECV_BUF_SIZE=128)(SEND_BUF_SIZE=256)(SDU=512)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.Enable = false;

            Assert.AreEqual(@"(DESCRIPTION=(RECV_BUF_SIZE=128)(SEND_BUF_SIZE=256)(SDU=512)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.ReceiveBufferSize = null;

            Assert.AreEqual(@"(DESCRIPTION=(SEND_BUF_SIZE=256)(SDU=512)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SendBufferSize = null;

            Assert.AreEqual(@"(DESCRIPTION=(SDU=512)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            description.SessionDataUnit = null;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);
        }

        [TestMethod]
        public void TestDBOracleDataSourceBuilderAddressList()
        {
            var builder = new DBOracleDataSourceBuilder();
            var addressList = builder.AddDescription().AddAddressList();

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            var address1 = addressList.AddAddress();

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address1.Protocol = DBOracleDataSourceBuilder.ProtocolTCP;
            address1.Host = @"<hostname1>";

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            var address2 = addressList.AddAddress();

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address2.Protocol = DBOracleDataSourceBuilder.ProtocolTCPS;
            address2.Host = @"<hostname2>";

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.Failover = false;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(FAILOVER=OFF)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.Failover = true;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(FAILOVER=ON)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.LoadBalance = false;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(FAILOVER=ON)(LOAD_BALANCE=OFF)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.LoadBalance = true;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(FAILOVER=ON)(LOAD_BALANCE=ON)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.SourceRoute = false;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(FAILOVER=ON)(LOAD_BALANCE=ON)(SOURCE_ROUTE=OFF)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.SourceRoute = true;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(FAILOVER=ON)(LOAD_BALANCE=ON)(SOURCE_ROUTE=ON)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.Failover = null;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(LOAD_BALANCE=ON)(SOURCE_ROUTE=ON)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.SourceRoute = null;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(LOAD_BALANCE=ON)(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            addressList.LoadBalance = null;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=<hostname1>))(ADDRESS=(PROTOCOL=TCPS)(HOST=<hostname2>)))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);
        }

        [TestMethod]
        public void TestDBOracleDataSourceBuilderAddress()
        {
            var builder = new DBOracleDataSourceBuilder();
            var address = builder.AddDescription().AddAddressList().AddAddress();

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=)(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.Protocol = DBOracleDataSourceBuilder.ProtocolIPC;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=IPC))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.IpcKey = @"<ipc_key>";

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=IPC)(KEY=<ipc_key>))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.IpcKey = null;
            address.Protocol = DBOracleDataSourceBuilder.ProtocolNMP;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=NMP))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.NamedPipeServer = @"<server>";

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=NMP)(SERVER=<server>))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.NamedPipeName = @"<pipe>";

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=NMP)(SERVER=<server>)(PIPE=<pipe>))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.NamedPipeServer = null;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=NMP)(PIPE=<pipe>))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.NamedPipeName = null;
            address.Protocol = DBOracleDataSourceBuilder.ProtocolTCP;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.Port = 1521;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.Host = @"<host>";

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=<host>)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.Protocol = DBOracleDataSourceBuilder.ProtocolTCPS;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCPS)(HOST=<host>)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            address.Protocol = DBOracleDataSourceBuilder.ProtocolSDP;

            Assert.AreEqual(@"(DESCRIPTION=(ADDRESS=(PROTOCOL=SDP)(HOST=<host>)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);
        }

        [TestMethod]
        public void TestDBOracleDataSourceBuilderConnectData()
        {
            var builder = new DBOracleDataSourceBuilder();
            var connectData = builder.AddDescription().ConnectData;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            connectData.HeterogeneousServices = true;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(HS=OK)(SERVER=DEDICATED)))", builder.DataSourceString);

            connectData.HeterogeneousServices = false;
            connectData.ServerPooled = false;
            connectData.ServerShared = true;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=SHARED)))", builder.DataSourceString);

            connectData.ServerPooled = true;
            connectData.ServerShared = false;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=POOLED)))", builder.DataSourceString);

            connectData.ServerPooled = true;
            connectData.ServerShared = true;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=SHARED)))", builder.DataSourceString);

            connectData.ServerPooled = false;
            connectData.ServerShared = false;
            connectData.ServiceName = @"<service_name>";

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=<service_name>)))", builder.DataSourceString);

            connectData.ServiceName = null;
            connectData.InstanceName = @"<instance_name>";

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(INSTANCE_NAME=<instance_name>)))", builder.DataSourceString);

            connectData.InstanceName = null;
            connectData.SID = @"<sid>";

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(SID=<sid>)))", builder.DataSourceString);

            connectData.Security_DistinguishedName = @"<dn>";

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)(SID=<sid>)(SECURITY=(SSL_SERVER_CERT_DN=<dn>))))", builder.DataSourceString);
        }

        [TestMethod]
        public void TestDBOracleDataSourceBuilderFailoverMode()
        {
            var builder = new DBOracleDataSourceBuilder();
            var failoverMode = builder.AddDescription().ConnectData.FailoverMode;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Backup = @"<backup>";

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Select = false;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(TYPE=SESSION))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Select = true;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(TYPE=SELECT))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Select = null;
            failoverMode.Preconnect = false;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(METHOD=BASIC))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Preconnect = true;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(METHOD=PRECONNECT))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Preconnect = null;
            failoverMode.Retries = 99;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(RETRIES=99))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Delay = 9;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(RETRIES=99)(DELAY=9))(SERVER=DEDICATED)))", builder.DataSourceString);

            failoverMode.Retries = null;

            Assert.AreEqual(@"(DESCRIPTION=(CONNECT_DATA=(FAILOVER_MODE=(BACKUP=<backup>)(DELAY=9))(SERVER=DEDICATED)))", builder.DataSourceString);
        }
    }
}
