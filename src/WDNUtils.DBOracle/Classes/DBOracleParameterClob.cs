﻿using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Large string bind parameter
    /// </summary>
    public sealed class DBOracleParameterClob : DBOracleParameter
    {
        /// <summary>
        /// Creates a large string bind parameter
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter initial value (may be null)</param>
        /// <param name="maxSize">Maximum length for the value, may be null for input or input/output parameters; if null, the value may be truncated by the server when storing into a smaller column</param>
        /// <param name="direction">Parameter type (input, output, input/output, or return value)</param>
        internal DBOracleParameterClob(string name, string value, int? maxSize, ParameterDirection direction)
            : base(parameterName: name, value: value, type: OracleDbType.Clob, maxSize: maxSize, direction: direction)
        {
            if (maxSize.HasValue)
            {
                if (value?.Length > maxSize)
                    throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidMaxSizeString, name, value.Length, maxSize));
            }
            else
            {
                if ((Parameter.Direction != ParameterDirection.Input) && (Parameter.Direction != ParameterDirection.InputOutput))
                    throw new ArgumentOutOfRangeException(nameof(maxSize), string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidParameterDirectionAutoSize, name));
            }
        }

        /// <summary>
        /// Value of the bind parameter
        /// </summary>
        public string Value
        {
            get
            {
                var value = GetValue();

                if (value is null)
                {
                    return null;
                }
                else if (value is OracleClob oracleClob)
                {
                    return oracleClob.Value;
                }
                else if (value is OracleString oracleString)
                {
                    return oracleString.Value;
                }

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.String:
                        return (string)value;
                    case TypeCode.Char:
                        return ((char)value).ToString();
                }

                throw new InvalidOperationException(string.Format(
                    DBOracleLocalizedText.DBOracleParameter_CastError,
                    Parameter.ParameterName,
                    value.GetType().FullName,
                    typeof(string).GetType().FullName));
            }

            set
            {
                if ((Parameter.Size > 0) && (value?.Length > Parameter.Size))
                    throw new ArgumentOutOfRangeException(nameof(value), string.Format(DBOracleLocalizedText.DBOracleParameter_InvalidMaxSizeString, Parameter.ParameterName, value.Length, Parameter.Size));

                SetValue(value);
            }
        }
    }
}