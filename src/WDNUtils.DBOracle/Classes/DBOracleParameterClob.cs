using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Large string bind parameter
    /// </summary>
    public sealed class DBOracleParameterClob : DBOracleParameter
    {
        /// <summary>
        /// Creates a large string bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterClob(string name, string value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Clob, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public string Value
        {
            get
            {
                var value = GetValue();

                if (value is null)
                {
                    return null;
                }
                else if (value is OracleClob oracleClob)
                {
                    return oracleClob.Value;
                }
                else if (value is OracleString oracleString)
                {
                    return oracleString.Value;
                }

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        return (string)value;
                    case TypeCode.Char:
                        return ((char)value).ToString();
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(string).GetType().FullName));
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
