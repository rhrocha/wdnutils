using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Long integer (64-bit signed) number bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterInt64 : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a long bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterInt64(string name, long? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlInt64.Null : new SqlInt64(value.Value), type: SqlDbType.BigInt, maxSize: null, direction: direction)
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
                    case SqlInt64 sqlInt64:
                        return sqlInt64.Value;
                    case SqlDecimal sqlDecimal:
                        return checked((long)Math.Round(sqlDecimal.Value));
                    case SqlInt32 sqlInt32:
                        return checked((long)sqlInt32.Value);
                    case SqlInt16 sqlInt16:
                        return checked((long)sqlInt16.Value);
                    case SqlByte sqlByte:
                        return checked((long)sqlByte.Value);
                    case SqlMoney sqlMoney:
                        return checked((long)Math.Round(sqlMoney.Value));
                    case SqlDouble sqlDouble:
                        return checked((long)Math.Round(sqlDouble.Value));
                    case SqlSingle sqlSingle:
                        return checked((long)Math.Round(sqlSingle.Value));
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
                                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(long).GetType().FullName));
                        }
                }
            }

            set
            {
                SetValue((value is null) ? SqlInt64.Null : new SqlInt64(value.Value));
            }
        }
    }
}
