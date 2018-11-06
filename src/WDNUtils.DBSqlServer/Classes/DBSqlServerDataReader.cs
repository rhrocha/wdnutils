using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Numerics;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Wrapper class for SqlDataReader
    /// </summary>
    public sealed class DBSqlServerDataReader
    {
        #region Properties

        /// <summary>
        /// Column name dictionary
        /// </summary>
        private Dictionary<string, int> ColumnIndex { get; set; }

        /// <summary>
        /// SqlServer data reader
        /// </summary>
        private SqlDataReader SqlDataReader { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new wrapper class for SqlDataReader (used by DBSqlServerCommand)
        /// </summary>
        /// <param name="sqlDataReader">SqlDataReader instance</param>
        internal DBSqlServerDataReader(SqlDataReader sqlDataReader)
        {
            ColumnIndex = null;
            SqlDataReader = sqlDataReader;
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Remove reference to SqlDataReader instance and clears the column name dictionary (used by DBSqlServerCommand)
        /// </summary>
        internal void Cleanup()
        {
            SqlDataReader = null;

            ColumnIndex?.Clear();  // This method fills the internal array with zeros to help the gc
            ColumnIndex = null;
        }

        #endregion

        #region Get column index

        /// <summary>
        /// Gets the index for a column name
        /// </summary>
        /// <param name="columnName">Column name (case insensitive)</param>
        /// <returns>Index for the specified column name</returns>
        private int GetColumnIndex(string columnName)
        {
            if (ColumnIndex is null)
            {
                ColumnIndex = new Dictionary<string, int>(SqlDataReader.FieldCount);
            }

            if (ColumnIndex.TryGetValue(columnName, out int index))
                return index;

            try
            {
                index = SqlDataReader.GetOrdinal(columnName);

                ColumnIndex[columnName] = index;

                return index;
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(columnName),
                    message: string.Format(DBSqlServerLocalizedText.DBSqlServerDataReader_ColumnNotFound, columnName));
            }
        }

        #endregion

        #region Get values

        /// <summary>
        /// Returns the value of a BigInteger column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public BigInteger? GetBigInteger(int index, BigInteger? nullValue = null)
        {
            if (SqlDataReader.IsDBNull(index))
                return nullValue;

            var valueString = SqlDataReader.GetSqlDecimal(index).ToString();

            return string.IsNullOrWhiteSpace(valueString) ? (BigInteger?)null : BigInteger.Parse(valueString, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Returns the value of a BigInteger column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public BigInteger? GetBigInteger(string columnName, BigInteger? nullValue = null)
            => GetBigInteger(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a decimal column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public decimal? GetDecimal(int index, decimal? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetDecimal(index);

        /// <summary>
        /// Returns the value of a decimal column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public decimal? GetDecimal(string columnName, decimal? nullValue = null)
            => GetDecimal(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a long column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public long? GetInt64(int index, long? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetInt64(index);

        /// <summary>
        /// Returns the value of a long column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public long? GetInt64(string columnName, long? nullValue = null)
            => GetInt64(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a int column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public int? GetInt32(int index, int? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetInt32(index);

        /// <summary>
        /// Returns the value of a int column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public int? GetInt32(string columnName, int? nullValue = null)
            => GetInt32(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a short column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public short? GetInt16(int index, short? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetInt16(index);

        /// <summary>
        /// Returns the value of a short column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public short? GetInt16(string columnName, short? nullValue = null)
            => GetInt16(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a byte column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte? GetByte(int index, byte? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetByte(index);

        /// <summary>
        /// Returns the value of a byte column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte? GetByte(string columnName, byte? nullValue = null)
            => GetByte(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a bit column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public bool? GetBoolean(int index, bool? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetBoolean(index);

        /// <summary>
        /// Returns the value of a bit column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte? GetBoolean(string columnName, byte? nullValue = null)
            => GetByte(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a string column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public string GetString(int index, string nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetString(index);

        /// <summary>
        /// Returns the value of a string column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public string GetString(string columnName, string nullValue = null)
            => GetString(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a DateTime column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public DateTime? GetDateTime(int index, DateTime? nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetDateTime(index);

        /// <summary>
        /// Returns the value of a DateTime column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public DateTime? GetDateTime(string columnName, DateTime? nullValue = null)
            => GetDateTime(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a byte array column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte[] GetBinary(int index, byte[] nullValue = null)
            => (SqlDataReader.IsDBNull(index)) ? nullValue : SqlDataReader.GetSqlBinary(index).Value;

        /// <summary>
        /// Returns the value of a byte array column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte[] GetBinary(string columnName, byte[] nullValue = null)
            => GetBinary(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        #endregion
    }
}
