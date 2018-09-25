using System;
using System.Collections.Generic;

namespace WDNUtils.Common
{
    /// <summary>
    /// Extended string comparer
    /// </summary>
    public sealed class StringComparerEx : IComparer<string>
    {
        #region Properties

        /// <summary>
        /// String comparison type.
        /// </summary>
        public StringComparisonEx ComparisonType { get; private set; }

        /// <summary>
        /// Indicates if null precedes non-null values in the sort order.
        /// </summary>
        public bool NullIsLower { get; private set; }

        /// <summary>
        /// Custom predicate to consider the values as null.
        /// </summary>
        public Func<string, bool> IsNull { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the string comparer
        /// </summary>
        /// <param name="comparisonType">Indicates if a case-insensitive comparison should be performed (default is Ordinal). WARNING: InvariantCulture should not be used in most cases; use Ordinal for application data, and Natural or CurrentCulture for user displayed data.</param>
        /// <param name="nullIsLower">Indicates if null precedes non-null values in the sort order (default is true)</param>
        /// <param name="isNull">Custom predicate to consider the values as null</param>
        public StringComparerEx(StringComparisonEx comparisonType = StringComparisonEx.Ordinal, bool nullIsLower = true, Func<string, bool> isNull = null)
        {
            ComparisonType = comparisonType;
            NullIsLower = nullIsLower; // Default is the same of string.Compare: null < not-null
            IsNull = isNull;
        }

        #endregion

        #region IComparer<string> implementation

        /// <summary>
        /// Compares two strings
        /// </summary>
        /// <param name="v1">The first string to compare</param>
        /// <param name="v2">The second string to compare</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        public int Compare(string v1, string v2)
        {
            return Compare(v1: v1, v2: v2, comparisonType: ComparisonType, nullIsLower: NullIsLower, isNull: IsNull);
        }

        #endregion

        #region String comparison

        /// <summary>
        /// Compares two strings
        /// </summary>
        /// <param name="v1">The first string to compare</param>
        /// <param name="v2">The second string to compare</param>
        /// <param name="comparisonType">Indicates if a case-insensitive comparison should be performed (default is StringComparison.Ordinal). WARNING: InvariantCulture should not be used in most cases; use Ordinal for application data, and Natural or CurrentCulture for user displayed data.</param>
        /// <param name="nullIsLower">Indicates if null precedes non-null values in the sort order (default is true)</param>
        /// <param name="isNull">Custom predicate to consider the values as null</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        public static int Compare(string v1, string v2, StringComparisonEx comparisonType = StringComparisonEx.Ordinal, bool nullIsLower = true, Func<string, bool> isNull = null)
        {
            if ((v1 is null) || (isNull?.Invoke(v1) == true))
            {
                if ((v2 is null) || (isNull?.Invoke(v2) == true))
                {
                    return 0;
                }
                else
                {
                    return nullIsLower ? -1 : 1;
                }
            }
            else if ((v2 is null) || (isNull?.Invoke(v2) == true))
            {
                return nullIsLower ? 1 : -1;
            }
            else
            {
                switch (comparisonType)
                {
                    case StringComparisonEx.Natural:
                        return NaturalStringCompare(v1: v1, v2: v2, ignoreCase: false);
                    case StringComparisonEx.NaturalIgnoreCase:
                        return NaturalStringCompare(v1: v1, v2: v2, ignoreCase: true);
                    default:
                        return string.Compare(strA: v1, strB: v2, comparisonType: (StringComparison)comparisonType);
                }
            }
        }

        #endregion

        #region Natural string comparison

        /// <summary>
        /// Natural string comparison
        /// </summary>
        /// <param name="v1">The first string to compare</param>
        /// <param name="v2">The second string to compare</param>
        /// <param name="ignoreCase">Indicates if the case of the strings being compared should be ignored</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        private static int NaturalStringCompare(string v1, string v2, bool ignoreCase)
        {
            int next1 = 0;
            int next2 = 0;
            int leadingZerosDiff = 0;

            var comparisonType = (ignoreCase) ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            while ((next1 < v1.Length) && (next2 < v2.Length))
            {
                var isDigit1 = (v1[next1] >= '0') && (v1[next1] <= '9');
                var isDigit2 = (v2[next2] >= '0') && (v2[next2] <= '9');

                if (isDigit1 != isDigit2)
                {
                    return (isDigit1) ? -1 : 1;
                }
                else if (!isDigit1)
                {
                    #region Seek last non-digit of group in each string

                    // Save the beginning of the non-digit groups
                    var start1 = next1;
                    var start2 = next2;

                    // Seek the end of the non-digit group (next1 will be at a digit char or the end of string)
                    while ((++next1 < v1.Length) && ((v1[next1] < '0') || (v1[next1] > '9'))) { }

                    // Seek the end of the non-digit group (next2 will be at a digit char or the end of string)
                    while ((++next2 < v2.Length) && ((v2[next2] < '0') || (v2[next2] > '9'))) { }

                    #endregion

                    #region Compare non-digit group

                    var result = string.Compare(
                        strA: v1,
                        indexA: start1,
                        strB: v2,
                        indexB: start2,
                        length: Math.Max(next1 - start1, next2 - start2),
                        comparisonType: comparisonType);

                    if (result != 0)
                        return result;

                    #endregion
                }
                else
                {
                    #region Seek last digit of group in each string

                    // Save the beginning of the digit groups
                    var start1 = next1;
                    var start2 = next2;

                    // Seek the end of the digit group (next1 will be at a non-digit char or the end of string)
                    while ((++next1 < v1.Length) && (v1[next1] >= '0') && (v1[next1] <= '9')) { }

                    // Seek the end of the digit group (next2 will be at a non-digit char or the end of string)
                    while ((++next2 < v2.Length) && (v2[next2] >= '0') && (v2[next2] <= '9')) { }

                    #endregion

                    #region Compare excess digits in the bigger digit group, if any

                    var count1 = next1 - start1; // Digit group length in v1
                    var count2 = next2 - start2; // Digit group length in v2
                    int countMin;

                    if (count1 > count2)
                    {
                        countMin = count2;

                        var last = next1 - countMin;

                        for (int digit = start1; digit < last; digit++)
                        {
                            if (v1[digit] != '0')
                                return 1;
                        }

                        if (leadingZerosDiff == 0)
                        {
                            // Only the first difference of leading zeros is stored, to be used if remaining contents of the strings are equal
                            leadingZerosDiff = 1;
                        }
                    }
                    else if (count2 > count1)
                    {
                        countMin = count1;

                        var last = next2 - countMin;

                        for (int digit = start2; digit < last; digit++)
                        {
                            if (v2[digit] != '0')
                                return -1;
                        }

                        if (leadingZerosDiff == 0)
                        {
                            // Only the first difference of leading zeros is stored, to be used if remaining contents of the strings are equal
                            leadingZerosDiff = -1;
                        }
                    }
                    else // count1 == count2
                    {
                        countMin = count1;
                    }

                    #endregion

                    #region Compare latin digits in both strings

                    var result = string.Compare(
                        strA: v1,
                        indexA: next1 - countMin,
                        strB: v2,
                        indexB: next2 - countMin,
                        length: countMin,
                        comparisonType: StringComparison.Ordinal);

                    if (result != 0)
                        return result;

                    #endregion
                }
            }

            if (next1 < v1.Length)
            {
                return 1; // v1 has more characters
            }
            else if (next2 < v2.Length)
            {
                return -1; // v2 has more characters
            }
            else
            {
                return leadingZerosDiff; // Strings are equal only if all digit groups have the same number of leading zeros
            }
        }

        #endregion
    }
}
