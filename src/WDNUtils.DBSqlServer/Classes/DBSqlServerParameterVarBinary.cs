using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Byte array (VARBINARY) bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterVarBinary : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a VARBINARY bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="maxSize">Maximum length for the value; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterVarBinary(string name, byte[] value, int? maxSize, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlBinary.Null : new SqlBinary(value), type: SqlDbType.VarBinary, maxSize: maxSize, direction: direction)
        {
            if (value?.Length > maxSize)
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_InvalidMaxSizeString, name, value.Length, maxSize)); // TODO Localized text
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public byte[] Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case SqlBinary sqlBinary:
                        return (sqlBinary == SqlBinary.Null) ? null : sqlBinary.Value;
                    case byte[] byteArray:
                        return byteArray;
                    default:
                        throw new InvalidOperationException(string.Format(
                            DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                            Parameter.ParameterName,
                            value.GetType().FullName,
                            typeof(byte[]).GetType().FullName));
                }
            }

            set
            {
                if ((Parameter.Size > 0) && (value?.Length > Parameter.Size))
                    throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_InvalidMaxSizeString, Parameter.ParameterName, value.Length, Parameter.Size)); // TODO Localized text

                SetValue((value is null) ? SqlBinary.Null : new SqlBinary(value));
            }
        }
    }
}
