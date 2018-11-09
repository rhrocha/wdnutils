using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// String (VARCHAR) bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterString : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a VARCHAR bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="maxSize">Maximum length for the value; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterString(string name, string value, int? maxSize, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlString.Null : new SqlString(value), type: SqlDbType.NVarChar, maxSize: maxSize, direction: direction)
        {
            if (value?.Length > maxSize)
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_InvalidMaxSizeString, name, value.Length, maxSize));
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public string Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case SqlString sqlString:
                        return sqlString.Value;
                    case SqlChars sqlChars:
                        return new string(sqlChars.Value);
                    default:
                        switch (Type.GetTypeCode(value.GetType()))
                        {
                            case TypeCode.String:
                                return (string)value;
                            case TypeCode.Char:
                                return ((char)value).ToString();
                            default:
                                throw new InvalidOperationException(string.Format(
                                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(string).GetType().FullName));
                        }
                }
            }

            set
            {
                if ((Parameter.Size > 0) && (value?.Length > Parameter.Size))
                    throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBSqlServerLocalizedText.DBSqlServerParameter_InvalidMaxSizeString, Parameter.ParameterName, value.Length, Parameter.Size));

                SetValue((value is null) ? SqlString.Null : new SqlString(value));
            }
        }
    }
}
