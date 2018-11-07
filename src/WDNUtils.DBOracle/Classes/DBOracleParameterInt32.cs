using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Integer (32-bit signed) number bind parameter
    /// </summary>
    public sealed class DBOracleParameterInt32 : DBOracleParameter
    {
        /// <summary>
        /// Creates an int bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterInt32(string name, int? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Int32, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public int? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case OracleDecimal oracleDecimal:
                        return checked((int)Math.Round(oracleDecimal.Value));
                    default:
                        switch (Type.GetTypeCode(value.GetType()))
                        {
                            case TypeCode.UInt64:
                                return checked((int)((ulong)value));
                            case TypeCode.Int64:
                                return checked((int)((long)value));
                            case TypeCode.UInt32:
                                return checked((int)((uint)value));
                            case TypeCode.Int32:
                                return (int)value;
                            case TypeCode.UInt16:
                                return checked((int)((ushort)value));
                            case TypeCode.Int16:
                                return checked((int)((short)value));
                            case TypeCode.Byte:
                                return checked((int)((byte)value));
                            case TypeCode.SByte:
                                return checked((int)((sbyte)value));
                            case TypeCode.Decimal:
                                return checked((int)Math.Round((decimal)value));
                            case TypeCode.Single:
                                return checked((int)Math.Round((float)value));
                            case TypeCode.Double:
                                return checked((int)Math.Round((double)value));
                            default:
                                throw new InvalidOperationException(string.Format(
                                    DBOracleLocalizedText.DBOracleParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(int).GetType().FullName));
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
