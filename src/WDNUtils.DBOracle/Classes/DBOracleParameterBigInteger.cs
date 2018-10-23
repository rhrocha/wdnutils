using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Globalization;
using System.Numerics;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// BigInteger (arbitrarily large signed integer) number bind parameter
    /// </summary>
    public sealed class DBOracleParameterBigInteger : DBOracleParameter
    {
        /// <summary>
        /// Creates a BigInteger bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterBigInteger(string name, BigInteger? value, ParameterDirection direction)
            : base(parameterName: name, value: value?.ToString(NumberFormatInfo.InvariantInfo), type: OracleDbType.Decimal, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public BigInteger? Value
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
                    var valueString = oracleDecimal.ToString();

                    return string.IsNullOrWhiteSpace(valueString) ? (BigInteger?)null : BigInteger.Parse(valueString, NumberFormatInfo.InvariantInfo);
                }

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.UInt64:
                        return new BigInteger((ulong)value);
                    case TypeCode.Int64:
                        return new BigInteger((long)value);
                    case TypeCode.UInt32:
                        return new BigInteger((uint)value);
                    case TypeCode.Int32:
                        return new BigInteger((int)value);
                    case TypeCode.UInt16:
                        return new BigInteger((ushort)value);
                    case TypeCode.Int16:
                        return new BigInteger((short)value);
                    case TypeCode.Byte:
                        return new BigInteger((byte)value);
                    case TypeCode.SByte:
                        return new BigInteger((sbyte)value);
                    case TypeCode.Decimal:
                        return new BigInteger((decimal)value);
                    case TypeCode.Single:
                        return new BigInteger((float)value);
                    case TypeCode.Double:
                        return new BigInteger((double)value);
                    case TypeCode.String:
                        return BigInteger.Parse((string)value, NumberFormatInfo.InvariantInfo);
                    default:
                        break;
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(BigInteger).GetType().FullName));
            }

            set
            {
                SetValue(value?.ToString(NumberFormatInfo.InvariantInfo));
            }
        }
    }
}
