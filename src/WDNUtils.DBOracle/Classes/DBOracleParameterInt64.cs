using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Long integer (64-bit signed) number bind parameter
    /// </summary>
    public sealed class DBOracleParameterInt64 : DBOracleParameter
    {
        /// <summary>
        /// Creates a long bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterInt64(string name, long? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Int64, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public long? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case OracleDecimal oracleDecimal:
                        return checked((long)Math.Round(oracleDecimal.Value));
                    default:
                        switch (Type.GetTypeCode(value.GetType()))
                        {
                            case TypeCode.UInt64:
                                return checked((long)((ulong)value));
                            case TypeCode.Int64:
                                return (long)value;
                            case TypeCode.UInt32:
                                return checked((long)((uint)value));
                            case TypeCode.Int32:
                                return checked((long)((int)value));
                            case TypeCode.UInt16:
                                return checked((long)((ushort)value));
                            case TypeCode.Int16:
                                return checked((long)((short)value));
                            case TypeCode.Byte:
                                return checked((long)((byte)value));
                            case TypeCode.SByte:
                                return checked((long)((sbyte)value));
                            case TypeCode.Decimal:
                                return checked((long)Math.Round((decimal)value));
                            case TypeCode.Single:
                                return checked((long)Math.Round((float)value));
                            case TypeCode.Double:
                                return checked((long)Math.Round((double)value));
                            default:
                                throw new InvalidOperationException(string.Format(
                                    DBOracleLocalizedText.DBOracleParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(long).GetType().FullName));
                        }
                }
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
