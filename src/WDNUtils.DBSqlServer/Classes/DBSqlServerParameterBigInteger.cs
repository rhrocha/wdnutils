using System;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Numerics;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// BigInteger (arbitrarily large signed integer) number bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterBigInteger : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a BigInteger bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterBigInteger(string name, BigInteger? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: BigIntegerToSqlDecimal(value), type: SqlDbType.Decimal, maxSize: null, direction: direction)
        {
        }

        private static SqlDecimal BigIntegerToSqlDecimal(BigInteger? value)
        {
            if (value is null)
                return SqlDecimal.Null;

            var valueString = value.Value.ToString(NumberFormatInfo.InvariantInfo);

            if (string.IsNullOrWhiteSpace(valueString))
                return SqlDecimal.Null;

            return SqlDecimal.Parse(valueString);
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public BigInteger? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case SqlDecimal sqlDecimal:
                        var valueString = sqlDecimal.ToString();
                        return string.IsNullOrWhiteSpace(valueString) ? (BigInteger?)null : BigInteger.Parse(valueString, NumberFormatInfo.InvariantInfo);
                    case SqlMoney sqlMoney:
                        return new BigInteger(sqlMoney.Value);
                    case SqlInt64 sqlInt64:
                        return new BigInteger(sqlInt64.Value);
                    case SqlInt32 sqlInt32:
                        return new BigInteger(sqlInt32.Value);
                    case SqlInt16 sqlInt16:
                        return new BigInteger(sqlInt16.Value);
                    case SqlByte sqlByte:
                        return new BigInteger(sqlByte.Value);
                    case SqlDouble sqlDouble:
                        return new BigInteger(sqlDouble.Value);
                    case SqlSingle sqlSingle:
                        return new BigInteger(sqlSingle.Value);
                    default:
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
                                throw new InvalidOperationException(string.Format(
                                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(BigInteger).GetType().FullName));
                        }
                }
            }

            set
            {
                SetValue(BigIntegerToSqlDecimal(value));
            }
        }
    }
}
