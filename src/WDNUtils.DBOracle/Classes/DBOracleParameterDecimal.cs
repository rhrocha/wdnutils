using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Decimal (128-bit signed fixed point) number bind parameter
    /// </summary>
    public sealed class DBOracleParameterDecimal : DBOracleParameter
    {
        /// <summary>
        /// Creates a decimal bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterDecimal(string name, decimal? value, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Decimal, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public decimal? Value
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
                    return oracleDecimal.Value;
                }

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.UInt64:
                        return checked((decimal)((ulong)value));
                    case TypeCode.Int64:
                        return checked((decimal)((long)value));
                    case TypeCode.UInt32:
                        return checked((decimal)((uint)value));
                    case TypeCode.Int32:
                        return checked((decimal)((int)value));
                    case TypeCode.UInt16:
                        return checked((decimal)((ushort)value));
                    case TypeCode.Int16:
                        return checked((decimal)((short)value));
                    case TypeCode.Byte:
                        return checked((decimal)((byte)value));
                    case TypeCode.SByte:
                        return checked((decimal)((sbyte)value));
                    case TypeCode.Decimal:
                        return (decimal)value;
                    case TypeCode.Single:
                        return checked((decimal)((float)value));
                    case TypeCode.Double:
                        return checked((decimal)((double)value));
                    default:
                        break;
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(decimal).GetType().FullName));
            }

            set
            {
                SetValue(value);
            }
        }
    }
}
