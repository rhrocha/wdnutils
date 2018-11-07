using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// TimeSpan bind parameter
    /// </summary>
    public sealed class DBOracleParameterTimeSpan : DBOracleParameter
    {
        /// <summary>
        /// Creates a TimeSpan bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterTimeSpan(string name, TimeSpan? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.IntervalDS, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public TimeSpan? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case OracleIntervalDS oracleIntervalDS:
                        return oracleIntervalDS.Value;
                    case TimeSpan timeSpan:
                        return timeSpan;
                    default:
                        throw new InvalidOperationException(string.Format(
                            DBOracleLocalizedText.DBOracleParameter_CastError,
                            Parameter.ParameterName,
                            value.GetType().FullName,
                            typeof(TimeSpan).GetType().FullName));
                }
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
