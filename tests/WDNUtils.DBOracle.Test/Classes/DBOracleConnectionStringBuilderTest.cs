using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WDNUtils.DBOracle.Test
{
    [TestClass]
    public class DBOracleConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestDBOracleConnectionStringBuilderDataSource()
        {
            var dataSource = new DBOracleDataSourceBuilder();

            var address = dataSource.AddDescription().AddAddressList().AddAddress();

            address.Protocol = DBOracleDataSourceBuilder.ProtocolTCP;
            address.Host = @"localhost";
            address.Port = 1521;

            var builder = new DBOracleConnectionStringBuilder();

            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder.DataSource = dataSource.DataSourceString;

            Assert.AreEqual(@"DATA SOURCE=""(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)))""", builder.ConnectionString);

            builder.DataSource = string.Empty;

            Assert.AreEqual(@"DATA SOURCE=", builder.ConnectionString);
        }

        [TestMethod]
        public void TestDBOracleConnectionStringBuilderProperties()
        {
            var builder = new DBOracleConnectionStringBuilder() { ConnectionLifeTime = 30 };
            Assert.AreEqual(@"CONNECTION LIFETIME=30", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { ConnectionTimeout = 120 };
            Assert.AreEqual(@"CONNECTION TIMEOUT=120", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { Pooling = true };
            Assert.AreEqual(@"POOLING=True", builder.ConnectionString);

            builder.Pooling = false;
            Assert.AreEqual(@"POOLING=False", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { IncrPoolSize = 4 };
            Assert.AreEqual(@"INCR POOL SIZE=4", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { DecrPoolSize = 2 };
            Assert.AreEqual(@"DECR POOL SIZE=2", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { MinPoolSize = 4 };
            Assert.AreEqual(@"MIN POOL SIZE=4", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { MaxPoolSize = 8 };
            Assert.AreEqual(@"MAX POOL SIZE=8", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { ValidateConnection = true };
            Assert.AreEqual(@"VALIDATE CONNECTION=True", builder.ConnectionString);

            builder.ValidateConnection = false;
            Assert.AreEqual(@"VALIDATE CONNECTION=False", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { HAEvents = false };
            Assert.AreEqual(@"HA EVENTS=False", builder.ConnectionString);

            builder.HAEvents = true;
            Assert.AreEqual(@"HA EVENTS=True", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { ContextConnection = true };
            Assert.AreEqual(@"CONTEXT CONNECTION=True", builder.ConnectionString);

            builder.ContextConnection = false;
            Assert.AreEqual(@"CONTEXT CONNECTION=False", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { DBAPrivilege = DBOracleConnectionStringBuilder.DBAPrivilege_SYSDBA };
            Assert.AreEqual(@"DBA PRIVILEGE=SYSDBA", builder.ConnectionString);

            builder.DBAPrivilege = DBOracleConnectionStringBuilder.DBAPrivilege_SYSOPER;
            Assert.AreEqual(@"DBA PRIVILEGE=SYSOPER", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { Enlist = @"true" };
            Assert.AreEqual(@"ENLIST=true", builder.ConnectionString);

            builder.Enlist = @"dynamic";
            Assert.AreEqual(@"ENLIST=dynamic", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { LoadBalancing = false };
            Assert.AreEqual(@"LOAD BALANCING=False", builder.ConnectionString);

            builder.LoadBalancing = true;
            Assert.AreEqual(@"LOAD BALANCING=True", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { MetadataPooling = false };
            Assert.AreEqual(@"METADATA POOLING=False", builder.ConnectionString);

            builder.MetadataPooling = true;
            Assert.AreEqual(@"METADATA POOLING=True", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { SelfTuning = false };
            Assert.AreEqual(@"SELF TUNING=False", builder.ConnectionString);

            builder.SelfTuning = true;
            Assert.AreEqual(@"SELF TUNING=True", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { StatementCachePurge = true };
            Assert.AreEqual(@"STATEMENT CACHE PURGE=True", builder.ConnectionString);

            builder.StatementCachePurge = false;
            Assert.AreEqual(@"STATEMENT CACHE PURGE=False", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder() { StatementCacheSize = 128 };
            Assert.AreEqual(@"STATEMENT CACHE SIZE=128", builder.ConnectionString);

            builder = new DBOracleConnectionStringBuilder()
            {
                UserID = @"<user>",
                Password = @"<password>",
                ProxyUserId = @"<proxy_user>",
                ProxyPassword = @"<proxy_password>"
            };

            var values = builder.ConnectionString.Split(';').OrderBy(item => item).ToList();

            Assert.IsTrue(values.SequenceEqual(new[] {
                @"PASSWORD=<password>",
                @"PROXY PASSWORD=<proxy_password>",
                @"PROXY USER ID=<proxy_user>",
                @"USER ID=<user>" }));

            builder = new DBOracleConnectionStringBuilder() { PersistSecurityInfo = true };
            Assert.AreEqual(@"PERSIST SECURITY INFO=True", builder.ConnectionString);

            builder.PersistSecurityInfo = false;
            Assert.AreEqual(@"PERSIST SECURITY INFO=False", builder.ConnectionString);
        }

        [TestMethod]
        public void TestDBOracleConnectionStringBuilderAll()
        {
            var dataSource = new DBOracleDataSourceBuilder();

            var address = dataSource.AddDescription().AddAddressList().AddAddress();

            address.Protocol = DBOracleDataSourceBuilder.ProtocolTCP;
            address.Host = @"localhost";
            address.Port = 1521;

            var builder = new DBOracleConnectionStringBuilder()
            {
                ConnectionLifeTime = 30,
                ConnectionTimeout = 120,
                Pooling = true,
                IncrPoolSize = 4,
                DecrPoolSize = 2,
                MinPoolSize = 4,
                MaxPoolSize = 8,
                ValidateConnection = true,
                HAEvents = false,
                ContextConnection = true,
                DBAPrivilege = DBOracleConnectionStringBuilder.DBAPrivilege_SYSDBA,
                Enlist = @"dynamic",
                LoadBalancing = true,
                MetadataPooling = false,
                SelfTuning = false,
                StatementCachePurge = true,
                StatementCacheSize = 128,
                UserID = @"<user>",
                Password = @"<password>",
                ProxyUserId = @"<proxy_user>",
                ProxyPassword = @"<proxy_password>",
                PersistSecurityInfo = true,
                DataSource = dataSource.DataSourceString
            };

            var values = builder.ConnectionString.Split(';').OrderBy(item => item).ToList();

            Assert.IsTrue(values.SequenceEqual(new[] {
                @"CONNECTION LIFETIME=30",
                @"CONNECTION TIMEOUT=120",
                @"CONTEXT CONNECTION=True",
                $@"DATA SOURCE=""{dataSource.DataSourceString}""",
                @"DBA PRIVILEGE=SYSDBA",
                @"DECR POOL SIZE=2",
                @"ENLIST=dynamic",
                @"HA EVENTS=False",
                @"INCR POOL SIZE=4",
                @"LOAD BALANCING=True",
                @"MAX POOL SIZE=8",
                @"METADATA POOLING=False",
                @"MIN POOL SIZE=4",
                @"PASSWORD=<password>",
                @"PERSIST SECURITY INFO=True",
                @"POOLING=True",
                @"PROXY PASSWORD=<proxy_password>",
                @"PROXY USER ID=<proxy_user>",
                @"SELF TUNING=False",
                @"STATEMENT CACHE PURGE=True",
                @"STATEMENT CACHE SIZE=128",
                @"USER ID=<user>",
                @"VALIDATE CONNECTION=True"
            }));
        }
    }
}
