using System;
using System.Collections.Generic;

namespace WDNUtils.Common
{
    /// <summary>
    /// Extended nullable struct comparer
    /// </summary>
    public class StructComparer<T> : IComparer<T?>, IComparer<T> where T : struct, IComparable<T>
    {
        #region Properties

        /// <summary>
        /// Indicates if null precedes non-null values in the sort order
        /// </summary>
        public bool NullIsLower { get; private set; }

        /// <summary>
        /// Custom predicate to consider the values as null
        /// </summary>
        public Func<T, bool> IsNull { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the struct comparer
        /// </summary>
        /// <param name="nullIsLower">Indicates if null precedes non-null values in the sort order (default is true)</param>
        /// <param name="isNull">Custom predicate to consider the values as null</param>
        public StructComparer(bool nullIsLower = true, Func<T, bool> isNull = null)
        {
            NullIsLower = nullIsLower;
            IsNull = isNull;
        }

        #endregion

        #region IComparer<T?> implementation

        /// <summary>
        /// Compares two nullable structs
        /// </summary>
        /// <param name="v1">The first struct to compare</param>
        /// <param name="v2">The second struct to compare</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        public int Compare(T? v1, T? v2)
        {
            return Compare(v1: v1, v2: v2, nullIsLower: NullIsLower, isNull: IsNull);
        }

        #endregion

        #region IComparer<T> implementation

        /// <summary>
        /// Compares two structs
        /// </summary>
        /// <param name="v1">The first struct to compare</param>
        /// <param name="v2">The second struct to compare</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        public int Compare(T v1, T v2)
        {
            return Compare(v1: v1, v2: v2, nullIsLower: NullIsLower, isNull: IsNull);
        }

        #endregion

        #region Struct comparison

        /// <summary>
        /// Compares two nullable structs
        /// </summary>
        /// <param name="v1">The first struct to compare</param>
        /// <param name="v2">The second struct to compare</param>
        /// <param name="nullIsLower">Indicates if null precedes non-null values in the sort order (default is true)</param>
        /// <param name="isNull">Custom predicate to consider the values as null</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        public static int Compare(T? v1, T? v2, bool nullIsLower = true, Func<T, bool> isNull = null)
        {
            if ((!v1.HasValue) || (isNull?.Invoke(v1.Value) == true))
            {
                if ((!v2.HasValue) || (isNull?.Invoke(v2.Value) == true))
                {
                    return 0;
                }
                else
                {
                    return nullIsLower ? -1 : 1;
                }
            }
            else if ((!v2.HasValue) || (isNull?.Invoke(v2.Value) == true))
            {
                return nullIsLower ? 1 : -1;
            }
            else
            {
                return v1.Value.CompareTo(v2.Value);
            }
        }

        #endregion
    }

    /// <summary>
    /// Extended nullable struct comparer (non generic static class)
    /// </summary>
    public static class StructComparer
    {
        #region Struct comparison

        /// <summary>
        /// Compares two nullable structs
        /// </summary>
        /// <param name="v1">The first struct to compare</param>
        /// <param name="v2">The second struct to compare</param>
        /// <param name="nullIsLower">Indicates if null precedes non-null values in the sort order (default is true)</param>
        /// <param name="isNull">Custom predicate to consider the values as null</param>
        /// <returns>Less than zero if v1 precedes v2 in the sort order. Zero v1 if occurs in the same position as v2 in the sort order. Greater than zero if v1 follows v2 in the sort order</returns>
        public static int Compare<T>(T? v1, T? v2, bool nullIsLower = true, Func<T, bool> isNull = null) where T : struct, IComparable<T>
        {
            return StructComparer<T>.Compare(v1: v1, v2: v2, nullIsLower: nullIsLower, isNull: isNull);
        }

        #endregion
    }
}
