using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Decimal (128-bit signed fixed point) number bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterDecimal : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a decimal bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterDecimal(string name, decimal? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlDecimal.Null : new SqlDecimal(value.Value), type: SqlDbType.Decimal, maxSize: null, direction: direction)
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

                switch (value)
                {
                    case null:
                        return null;
                    case SqlDecimal sqlDecimal:
                        return checked((decimal)sqlDecimal.Value);
                    case SqlMoney sqlMoney:
                        return sqlMoney.Value;
                    case SqlInt64 sqlInt64:
                        return checked((decimal)sqlInt64.Value);
                    case SqlInt32 sqlInt32:
                        return checked((decimal)sqlInt32.Value);
                    case SqlInt16 sqlInt16:
                        return checked((decimal)sqlInt16.Value);
                    case SqlByte sqlByte:
                        return checked((decimal)sqlByte.Value);
                    case SqlDouble sqlDouble:
                        return checked((decimal)sqlDouble.Value);
                    case SqlSingle sqlSingle:
                        return checked((decimal)sqlSingle.Value);
                    default:
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
                                throw new InvalidOperationException(string.Format(
                                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(decimal).GetType().FullName));
                        }
                }
            }

            set
            {
                SetValue((value is null) ? SqlDecimal.Null : new SqlDecimal(value.Value));
            }
        }
    }
}
