using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Year-to-month interval bind parameter
    /// </summary>
    public sealed class DBOracleParameterIntervalYM : DBOracleParameter
    {
        /// <summary>
        /// Creates a year/month interval bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value, in months (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterIntervalYM(string name, long? value, ParameterDirection direction)
            : base(parameterName: name, value: (!value.HasValue) ? (OracleIntervalYM?)null : new OracleIntervalYM(value.Value), type: OracleDbType.IntervalYM, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter, in months
        /// </summary>
        public long? Value
        {
            get
            {
                var value = GetValue();

                if (value is null)
                {
                    return null;
                }
                else if (value is OracleIntervalYM oracleIntervalYM)
                {
                    return oracleIntervalYM.Value;
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(OracleIntervalYM).GetType().FullName));
            }

            set
            {
                SetValue((value.HasValue) ? new OracleIntervalYM(value.Value) : (OracleIntervalYM?)null);
            }
        }
    }
}
