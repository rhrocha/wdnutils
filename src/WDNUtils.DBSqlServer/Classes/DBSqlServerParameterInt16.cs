using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Short integer (16-bit signed) number bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterInt16 : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a short bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterInt16(string name, short? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlInt16.Null : new SqlInt16(value.Value), type: SqlDbType.SmallInt, maxSize: null, direction: direction)
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

                switch (value)
                {
                    case null:
                        return null;
                    case SqlDecimal sqlDecimal:
                        return checked((short)Math.Round(sqlDecimal.Value));
                    case SqlInt64 sqlInt64:
                        return checked((short)sqlInt64.Value);
                    case SqlInt32 sqlInt32:
                        return checked((short)sqlInt32.Value);
                    case SqlInt16 sqlInt16:
                        return sqlInt16.Value;
                    case SqlByte sqlByte:
                        return checked((short)sqlByte.Value);
                    case SqlMoney sqlMoney:
                        return checked((short)Math.Round(sqlMoney.Value));
                    case SqlDouble sqlDouble:
                        return checked((short)Math.Round(sqlDouble.Value));
                    case SqlSingle sqlSingle:
                        return checked((short)Math.Round(sqlSingle.Value));
                    default:
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
                                throw new InvalidOperationException(string.Format(
                                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(short).GetType().FullName));
                        }
                }
            }

            set
            {
                SetValue((value is null) ? SqlInt16.Null : new SqlInt16(value.Value));
            }
        }
    }
}
