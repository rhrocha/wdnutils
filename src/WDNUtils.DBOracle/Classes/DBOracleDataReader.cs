using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Wrapper class for OracleDataReader
    /// </summary>
    public sealed class DBOracleDataReader
    {
        #region Properties

        /// <summary>
        /// Column name dictionary
        /// </summary>
        private Dictionary<string, int> ColumnIndex { get; set; }

        /// <summary>
        /// Oracle data reader
        /// </summary>
        private OracleDataReader OracleDataReader { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new wrapper class for OracleDataReader (used by DBOracleCommand)
        /// </summary>
        /// <param name="oracleDataReader">OracleDataReader instance</param>
        internal DBOracleDataReader(OracleDataReader oracleDataReader)
        {
            ColumnIndex = null;
            OracleDataReader = oracleDataReader;
        }

        #endregion

        #region Cleanup

        /// <summary>
        /// Remove reference to OracleDataReader instance and clears the column name dictionary (used by DBOracleCommand)
        /// </summary>
        internal void Cleanup()
        {
            OracleDataReader = null;

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
                ColumnIndex = new Dictionary<string, int>(OracleDataReader.FieldCount);
            }

            if (ColumnIndex.TryGetValue(columnName, out int index))
                return index;

            try
            {
                index = OracleDataReader.GetOrdinal(columnName);

                ColumnIndex[columnName] = index;

                return index;
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(columnName),
                    message: string.Format(DBOracleLocalizedText.DBOracleDataReader_ColumnNotFound, columnName));
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
            if (OracleDataReader.IsDBNull(index))
                return nullValue;

            var valueString = OracleDataReader.GetOracleDecimal(index).ToString();

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
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetDecimal(index);

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
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetInt64(index);

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
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetInt32(index);

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
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetInt16(index);

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
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetByte(index);

        /// <summary>
        /// Returns the value of a byte column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte? GetByte(string columnName, byte? nullValue = null)
            => GetByte(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a string column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public string GetString(int index, string nullValue = null)
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetString(index);

        /// <summary>
        /// Returns the value of a string column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public string GetString(string columnName, string nullValue = null)
            => GetString(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a large string column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public string GetClob(int index, string nullValue = null)
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetOracleClob(index).Value;

        /// <summary>
        /// Returns the value of a large string column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public string GetClob(string columnName, string nullValue = null)
            => GetClob(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a DateTime column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public DateTime? GetDateTime(int index, DateTime? nullValue = null)
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetDateTime(index);

        /// <summary>
        /// Returns the value of a DateTime column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public DateTime? GetDateTime(string columnName, DateTime? nullValue = null)
            => GetDateTime(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a TimeSpan column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public TimeSpan? GetTimeSpan(int index, TimeSpan? nullValue = null)
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetTimeSpan(index);

        /// <summary>
        /// Returns the value of a TimeSpan column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public TimeSpan? GetTimeSpan(string columnName, TimeSpan? nullValue = null)
            => GetTimeSpan(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a month/year interval column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public long? GetIntervalYM(int index, long? nullValue = null)
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetOracleIntervalYM(index).Value;

        /// <summary>
        /// Returns the value of a month/year interval column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public long? GetIntervalYM(string columnName, long? nullValue = null)
            => GetIntervalYM(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        /// <summary>
        /// Returns the value of a byte array column
        /// </summary>
        /// <param name="index">Column index</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte[] GetBlob(int index, byte[] nullValue = null)
            => (OracleDataReader.IsDBNull(index)) ? nullValue : OracleDataReader.GetOracleBlob(index).Value;

        /// <summary>
        /// Returns the value of a byte array column
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="nullValue">Value to be returned if the column value is null (default is NULL)</param>
        /// <returns>The value of the specified column</returns>
        public byte[] GetBlob(string columnName, byte[] nullValue = null)
            => GetBlob(GetColumnIndex(columnName: columnName), nullValue: nullValue);

        #endregion
    }
}
