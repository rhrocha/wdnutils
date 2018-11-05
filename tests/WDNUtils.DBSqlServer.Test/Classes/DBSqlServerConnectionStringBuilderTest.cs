using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace WDNUtils.DBSqlServer.Test
{
    [TestClass]
    public class DBSqlServerConnectionStringBuilderTest
    {
        [TestMethod]
        public void TestDBSqlServerConnectionStringBuilderProperties()
        {
            var builder = new DBSqlServerConnectionStringBuilder() { ApplicationName = @"WDNUtils Unit Test" };
            Assert.AreEqual(@"Application Name=""WDNUtils Unit Test""", builder.ConnectionString);
            builder.ApplicationName = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { AttachDBFilename = @"test.db" };
            Assert.AreEqual(@"AttachDbFilename=test.db", builder.ConnectionString);
            builder.AttachDBFilename = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { ConnectRetryCount = 1 };
            Assert.AreEqual(@"ConnectRetryCount=1", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { ConnectRetryInterval = 2 };
            Assert.AreEqual(@"ConnectRetryInterval=2", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { ConnectTimeout = 3 };
            Assert.AreEqual(@"Connect Timeout=3", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { CurrentLanguage = @"en-US" };
            Assert.AreEqual(@"Current Language=en-US", builder.ConnectionString);
            builder.CurrentLanguage = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { DataSource = @"192.167.0.1" };
            Assert.AreEqual(@"Data Source=192.167.0.1", builder.ConnectionString);
            builder.DataSource = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Encrypt = true };
            Assert.AreEqual(@"Encrypt=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Enlist = true };
            Assert.AreEqual(@"Enlist=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { FailoverPartner = @"192.167.0.2" };
            Assert.AreEqual(@"Failover Partner=192.167.0.2", builder.ConnectionString);
            builder.FailoverPartner = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { InitialCatalog = @"<database_name>" };
            Assert.AreEqual(@"Initial Catalog=<database_name>", builder.ConnectionString);
            builder.InitialCatalog = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { IntegratedSecurity = true };
            Assert.AreEqual(@"Integrated Security=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { LoadBalanceTimeout = 4 };
            Assert.AreEqual(@"Load Balance Timeout=4", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { MaxPoolSize = 6 };
            Assert.AreEqual(@"Max Pool Size=6", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { MinPoolSize = 5 };
            Assert.AreEqual(@"Min Pool Size=5", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { MultipleActiveResultSets = true };
            Assert.AreEqual(@"MultipleActiveResultSets=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { MultiSubnetFailover = true };
            Assert.AreEqual(@"MultiSubnetFailover=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { PacketSize = 2048 };
            Assert.AreEqual(@"Packet Size=2048", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Password = @"test" };
            Assert.AreEqual(@"Password=test", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Password = @"test'" };
            Assert.AreEqual(@"Password=""test'""", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Password = @"test""" };
            Assert.AreEqual(@"Password='test""'", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Password = @"test""'" };
            Assert.AreEqual(@"Password=""test""""'""", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { PersistSecurityInfo = true };
            Assert.AreEqual(@"Persist Security Info=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Pooling = true };
            Assert.AreEqual(@"Pooling=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { ReadOnly = true };
            Assert.AreEqual(@"ApplicationIntent=ReadOnly", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { Replication = true };
            Assert.AreEqual(@"Replication=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { TransactionBindingExplicit = true };
            Assert.AreEqual(@"Transaction Binding=""Explicit Unbind""", builder.ConnectionString);
            builder.TransactionBindingExplicit = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { TrustServerCertificate = true };
            Assert.AreEqual(@"TrustServerCertificate=True", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { TypeSystemVersion = @"SQL Server 2012" };
            Assert.AreEqual(@"Type System Version=""SQL Server 2012""", builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { UserID = @"<user_id>" };
            Assert.AreEqual(@"User ID=<user_id>", builder.ConnectionString);
            builder.UserID = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { UserInstance = true };
            Assert.AreEqual(@"User Instance=True", builder.ConnectionString);
            builder.UserInstance = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);

            builder = new DBSqlServerConnectionStringBuilder() { WorkstationID = @"localhost" };
            Assert.AreEqual(@"Workstation ID=localhost", builder.ConnectionString);
            builder.WorkstationID = null;
            Assert.AreEqual(string.Empty, builder.ConnectionString);
        }

        [TestMethod]
        public void TestDBSqlServerConnectionStringBuilderAll()
        {
            var builder = new DBSqlServerConnectionStringBuilder()
            {
                ApplicationName = @"WDNUtils Unit Test",
                ReadOnly = true,
                AttachDBFilename = @"test.db",
                ConnectTimeout = 3,
                ConnectRetryCount = 1,
                ConnectRetryInterval = 2,
                CurrentLanguage = @"en-US",
                DataSource = @"192.167.0.1",
                Encrypt = true,
                Enlist = true,
                FailoverPartner = @"192.167.0.2",
                InitialCatalog = @"<database_name>",
                IntegratedSecurity = true,
                LoadBalanceTimeout = 4,
                MaxPoolSize = 6,
                MinPoolSize = 5,
                MultipleActiveResultSets = true,
                MultiSubnetFailover = true,
                PacketSize = 2048,
                Password = @"test""'",
                PersistSecurityInfo = true,
                Pooling = true,
                Replication = true,
                TransactionBindingExplicit = true,
                TrustServerCertificate = true,
                TypeSystemVersion = @"SQL Server 2012",
                UserID = @"<user_id>",
                UserInstance = true,
                WorkstationID = @"localhost"
            };

            var values = builder.ConnectionString.Split(';').OrderBy(item => item).ToList();

            var expected = new[] {
                @"Application Name=""WDNUtils Unit Test""",
                @"ApplicationIntent=ReadOnly",
                @"AttachDbFilename=test.db",
                @"Connect Timeout=3",
                @"ConnectRetryCount=1",
                @"ConnectRetryInterval=2",
                @"Current Language=en-US",
                @"Data Source=192.167.0.1",
                @"Encrypt=True",
                @"Enlist=True",
                @"Failover Partner=192.167.0.2",
                @"Initial Catalog=<database_name>",
                @"Integrated Security=True",
                @"Load Balance Timeout=4",
                @"Max Pool Size=6",
                @"Min Pool Size=5",
                @"MultipleActiveResultSets=True",
                @"MultiSubnetFailover=True",
                @"Packet Size=2048",
                @"Password=""test""""'""",
                @"Persist Security Info=True",
                @"Pooling=True",
                @"Replication=True",
                @"Transaction Binding=""Explicit Unbind""",
                @"TrustServerCertificate=True",
                @"Type System Version=""SQL Server 2012""",
                @"User ID=<user_id>",
                @"User Instance=True",
                @"Workstation ID=localhost"
            };

            Assert.IsTrue(values.SequenceEqual(expected));
        }
    }
}
