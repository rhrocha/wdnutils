using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Short integer (16-bit signed) number bind parameter
    /// </summary>
    public sealed class DBOracleParameterInt16 : DBOracleParameter
    {
        /// <summary>
        /// Creates a short bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterInt16(string name, short? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Int16, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public short? Value
        {
            get
            {
                var value = GetValue();

                if (value is null)
                {
                    return null;
                }
                else if (value is OracleDecimal oracleDecimal)
                {
                    return checked((short)Math.Round(oracleDecimal.Value));
                }

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.UInt64:
                        return checked((short)((ulong)value));
                    case TypeCode.Int64:
                        return checked((short)((long)value));
                    case TypeCode.UInt32:
                        return checked((short)((uint)value));
                    case TypeCode.Int32:
                        return checked((short)((int)value));
                    case TypeCode.UInt16:
                        return checked((short)((ushort)value));
                    case TypeCode.Int16:
                        return (short)value;
                    case TypeCode.Byte:
                        return checked((short)((byte)value));
                    case TypeCode.SByte:
                        return checked((short)((sbyte)value));
                    case TypeCode.Decimal:
                        return checked((short)Math.Round((decimal)value));
                    case TypeCode.Single:
                        return checked((short)Math.Round((float)value));
                    case TypeCode.Double:
                        return checked((short)Math.Round((double)value));
                    default:
                        break;
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(short).GetType().FullName));
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
