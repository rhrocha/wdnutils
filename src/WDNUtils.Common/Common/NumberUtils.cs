using System;
using System.Globalization;

namespace WDNUtils.Common
{
    /// <summary>
    /// Culture invariant number/string conversion
    /// </summary>
    public static class NumberUtils
    {
        #region Culture invariant number to string conversion

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string DecimalToStringISO(decimal? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string Int64ToStringISO(long? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string Int32ToStringISO(int? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string Int16ToStringISO(short? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string SByteToStringISO(sbyte? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string UInt64ToStringISO(ulong? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string UInt32ToStringISO(uint? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string UInt16ToStringISO(ushort? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string ByteToStringISO(byte? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string DoubleToStringISO(double? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        /// <summary>
        /// Converts a numeric value to its equivalent string representation using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The number to be converted</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the value, or nullValue if the value is null</returns>
        public static string SingleToStringISO(float? value, string nullValue = @"NULL")
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo) ?? nullValue;
        }

        #endregion

        #region Culture invariant string to number conversion

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static decimal? ToDecimalISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return decimal.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static long? ToInt64ISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return long.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static int? ToInt32ISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return int.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static short? ToInt16ISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return short.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static sbyte? ToSByteISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return sbyte.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static ulong? ToUInt64ISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return ulong.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static uint? ToUInt32ISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return uint.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static ushort? ToUInt16ISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return ushort.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static byte? ToByteISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return byte.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static double? ToDoubleISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return double.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string representation of a number to a numeric value, using the format information of the invariant culture
        /// </summary>
        /// <param name="value">The string representation of a number to be converted</param>
        /// <param name="nullValue">The string that represents the null value (default is "NULL", case-insensitive)</param>
        /// <returns>The converted numeric value; or null if the string is null, empty, has only white-spaces characters, or has the same contents of nullValue (case-insensitive)</returns>
        public static float? ToSingleISO(this string value, string nullValue = @"NULL")
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return float.Parse(value, NumberFormatInfo.InvariantInfo);
        }

        #endregion

        #region Parse hexadecimal

        /// <summary>
        /// Converts a hexadecimal string to a numeric value
        /// </summary>
        /// <param name="value">The hexadecimal string to be converted</param>
        /// <returns>The numeric value represented by the hexadecimal string</returns>
        public static ulong ParseHex(string value)
        {
            return (TryParseHex(value: value, startIndex: 0, length: value?.Length ?? 0, result: out ulong result)) ? result : throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Converts a hexadecimal string to a numeric value
        /// </summary>
        /// <param name="value">The string that contains the hexadecimal value to be converted</param>
        /// <param name="startIndex">The zero-based starting character position of the hexadecimal value in the string</param>
        /// <param name="length">The number of characters in the hexadecimal value inside the string</param>
        /// <returns>The numeric value represented by the hexadecimal substring</returns>
        public static ulong ParseHex(string value, int startIndex, int length)
        {
            return (TryParseHex(value: value, startIndex: startIndex, length: length, result: out ulong result)) ? result : throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Converts a hexadecimal string to a numeric value
        /// </summary>
        /// <param name="value">The hexadecimal string to be converted</param>
        /// <param name="result">The numeric value represented by the hexadecimal string, ulong.MinValue if the string has invalid characters, zero if the string is empty or null, or ulong.MaxValue if the string has more than 16 characters</param>
        /// <returns>True if the hexadecimal string was converted to a numeric value successfully, otherwise false</returns>
        public static bool TryParseHex(string value, out ulong result)
        {
            return TryParseHex(value: value, startIndex: 0, length: value?.Length ?? 0, result: out result);
        }

        /// <summary>
        /// Converts a hexadecimal string to a numeric value
        /// </summary>
        /// <param name="value">The string that contains the hexadecimal value to be converted</param>
        /// <param name="startIndex">The zero-based starting character position of the hexadecimal value in the string</param>
        /// <param name="length">The number of characters in the hexadecimal value inside the string</param>
        /// <param name="result">The numeric value represented by the hexadecimal substring, ulong.MinValue if the substring has invalid characters, zero if the substring is empty or null, or ulong.MaxValue if the substring has more than 16 characters</param>
        /// <returns>True if the hexadecimal string was converted to a numeric value successfully, otherwise false</returns>
        public static bool TryParseHex(string value, int startIndex, int length, out ulong result)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            var end = startIndex + length;

            if ((length < 0) || (end > value?.Length))
                throw new ArgumentOutOfRangeException(nameof(length));

            if ((length <= 0) || (string.IsNullOrEmpty(value)))
            {
                result = 0;
                return false;
            }

            if (length > 16) // 16 hex == 8 bytes == 64 bits
            {
                while ((length > 16) && (value[startIndex] == '0'))
                {
                    length--;
                    startIndex++;
                }

                if (length > 16)
                {
                    result = ulong.MaxValue;
                    return false;
                }
            }

            result = 0;

            for (int index = startIndex; index < end; index++)
            {
                var chr = value[index];
                ulong chrValue;

                if ((chr >= '0') && (chr <= '9'))
                {
                    chrValue = (ulong)(chr - '0');
                }
                else if ((chr >= 'A') && (chr <= 'F'))
                {
                    chrValue = (ulong)(chr - 'A' + 10);
                }
                else if ((chr >= 'a') && (chr <= 'f'))
                {
                    chrValue = (ulong)(chr - 'a' + 10);
                }
                else
                {
                    result = ulong.MinValue;
                    return false;
                }

                result = (result << 4) | chrValue;
            }

            return true;
        }

        #endregion
    }
}
