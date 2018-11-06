using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Integer (32-bit signed) number bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterInt32 : DBSqlServerParameter
    {
        /// <summary>
        /// Creates an int bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterInt32(string name, int? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlInt32.Null : new SqlInt32(value.Value), type: SqlDbType.Int, maxSize: null, direction: direction)
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

                if (value is null)
                {
                    return null;
                }
                else if (value is SqlInt32 sqlInt32)
                {
                    return sqlInt32.Value;
                }
                else if (value is SqlDecimal sqlDecimal)
                {
                    return checked((int)Math.Round(sqlDecimal.Value));
                }
                else if (value is SqlInt64 sqlInt64)
                {
                    return checked((int)sqlInt64.Value);
                }
                else if (value is SqlInt16 sqlInt16)
                {
                    return checked((int)sqlInt16.Value);
                }
                else if (value is SqlByte sqlByte)
                {
                    return checked((int)sqlByte.Value);
                }
                else if (value is SqlMoney sqlMoney)
                {
                    return checked((int)Math.Round(sqlMoney.Value));
                }
                else if (value is SqlDouble sqlDouble)
                {
                    return checked((int)Math.Round(sqlDouble.Value));
                }
                else if (value is SqlSingle sqlSingle)
                {
                    return checked((int)Math.Round(sqlSingle.Value));
                }

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
                        break;
                }

                throw new InvalidOperationException(string.Format(
                    DBSqlServerLocalizedText.DBSqlServerParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(int).GetType().FullName));
            }

            set
            {
                SetValue((value is null) ? SqlInt32.Null : new SqlInt32(value.Value));
            }
        }
    }
}
