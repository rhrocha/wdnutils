using System;
using System.Globalization;

namespace WDNUtils.Common
{
    /// <summary>
    /// Date/time utilities
    /// </summary>
    public static class DateUtils
    {
        #region Standard date/time formats

        /// <summary>
        /// Standard formats for DateTime.ToString
        /// </summary>
        public static class DateTimeFormat
        {
            /// <summary>
            /// Short date pattern (contains the day, month and year, using the local culture format).
            /// </summary>
            public const string CultureDate = @"d";

            /// <summary>
            /// Long time pattern (contains the hours, minutes and seconds, using the local culture format).
            /// </summary>
            public const string CultureTime = @"T";

            /// <summary>
            /// Short time pattern (contains the hours and minutes, using the local culture format).
            /// </summary>
            public const string CultureTimeWithoutSeconds = @"t";

            /// <summary>
            /// Short date and long time pattern separated by a space (contains the day, month, year, hours, minutes and seconds, using the local culture format).
            /// </summary>
            public const string CultureDateTime = @"G";

            /// <summary>
            /// Short date and short time pattern separated by a space (contains the day, month, year, hours, minutes and seconds, using the local culture format).
            /// </summary>
            public const string CultureDateTimeWithoutSeconds = @"g";

            /// <summary>
            /// ISO 8601 date/time without timezone and fractional seconds.
            /// It uses the format "yyyy'-'MM'-'dd'T'HH':'mm':'ss" with the invariant culture.
            /// </summary>
            public const string ISOShort = @"s";

            /// <summary>
            /// ISO 8601 date/time with timezone and fractional seconds.
            /// It uses the format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK" with the invariant culture.
            /// </summary>
            public const string ISOLong = @"o";

            /// <summary>
            /// Variant of ISO 8601 date/time, using a space as separator instead of 'T', without timezone and fractional seconds.
            /// It uses the format "yyyy'-'MM'-'dd' 'HH':'mm':'ss", and should be used with the invariant culture.
            /// </summary>
            public const string CustomShort = @"yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            /// <summary>
            /// Variant of ISO 8601 date/time, using a space as separator instead of 'T', without timezone, and with fractional seconds.
            /// It uses the format "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff" and should be used with the invariant culture.
            /// </summary>
            public const string CustomLocal = @"yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffff";

            /// <summary>
            /// Variant of ISO 8601 date/time, using a space as separator instead of 'T', with timezone and fractional seconds.
            /// It uses the format "yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffffK" and should be used with the invariant culture.
            /// </summary>
            public const string CustomLong = @"yyyy'-'MM'-'dd' 'HH':'mm':'ss'.'fffffffK";
        }

        /// <summary>
        /// Standard formats for TimeSpan.ToString
        /// </summary>
        public static class TimeSpanFormat
        {
            /// <summary>
            /// Always displays hours (single or double digit), minutes and seconds, and if necessary, it displays days and fractional seconds (with required number of digits).
            /// It uses the format "[-][d':']h':'mm':'ss[.FFFFFFF]" and should be used with the current culture.
            /// </summary>
            public const string Short = @"g";

            /// <summary>
            /// Always displays days, hours, minutes, seconds, and fractional seconds (7 digits with trailing zeros).
            /// It uses the format "[-]d':'hh':'mm':'ss.fffffff" and should be used with the current culture.
            /// </summary>
            public const string Long = @"G";

            /// <summary>
            /// Always displays hours, minutes and seconds, and if necessary, it display days and fractional seconds (7 digits with trailing zeros).
            /// It uses the format "[-][d'.']hh':'mm':'ss['.'fffffff]" with the invariant culture.
            /// </summary>
            public const string Invariant = @"c";
        }

        #endregion

        #region Date/time to string conversion

        /// <summary>
        /// Converts a DateTime to its string representation
        /// </summary>
        /// <param name="value">DateTime value to be converted to string</param>
        /// <param name="format">Format pattern for DateTime.ToString</param>
        /// <param name="nullValue">String to be returned if the value is null</param>
        /// <param name="minimumDateIsNull">Indicates if DateTime.MinValue should be handled as a null DateTime value</param>
        /// <param name="useInvariantCulture">Indicates if the invariant culture should be used instead of the current culture</param>
        /// <returns>The string representation of the DateTime value</returns>
        public static string ToStringEx(this DateTime? value, string format = DateTimeFormat.ISOShort, string nullValue = null, bool minimumDateIsNull = false, bool? useInvariantCulture = null)
        {
            if ((!value.HasValue) || ((minimumDateIsNull) && (value <= DateTime.MinValue)))
                return nullValue;

            if (useInvariantCulture is null)
            {
                switch (format)
                {
                    case DateTimeFormat.ISOShort:
                    case DateTimeFormat.ISOLong:
                    case DateTimeFormat.CustomShort:
                    case DateTimeFormat.CustomLocal:
                    case DateTimeFormat.CustomLong:
                        useInvariantCulture = true;
                        break;
                    default:
                        useInvariantCulture = false;
                        break;
                }
            }

            return value.Value.ToString(format, useInvariantCulture.Value ? DateTimeFormatInfo.InvariantInfo : DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts a TimeSpan to its string representation
        /// </summary>
        /// <param name="value">TimeSpan value to be converted to string</param>
        /// <param name="format">Format pattern for TimeSpan.ToString</param>
        /// <param name="nullValue">String to be returned if the value is null</param>
        /// <param name="useInvariantCulture">Indicates if the invariant culture should be used instead of the current culture</param>
        /// <returns>The string representation of the DateTime value</returns>
        public static string ToStringEx(this TimeSpan? value, string format = TimeSpanFormat.Short, string nullValue = @"", bool? useInvariantCulture = null)
        {
            if (!value.HasValue)
                return nullValue;

            if (useInvariantCulture is null)
            {
                switch (format)
                {
                    case TimeSpanFormat.Invariant:
                        useInvariantCulture = true;
                        break;
                    default:
                        useInvariantCulture = false;
                        break;
                }
            }

            // TimeSpan uses 'hh' instead of 'HH' for 00-23 hour values
            return value.Value.ToString(format, useInvariantCulture.Value ? DateTimeFormatInfo.InvariantInfo : DateTimeFormatInfo.CurrentInfo);
        }

        #endregion

        #region String to date/time conversion

        /// <summary>
        /// String to DateTime conversion
        /// </summary>
        /// <param name="value">String value to be converted to DateTime</param>
        /// <param name="useInvariantCulture">Indicates if the invariant culture must be used, instead of the current culture</param>
        /// <param name="nullValue">String value that represents a null DateTime</param>
        /// <returns>The DateTime value represented by the string, or null if the string is null, empty or equal to the nullValue</returns>
        public static DateTime? ToDateTime(this string value, bool useInvariantCulture, string nullValue = null)
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return DateTime.Parse(
                s: value,
                provider: useInvariantCulture ? DateTimeFormatInfo.InvariantInfo : DateTimeFormatInfo.CurrentInfo,
                styles: DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
        }

        /// <summary>
        /// String to DateTime conversion
        /// </summary>
        /// <param name="value">String value to be converted to DateTime</param>
        /// <param name="cultureInfo">Culture to be used in the conversion</param>
        /// <param name="nullValue">String value that represents a null DateTime</param>
        /// <returns>The DateTime value represented by the string, or null if the string is null, empty or equal to the nullValue</returns>
        public static DateTime? ToDateTime(this string value, CultureInfo cultureInfo, string nullValue = null)
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return DateTime.Parse(
                s: value,
                provider: cultureInfo.DateTimeFormat,
                styles: DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal);
        }

        /// <summary>
        /// String to TimeSpan conversion
        /// </summary>
        /// <param name="value">String value to be converted to TimeSpan</param>
        /// <param name="useInvariantCulture">Indicates if the invariant culture must be used, instead of the current culture</param>
        /// <param name="nullValue">String value that represents a null TimeSpan</param>
        /// <returns>The TimeSpan value represented by the string, or null if the string is null, empty or equal to the nullValue</returns>
        public static TimeSpan? ToTimeSpan(this string value, bool useInvariantCulture, string nullValue = null)
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return TimeSpan.Parse(
                input: value,
                formatProvider: useInvariantCulture ? DateTimeFormatInfo.InvariantInfo : DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// String to TimeSpan conversion
        /// </summary>
        /// <param name="value">String value to be converted to TimeSpan</param>
        /// <param name="cultureInfo">Culture to be used in the conversion</param>
        /// <param name="nullValue">String value that represents a null TimeSpan</param>
        /// <returns>The TimeSpan value represented by the string, or null if the string is null, empty or equal to the nullValue</returns>
        public static TimeSpan? ToTimeSpan(this string value, CultureInfo cultureInfo, string nullValue = null)
        {
            if ((string.IsNullOrWhiteSpace(value)) || (string.Equals(value, nullValue, StringComparison.OrdinalIgnoreCase)))
                return null;

            return TimeSpan.Parse(
                input: value,
                formatProvider: cultureInfo.DateTimeFormat);
        }

        #endregion

        #region Custom TimeSpan to string conversion

        /// <summary>
        /// Custom TimeSpan to string conversion
        /// </summary>
        /// <param name="value">TimeSpan value to be converted to a string</param>
        /// <param name="displayDays">Indicates if the number of whole days should be displayed; if false, the displayed number of hours may exceed 23</param>
        /// <param name="displaySeconds">Indicates if the number of seconds should be displayed; if false, the number of minutes wont be rounded (seconds will be truncated)</param>
        /// <param name="dayTimeSeparator">The separator character to be placed between the number of days and the number of hours</param>
        /// <param name="nullValue">The string to be returned if the value is null</param>
        /// <returns>The string representation of the TimeSpan value</returns>
        public static string ToStringEx(TimeSpan? value, bool displayDays, bool displaySeconds = true, string dayTimeSeparator = ".", string nullValue = null)
        {
            if (!value.HasValue)
                return nullValue;

            var signal = string.Empty;

            if (value.Value < TimeSpan.Zero)
            {
                signal = @"-";
                value = value.Value.Negate();
            }

            if ((displayDays) && (value.Value.Days > 0))
            {
                if (displaySeconds)
                    return $@"{(value.Value > TimeSpan.FromSeconds(1) ? signal : string.Empty)}{value.Value.Days:0}{dayTimeSeparator}{value.Value.Hours:00}:{value.Value.Minutes:00}:{value.Value.Seconds:00}";
                else
                    return $@"{(value.Value > TimeSpan.FromMinutes(1) ? signal : string.Empty)}{value.Value.Days:0}{dayTimeSeparator}{value.Value.Hours:00}:{value.Value.Minutes:00}";
            }
            else
            {
                if (displaySeconds)
                    return $@"{(value.Value > TimeSpan.FromSeconds(1) ? signal : string.Empty)}{((long)value.Value.TotalHours):00}:{value.Value.Minutes:00}:{value.Value.Seconds:00}";
                else
                    return $@"{(value.Value > TimeSpan.FromMinutes(1) ? signal : string.Empty)}{((long)value.Value.TotalHours):00}:{value.Value.Minutes:00}";
            }
        }

        #endregion

        #region Rounding

        /// <summary>
        /// Returns the value rounded to the nearest of the specified values (multiples of the scale plus the offset)
        /// </summary>
        /// <param name="value">TimeSpan value to be rounded</param>
        /// <param name="scale">Scale for the output values</param>
        /// <param name="offset">Offset for the output values</param>
        /// <returns>The value rounded to the nearest of the specified values (multiples of the scale plus the offset)</returns>
        /// <remarks>Example: With scale '00:00:30' and offset '00:00:10', the returned values could be: [...'-00:01:20', '-00:00:50', '-00:00:20', '00:00:10', '00:00:40', '00:01:10', ...]</remarks>
        public static DateTime Round(this DateTime value, TimeSpan scale, TimeSpan offset = default(TimeSpan))
        {
            if (scale <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(scale));

            return new DateTime(Convert.ToInt64(Math.Round(((decimal)value.Ticks - offset.Ticks) / scale.Ticks) * scale.Ticks) + offset.Ticks);
        }

        /// <summary>
        /// Returns the smallest value in the specified values (multiples of the scale plus the offset) that is greater than or equal to the specified value
        /// </summary>
        /// <param name="value">TimeSpan value to be rounded</param>
        /// <param name="scale">Scale for the output values</param>
        /// <param name="offset">Offset for the output values</param>
        /// <returns>The smallest value in the specified values (multiples of the scale plus the offset) that is greater than or equal to the specified value</returns>
        /// <remarks>Example: With scale '00:00:30' and offset '00:00:10', the returned values could be: [...'-00:01:20', '-00:00:50', '-00:00:20', '00:00:10', '00:00:40', '00:01:10', ...]</remarks>
        public static DateTime Ceiling(this DateTime value, TimeSpan scale, TimeSpan offset = default(TimeSpan))
        {
            if (scale <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(scale));

            return new DateTime(Convert.ToInt64(Math.Ceiling(((decimal)value.Ticks - offset.Ticks) / scale.Ticks) * scale.Ticks) + offset.Ticks);
        }

        /// <summary>
        /// Returns the largest value in the specified values (multiples of the scale plus the offset) that is less than or equal to the specified value
        /// </summary>
        /// <param name="value">TimeSpan value to be rounded</param>
        /// <param name="scale">Scale for the output values</param>
        /// <param name="offset">Offset for the output values</param>
        /// <returns>The largest value in the specified values (multiples of the scale plus the offset) that is less than or equal to the specified value</returns>
        /// <remarks>Example: With scale '00:00:30' and offset '00:00:10', the returned values could be: [...'-00:01:20', '-00:00:50', '-00:00:20', '00:00:10', '00:00:40', '00:01:10', ...]</remarks>
        public static DateTime Floor(this DateTime value, TimeSpan scale, TimeSpan offset = default(TimeSpan))
        {
            if (scale <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(scale));

            return new DateTime(Convert.ToInt64(Math.Floor(((decimal)value.Ticks - offset.Ticks) / scale.Ticks) * scale.Ticks) + offset.Ticks);
        }

        /// <summary>
        /// Returns the value rounded to the nearest of the specified values (multiples of the scale plus the offset)
        /// </summary>
        /// <param name="value">TimeSpan value to be rounded</param>
        /// <param name="scale">Scale for the output values</param>
        /// <param name="offset">Offset for the output values</param>
        /// <returns>The value rounded to the nearest of the specified values (multiples of the scale plus the offset)</returns>
        /// <remarks>Example: With scale '00:00:30' and offset '00:00:10', the returned values could be: [...'-00:01:20', '-00:00:50', '-00:00:20', '00:00:10', '00:00:40', '00:01:10', ...]</remarks>
        public static TimeSpan Round(this TimeSpan value, TimeSpan scale, TimeSpan offset = default(TimeSpan))
        {
            if (scale <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(scale));

            return new TimeSpan(Convert.ToInt64(Math.Round(((decimal)value.Ticks - offset.Ticks) / scale.Ticks) * scale.Ticks) + offset.Ticks);
        }

        /// <summary>
        /// Returns the smallest value in the specified values (multiples of the scale plus the offset) that is greater than or equal to the specified value
        /// </summary>
        /// <param name="value">TimeSpan value to be rounded</param>
        /// <param name="scale">Scale for the output values</param>
        /// <param name="offset">Offset for the output values</param>
        /// <returns>The smallest value in the specified values (multiples of the scale plus the offset) that is greater than or equal to the specified value</returns>
        /// <remarks>Example: With scale '00:00:30' and offset '00:00:10', the returned values could be: [...'-00:01:20', '-00:00:50', '-00:00:20', '00:00:10', '00:00:40', '00:01:10', ...]</remarks>
        public static TimeSpan Ceiling(this TimeSpan value, TimeSpan scale, TimeSpan offset = default(TimeSpan))
        {
            if (scale <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(scale));

            return new TimeSpan(Convert.ToInt64(Math.Ceiling(((decimal)value.Ticks - offset.Ticks) / scale.Ticks) * scale.Ticks) + offset.Ticks);
        }

        /// <summary>
        /// Returns the largest value in the specified values (multiples of the scale plus the offset) that is less than or equal to the specified value
        /// </summary>
        /// <param name="value">TimeSpan value to be rounded</param>
        /// <param name="scale">Scale for the output values</param>
        /// <param name="offset">Offset for the output values</param>
        /// <returns>The largest value in the specified values (multiples of the scale plus the offset) that is less than or equal to the specified value</returns>
        /// <remarks>Example: With scale '00:00:30' and offset '00:00:10', the returned values could be: [...'-00:01:20', '-00:00:50', '-00:00:20', '00:00:10', '00:00:40', '00:01:10', ...]</remarks>
        public static TimeSpan Floor(this TimeSpan value, TimeSpan scale, TimeSpan offset = default(TimeSpan))
        {
            if (scale <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(scale));

            return new TimeSpan(Convert.ToInt64(Math.Floor(((decimal)value.Ticks - offset.Ticks) / scale.Ticks) * scale.Ticks) + offset.Ticks);
        }

        #endregion

        #region Calculate age in whole years

        /// <summary>
        /// Calculate age in whole years
        /// </summary>
        /// <param name="endDate">Target date for age calculation</param>
        /// <param name="startDate">Creation date (birth)</param>
        /// <returns>Age in whole years</returns>
        public static long CalculateAge(DateTime endDate, DateTime startDate)
        {
            // Truncate time values
            endDate = endDate.Date;
            startDate = startDate.Date;

            if (startDate > endDate)
                throw new ArgumentOutOfRangeException(nameof(startDate));

            if ((endDate.Month < startDate.Month) || ((endDate.Month == startDate.Month) && (endDate.Day < startDate.Day)))
            {
                return endDate.Year - startDate.Year - 1;
            }
            else
            {
                return endDate.Year - startDate.Year;
            }
        }

        #endregion
    }
}
