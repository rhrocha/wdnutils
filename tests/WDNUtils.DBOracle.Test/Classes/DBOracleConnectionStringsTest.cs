using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WDNUtils.DBOracle.Test
{
    [TestClass]
    public class DBOracleConnectionStringsTest
    {
        [TestMethod]
        public void TestDBOracleConnectionStrings()
        {
            #region Build connection strings

            var connectionString = new DBOracleConnectionStringBuilder();

            var dataSource = new DBOracleDataSourceBuilder();
            var description = dataSource.AddDescription();
            description.ConnectData.ServiceName = @"XE";
            var address = description.AddAddressList().AddAddress();
            address.Protocol = DBOracleDataSourceBuilder.ProtocolTCP;
            address.Host = @"localhost";
            address.Port = 1521;

            connectionString.DataSource = dataSource.DataSourceString;
            connectionString.Pooling = true;
            var connectionStringWithPooling = connectionString.ConnectionString;

            connectionString.Pooling = false;
            var connectionStringWithoutPooling = connectionString.ConnectionString;

            #endregion

            Assert.AreEqual(0, DBOracleConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");

            DBOracleConnectionStrings.Add(@"DB_POOL", connectionStringWithPooling);

            Assert.AreEqual(1, DBOracleConnectionStrings.GetAll().Count);
            Assert.AreEqual(connectionStringWithPooling, DBOracleConnectionStrings.Get(@"DB_POOL"));
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBOracleConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_POOL", connectionStringWithPooling) }));

            DBOracleConnectionStrings.Add(@"DB_NOPOOL", connectionStringWithoutPooling);

            Assert.AreEqual(2, DBOracleConnectionStrings.GetAll().Count);
            Assert.AreEqual(connectionStringWithPooling, DBOracleConnectionStrings.Get(@"DB_POOL"));
            Assert.AreEqual(connectionStringWithoutPooling, DBOracleConnectionStrings.Get(@"DB_NOPOOL"));
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBOracleConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_NOPOOL", connectionStringWithoutPooling),
                new KeyValuePair<string, string>(@"DB_POOL", connectionStringWithPooling) }));

            Assert.IsTrue(DBOracleConnectionStrings.Remove(@"DB_POOL"));

            Assert.AreEqual(1, DBOracleConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            Assert.AreEqual(connectionStringWithoutPooling, DBOracleConnectionStrings.Get(@"DB_NOPOOL"));
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBOracleConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_NOPOOL", connectionStringWithoutPooling) }));

            Assert.IsFalse(DBOracleConnectionStrings.Remove(@"DB_POOL"));

            Assert.AreEqual(1, DBOracleConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            Assert.AreEqual(connectionStringWithoutPooling, DBOracleConnectionStrings.Get(@"DB_NOPOOL"));
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBOracleConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_NOPOOL", connectionStringWithoutPooling) }));

            Assert.IsTrue(DBOracleConnectionStrings.Remove(@"DB_NOPOOL"));

            Assert.AreEqual(0, DBOracleConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");

            Assert.IsFalse(DBOracleConnectionStrings.Remove(@"DB_NOPOOL"));

            Assert.AreEqual(0, DBOracleConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");
        }

        private void AssertNotExist(string connectionStringName)
        {
            try
            {
                DBOracleConnectionStrings.Get(connectionStringName);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }
        }
    }
}
