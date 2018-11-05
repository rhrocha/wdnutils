using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WDNUtils.DBSqlServer.Test
{
    [TestClass]
    public class DBSqlServerConnectionStringsTest
    {
        [TestMethod]
        public void TestDBSqlServerConnectionStrings()
        {
            #region Build connection strings

            var connectionString = new DBSqlServerConnectionStringBuilder();
            connectionString.DataSource = @"localhost";
            connectionString.Pooling = true;
            var connectionStringWithPooling = connectionString.ConnectionString;

            connectionString.Pooling = false;
            var connectionStringWithoutPooling = connectionString.ConnectionString;

            #endregion

            Assert.AreEqual(0, DBSqlServerConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");

            DBSqlServerConnectionStrings.Add(@"DB_POOL", connectionStringWithPooling);

            Assert.AreEqual(1, DBSqlServerConnectionStrings.GetAll().Count);
            Assert.AreEqual(connectionStringWithPooling, DBSqlServerConnectionStrings.Get(@"DB_POOL"));
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBSqlServerConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_POOL", connectionStringWithPooling) }));

            DBSqlServerConnectionStrings.Add(@"DB_NOPOOL", connectionStringWithoutPooling);

            Assert.AreEqual(2, DBSqlServerConnectionStrings.GetAll().Count);
            Assert.AreEqual(connectionStringWithPooling, DBSqlServerConnectionStrings.Get(@"DB_POOL"));
            Assert.AreEqual(connectionStringWithoutPooling, DBSqlServerConnectionStrings.Get(@"DB_NOPOOL"));
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBSqlServerConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_NOPOOL", connectionStringWithoutPooling),
                new KeyValuePair<string, string>(@"DB_POOL", connectionStringWithPooling) }));

            Assert.IsTrue(DBSqlServerConnectionStrings.Remove(@"DB_POOL"));

            Assert.AreEqual(1, DBSqlServerConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            Assert.AreEqual(connectionStringWithoutPooling, DBSqlServerConnectionStrings.Get(@"DB_NOPOOL"));
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBSqlServerConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_NOPOOL", connectionStringWithoutPooling) }));

            Assert.IsFalse(DBSqlServerConnectionStrings.Remove(@"DB_POOL"));

            Assert.AreEqual(1, DBSqlServerConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            Assert.AreEqual(connectionStringWithoutPooling, DBSqlServerConnectionStrings.Get(@"DB_NOPOOL"));
            AssertNotExist(@"DB_TEST");
            Assert.IsTrue(DBSqlServerConnectionStrings.GetAll().OrderBy(item => item.Key).SequenceEqual(new[] {
                new KeyValuePair<string, string>(@"DB_NOPOOL", connectionStringWithoutPooling) }));

            Assert.IsTrue(DBSqlServerConnectionStrings.Remove(@"DB_NOPOOL"));

            Assert.AreEqual(0, DBSqlServerConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");

            Assert.IsFalse(DBSqlServerConnectionStrings.Remove(@"DB_NOPOOL"));

            Assert.AreEqual(0, DBSqlServerConnectionStrings.GetAll().Count);
            AssertNotExist(@"DB_POOL");
            AssertNotExist(@"DB_NOPOOL");
            AssertNotExist(@"DB_TEST");
        }

        private void AssertNotExist(string connectionStringName)
        {
            try
            {
                DBSqlServerConnectionStrings.Get(connectionStringName);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }
        }
    }
}
