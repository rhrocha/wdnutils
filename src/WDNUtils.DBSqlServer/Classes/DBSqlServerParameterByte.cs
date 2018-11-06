using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Byte (8-bit signed) number bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterByte : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a byte bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterByte(string name, byte? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlByte.Null : new SqlByte(value.Value), type: SqlDbType.TinyInt, maxSize: null, direction: direction)
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
                    case SqlByte sqlByte:
                        return sqlByte.Value;
                    case SqlDecimal sqlDecimal:
                        return checked((byte)Math.Round(sqlDecimal.Value));
                    case SqlInt64 sqlInt64:
                        return checked((byte)sqlInt64.Value);
                    case SqlInt32 sqlInt32:
                        return checked((byte)sqlInt32.Value);
                    case SqlInt16 sqlInt16:
                        return checked((byte)sqlInt16.Value);
                    case SqlMoney sqlMoney:
                        return checked((byte)Math.Round(sqlMoney.Value));
                    case SqlDouble sqlDouble:
                        return checked((byte)Math.Round(sqlDouble.Value));
                    case SqlSingle sqlSingle:
                        return checked((byte)Math.Round(sqlSingle.Value));
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
                                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                                    Parameter.ParameterName,
                                    value.GetType().FullName,
                                    typeof(byte).GetType().FullName));
                        }
                }
            }

            set
            {
                SetValue((value is null) ? SqlByte.Null : new SqlByte(value.Value));
            }
        }
    }
}
