using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Byte array bind parameter
    /// </summary>
    public sealed class DBOracleParameterBlob : DBOracleParameter
    {
        /// <summary>
        /// Creates a byte array bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterBlob(string name, byte[] value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Blob, maxSize: null, direction: direction)
        {
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
                    case OracleBinary oracleBinary:
                        return oracleBinary.Value;
                    case OracleBlob oracleBlob:
                        return oracleBlob.Value;
                    case byte[] byteArray:
                        return byteArray;
                    default:
                        throw new InvalidOperationException(string.Format(
                            DBOracleLocalizedText.DBOracleParameter_CastError,
                            Parameter.ParameterName,
                            value.GetType().FullName,
                            typeof(byte[]).GetType().FullName));
                }
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
