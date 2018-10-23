using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Date/time bind parameter
    /// </summary>
    public sealed class DBOracleParameterDateTime : DBOracleParameter
    {
        /// <summary>
        /// Creates a DateTime bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterDateTime(string name, DateTime? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.TimeStamp, maxSize: null, direction: direction)
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

                if (value is null)
                {
                    return null;
                }
                else if (value is OracleDate oracleDate)
                {
                    return oracleDate.Value;
                }
                else if (value is OracleTimeStamp oracleTimeStamp)
                {
                    return oracleTimeStamp.Value;
                }
                else if (value is OracleTimeStampLTZ oracleTimeStampLTZ)
                {
                    return oracleTimeStampLTZ.Value;
                }
                else if (value is OracleTimeStampTZ oracleTimeStampTZ)
                {
                    return oracleTimeStampTZ.Value;
                }
                else if (value is DateTime dateTime)
                {
                    return dateTime;
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(DateTime).GetType().FullName));
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
