using System;
using System.Data;
using System.Data.SqlTypes;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// Boolean (1-bit number) bind parameter
    /// </summary>
    public sealed class DBSqlServerParameterBoolean : DBSqlServerParameter
    {
        /// <summary>
        /// Creates a bit bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBSqlServerParameterBoolean(string name, bool? value, ParameterDirection direction)
            : base(parameterName: name, sqlValue: (value is null) ? SqlBoolean.Null : new SqlBoolean(value.Value), type: SqlDbType.Bit, maxSize: null, direction: direction)
        {
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public bool? Value
        {
            get
            {
                var value = GetValue();

                switch (value)
                {
                    case null:
                        return null;
                    case SqlBoolean sqlBoolean:
                        return sqlBoolean.Value;
                    case SqlByte sqlByte:
                        return sqlByte.Value != 0;
                    case SqlDecimal sqlDecimal:
                        return sqlDecimal.Value != 0;
                    case SqlMoney sqlMoney:
                        return sqlMoney.Value != 0;
                    case SqlInt64 sqlInt64:
                        return sqlInt64.Value != 0;
                    case SqlInt32 sqlInt32:
                        return sqlInt32.Value != 0;
                    case SqlInt16 sqlInt16:
                        return sqlInt16.Value != 0;
                    case SqlDouble sqlDouble:
                        return Math.Abs(sqlDouble.Value) >= double.Epsilon;
                    case SqlSingle sqlSingle:
                        return Math.Abs(sqlSingle.Value) >= float.Epsilon;
                    default:
                        switch (Type.GetTypeCode(value.GetType()))
                        {
                            case TypeCode.UInt64:
                                return (((ulong)value) != 0);
                            case TypeCode.Int64:
                                return (((long)value) != 0);
                            case TypeCode.UInt32:
                                return (((uint)value) != 0);
                            case TypeCode.Int32:
                                return (((int)value) != 0);
                            case TypeCode.UInt16:
                                return (((ushort)value) != 0);
                            case TypeCode.Int16:
                                return (((short)value) != 0);
                            case TypeCode.Byte:
                                return (((byte)value) != 0);
                            case TypeCode.SByte:
                                return (((sbyte)value) != 0);
                            case TypeCode.Decimal:
                                return (((decimal)value) != 0);
                            case TypeCode.Single:
                                return (Math.Abs((float)value) >= double.Epsilon);
                            case TypeCode.Double:
                                return (Math.Abs((double)value) >= double.Epsilon);
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
                SetValue((value is null) ? SqlBoolean.Null : new SqlBoolean(value.Value));
            }
        }
    }
}
