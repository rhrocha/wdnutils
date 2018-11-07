using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Byte (8-bit signed) number bind parameter
    /// </summary>
    public sealed class DBOracleParameterByte : DBOracleParameter
    {
        /// <summary>
        /// Creates a byte bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterByte(string name, byte? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Byte, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public byte? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case OracleDecimal oracleDecimal:
                        return checked((byte)Math.Round(oracleDecimal.Value));
                    default:
                        switch (Type.GetTypeCode(value.GetType()))
                        {
                            case TypeCode.UInt64:
                                return checked((byte)((ulong)value));
                            case TypeCode.Int64:
                                return checked((byte)((long)value));
                            case TypeCode.UInt32:
                                return checked((byte)((uint)value));
                            case TypeCode.Int32:
                                return checked((byte)((int)value));
                            case TypeCode.UInt16:
                                return checked((byte)((ushort)value));
                            case TypeCode.Int16:
                                return checked((byte)((short)value));
                            case TypeCode.Byte:
                                return (byte)value;
                            case TypeCode.SByte:
                                return checked((byte)((sbyte)value));
                            case TypeCode.Decimal:
                                return checked((byte)Math.Round((decimal)value));
                            case TypeCode.Single:
                                return checked((byte)Math.Round((float)value));
                            case TypeCode.Double:
                                return checked((byte)Math.Round((double)value));
                            default:
                                throw new InvalidOperationException(string.Format(
                                    DBOracleLocalizedText.DBOracleParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(byte).GetType().FullName));
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
