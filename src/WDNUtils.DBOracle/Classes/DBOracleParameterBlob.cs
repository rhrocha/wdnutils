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
        /// <param name="maxSize">Maximum length for the value, may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterBlob(string name, byte[] value, int? maxSize, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Blob, maxSize: maxSize, direction: direction)
        {
            if (maxSize.HasValue)
            {
                if (value?.Length > maxSize)
                    throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidMaxSizeByteArray, name, value.Length, maxSize));
            }
            else
            {
                if ((Parameter.Direction != ParameterDirection.Input) && (Parameter.Direction != ParameterDirection.InputOutput))
                    throw new ArgumentOutOfRangeException(nameof(maxSize), string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidParameterDirectionAutoSize, name));
            }
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public byte[] Value
        {
            get
            {
                var value = GetValue();

                if (value is null)
                {
                    return null;
                }
                else if (value is OracleBinary oracleBinary)
                {
                    return oracleBinary.Value;
                }
                else if (value is OracleBlob oracleBlob)
                {
                    return oracleBlob.Value;
                }
                else if (value is byte[] byteArray)
                {
                    return byteArray;
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(byte[]).GetType().FullName));
            }

            set
            {
                if ((Parameter.Size > 0) && (value?.Length > Parameter.Size))
                    throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidMaxSizeByteArray, Parameter.ParameterName, value.Length, Parameter.Size));

                SetValue(value);
            }
        }
    }
}
