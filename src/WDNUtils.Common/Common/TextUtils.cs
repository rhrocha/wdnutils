using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WDNUtils.Common
{
    /// <summary>
    /// Text processing utilities
    /// </summary>
    public static class TextUtils
    {
        #region Constants

        /// <summary>
        /// String containing two line breaks
        /// </summary>
        public static readonly string DoubleNewLine = string.Concat(Environment.NewLine, Environment.NewLine);

        #endregion

        #region Remove diacritics

        /// <summary>
        /// Removes diacritics from a string (convert to unicode 'Form D' then remove non-spacing mark characters)
        /// </summary>
        /// <param name="value">Text where the diacritics will be removed</param>
        /// <returns>Text without diacritics</returns>
        public static string RemoveDiacritics(this string value)
        {
            return new string(value
                // Transform to Unicode normalization form D, which splits the base characters and diacritical marks. Example: "ÁÇÕ" => "A´C,O~"
                .Normalize(NormalizationForm.FormD)
                // Ignore characters that neither take space nor modify the base characters. Example: "A´C,O~" => "ACO"
                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                // Convert from IEnumerable<char> to char[], to be used in the constructor 'string(char[] value)'
                .ToArray());
        }

        #endregion

        #region Build a database search filter from a user input string

        /// <summary>
        /// Build a search filter from a user input string (remove diacritics, replace spaces by wildcard, and add wildcards to beginning and trailing)
        /// </summary>
        /// <param name="value">Input string</param>
        /// <param name="appendWildcards">Indicates if wildcards should be added to the beginning and trailing of the string</param>
        /// <param name="wildcard">Wildcard character (default is '%')</param>
        /// <returns>Search filter from a user input string</returns>
        public static string BuildSearchFilter(string value, bool appendWildcards = false, char wildcard = '%')
        {
            if (string.IsNullOrWhiteSpace(value))
                return wildcard.ToString();

            value = Regex.Replace(RemoveDiacritics(value.Trim()).ToUpper(), @"( |\*|\.|\?|_|%)+", wildcard.ToString());

            return (!appendWildcards) ? value : string.Concat(wildcard, value.Trim(wildcard), wildcard);
        }

        #endregion

        #region Substring

        /// <summary>
        /// Replacement of the method string.Substring(int start), returning null when the string is null
        /// (instead of throwing NullReferenceException), and string.Empty when the start index is bigger
        /// than the string length (instead of throwing ArgumentOutOfRangeException).
        /// </summary>
        /// <param name="value">Source string</param>
        /// <param name="start">The zero-based starting character position of a substring in this instance</param>
        /// <returns>The substring</returns>
        /// <exception cref="ArgumentOutOfRangeException">start index is lower than zero</exception>
        public static string SubstringSafe(this string value, int start)
        {
            // The string is null, or the extraction starts at beginning
            // of the string, so just return the whole string

            if ((value == null) || (start == 0))
                return value;

            // The start offset is bigger than the last character of the string
            // 'value[value.Length - 1]', so just return an empty string.
            // The comparison 'start > (value.Length - 1)' can be simplified
            // as 'start >= value.Length'.

            if (start >= value.Length)
                return string.Empty;

            return value.Substring(start);
        }

        /// <summary>
        /// Replacement of the method string.Substring(int start), returning null when the string is null
        /// (instead of throwing NullReferenceException), and string.Empty when the start index plus length
        /// is bigger than the string length (instead of throwing ArgumentOutOfRangeException).
        /// </summary>
        /// <param name="value">Source string</param>
        /// <param name="start">The zero-based starting character position of a substring in this instance</param>
        /// <param name="length">The number of characters in the substring</param>
        /// <returns>The substring</returns>
        /// <exception cref="ArgumentOutOfRangeException">start index is lower than zero</exception>
        public static string SubstringSafe(this string value, int start, int length)
        {
            // The string is null, or the extraction starts at beginning of the string and the desired
            // length is equal or bigger than the string length, so just return the whole string

            if ((value == null) || ((start == 0) && (length >= value.Length)))
                return value;

            // Desired length is less or equal to zero, or start offset is bigger than
            // the last character of the string 'value[value.Length - 1]', so just return
            // an empty string. The comparison 'start > (value.Length - 1)' can be simplified
            // as 'start >= value.Length'.

            if ((length <= 0) || (start >= value.Length))
                return string.Empty;

            // Start plus desired length exceeds the last character offset, so return
            // a substring using only the start offset argument.

            // The last extracted character 'value[start + length - 1]' would have a bigger
            // offset than the string last character 'value[value.Length - 1]', so
            // '(start + length - 1) > (value.Length - 1)'. This can be simplified to
            // '(start + length) > value.Length'.

            if ((start + length) > value.Length)
                return value.Substring(start);

            return value.Substring(start, length);
        }

        #endregion

        #region ASCII escaped values

        /// <summary>
        /// Converts a UTF-16 string into an ASCII escaped value (characters lower than 32, bigger than 127, and the backslash will be escaped in \u???? notation), so it can be stored in an single-byte text database column.
        /// </summary>
        /// <param name="value">String to be escaped</param>
        /// <returns>Escaped string, encoded with ASCII characters</returns>
        public static string UnicodeToEscapedAscii(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var result = new StringBuilder(value.Length * 6);

            for (int index = 0; index < value.Length; index++)
            {
                var chr = Convert.ToUInt16(value[index]);

                if ((chr >= 32) && (chr <= 127) &&
                    ((chr != '\\') || // current char is not escape character
                        (index >= (value.Length - 1)) || // current char is the last of the string
                        (value[index + 1] != 'u'))) // next char is not 'u'
                {
                    result.Append(value[index]);
                }
                else
                {
                    result.Append(@"\u").Append(chr.ToString("x4"));
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts an ASCII escaped string into a UTF-16 string (reverts the conversion done by UnicodeToEscapedAscii)
        /// </summary>
        /// <param name="value">Escaped string, encoded with ASCII characters</param>
        /// <returns>UTF-16 string</returns>
        public static string EscapedAsciiToUnicode(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var result = new StringBuilder(value.Length);

            int index = 0;

            while (index < value.Length)
            {
                var chr = value[index];

                if ((chr == '\\') && (index < (value.Length - 5)) && (value[index + 1] == 'u') && (NumberUtils.TryParseHex(value, index + 2, 4, out ulong number)))
                {
                    result.Append(Convert.ToChar(number));
                    index += 6;
                }
                else
                {
                    result.Append(chr);
                    index++;
                }
            }

            return result.ToString();
        }

        #endregion

        #region Text encoding

        /// <summary>
        /// Checks if a string can be encoded using a given text encoding
        /// </summary>
        /// <param name="encoding">Target text encoding</param>
        /// <param name="value">Text to be encoded</param>
        /// <returns>True if the string can be encoded using the specified text encoding</returns>
        public static bool CanEncode(this Encoding encoding, string value)
        {
            return (value?.Length > 0)
                ? string.Equals(encoding.GetString(encoding.GetBytes(value)), value, StringComparison.Ordinal)
                : true;
        }

        #endregion

        #region Trim implementation for StringBuilder object

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified in an array from the text of a StringBuilder
        /// </summary>
        /// <param name="stringBuilder">StringBuilder to be trimmed</param>
        /// <param name="trimChars"> An array of Unicode characters to remove, or null</param>
        /// <returns>A reference to the modified StringBuilder object</returns>
        public static StringBuilder Trim(this StringBuilder stringBuilder, params char[] trimChars)
        {
            return stringBuilder?.TrimStart(trimChars: trimChars)?.TrimEnd(trimChars: trimChars);
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the text of a StringBuilder
        /// </summary>
        /// <param name="stringBuilder">StringBuilder to be trimmed</param>
        /// <returns>A reference to the modified StringBuilder object</returns>
        public static StringBuilder Trim(this StringBuilder stringBuilder)
        {
            return stringBuilder?.TrimStart()?.TrimEnd();
        }

        /// <summary>
        /// Removes all leading occurrences of a set of characters specified in an array from the text of a StringBuilder
        /// </summary>
        /// <param name="stringBuilder">StringBuilder to be trimmed</param>
        /// <param name="trimChars"> An array of Unicode characters to remove, or null</param>
        /// <returns>A reference to the modified StringBuilder object</returns>
        public static StringBuilder TrimStart(this StringBuilder stringBuilder, params char[] trimChars)
        {
            if (!(stringBuilder?.Length > 0))
                return stringBuilder;

            if (!(trimChars?.Length > 0))
                return stringBuilder;

            int first = 0;

            while ((first < stringBuilder.Length) && (trimChars.Contains(stringBuilder[first])))
            {
                first++;
            }

            if (first >= stringBuilder.Length)
            {
                stringBuilder.Clear();
            }
            else if (first > 0)
            {
                stringBuilder.Remove(0, first);
            }

            return stringBuilder;
        }

        /// <summary>
        /// Removes all leading white-space characters from the text of a StringBuilder
        /// </summary>
        /// <param name="stringBuilder">StringBuilder to be trimmed</param>
        /// <returns>A reference to the modified StringBuilder object</returns>
        public static StringBuilder TrimStart(this StringBuilder stringBuilder)
        {
            if (!(stringBuilder?.Length > 0))
                return stringBuilder;

            int first = 0;

            while ((first < stringBuilder.Length) && (char.IsWhiteSpace(stringBuilder[first])))
            {
                first++;
            }

            if (first >= stringBuilder.Length)
            {
                stringBuilder.Clear();
            }
            else if (first > 0)
            {
                stringBuilder.Remove(0, first);
            }

            return stringBuilder;
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified in an array from the text of a StringBuilder
        /// </summary>
        /// <param name="stringBuilder">StringBuilder to be trimmed</param>
        /// <param name="trimChars"> An array of Unicode characters to remove, or null</param>
        /// <returns>A reference to the modified StringBuilder object</returns>
        public static StringBuilder TrimEnd(this StringBuilder stringBuilder, params char[] trimChars)
        {
            if (!(stringBuilder?.Length > 0))
                return stringBuilder;

            if (!(trimChars?.Length > 0))
                return stringBuilder;

            int last = stringBuilder.Length - 1;

            while ((last >= 0) && (trimChars.Contains(stringBuilder[last])))
            {
                last--;
            }

            if (last < 0)
            {
                stringBuilder.Clear();
            }
            else if (last < (stringBuilder.Length - 1))
            {
                stringBuilder.Length = last + 1;
            }

            return stringBuilder;
        }

        /// <summary>
        /// Removes all trailing white-space characters from the text of a StringBuilder
        /// </summary>
        /// <param name="stringBuilder">StringBuilder to be trimmed</param>
        /// <returns>A reference to the modified StringBuilder object</returns>
        public static StringBuilder TrimEnd(this StringBuilder stringBuilder)
        {
            if (!(stringBuilder?.Length > 0))
                return stringBuilder;

            int last = stringBuilder.Length - 1;

            while ((last >= 0) && (char.IsWhiteSpace(stringBuilder[last])))
            {
                last--;
            }

            if (last < 0)
            {
                stringBuilder.Clear();
            }
            else if (last < (stringBuilder.Length - 1))
            {
                stringBuilder.Length = last + 1;
            }

            return stringBuilder;
        }

        #endregion

        #region Base64 extension

        /// <summary>
        /// Converts the specified string, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array
        /// </summary>
        /// <param name="value">The string to convert</param>
        /// <returns>An array of 8-bit unsigned integers that is equivalent to value</returns>
        public static byte[] FromBase64(this string value)
        {
            return (value == null) ? null : Convert.FromBase64String(value);
        }

        /// <summary>
        /// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits
        /// </summary>
        /// <param name="value">An array of 8-bit unsigned integers</param>
        /// <returns>The string representation, in base 64, of the contents of value</returns>
        public static string ToBase64(this byte[] value)
        {
            return (value == null) ? null : Convert.ToBase64String(value);
        }

        #endregion
    }
}
