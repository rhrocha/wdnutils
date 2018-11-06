using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Date/time bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterDateTime : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a DateTime bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterDateTime(string name, DateTime? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlDateTime.Null : new SqlDateTime(value.Value), type: SqlDbType.DateTime, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public DateTime? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case SqlDateTime sqlServerDate:
                        return sqlServerDate.Value;
                    case DateTime dateTime:
                        return dateTime;
                    default:
                        throw new InvalidOperationException(string.Format(
                            DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                            Parameter.ParameterName,
                            value.GetType().FullName,
                            typeof(DateTime).GetType().FullName));
                }
            }

            set
            {
                SetValue((value is null) ? SqlDateTime.Null : new SqlDateTime(value.Value));
            }
        }
    }
}
