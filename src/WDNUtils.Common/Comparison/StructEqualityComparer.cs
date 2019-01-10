using System;
using System.Collections.Generic;

namespace WDNUtils.Common
{
    /// <summary>
    /// Defines methods to support the comparison of nullable structs for equality
    /// </summary>
    /// <typeparam name="T">The type of structs to compare</typeparam>
    public class StructEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer<T?> where T : struct
    {
        /// <summary>
        /// Method that determines whether the specified structs are equal
        /// </summary>
        private readonly Func<T, T, bool> _equals;

        /// <summary>
        /// Method that returns a hash code for the specified struct
        /// </summary>
        private readonly Func<T, int> _getHashCode;

        /// <summary>
        /// Method that determines whether the specified non-null struct should be handled as null
        /// </summary>
        private readonly Func<T, bool> _isNull;

        /// <summary>
        /// Creates a new instance of StructEqualityComparer
        /// </summary>
        /// <param name="equals">Method that determines whether the specified structs are equal</param>
        /// <param name="getHashCode">Method that returns a hash code for the specified struct</param>
        /// <param name="isNull">Method that determines whether the specified non-null struct should be handled as null</param>
        public StructEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode, Func<T, bool> isNull = null)
        {
            _equals = equals ?? throw new ArgumentNullException(nameof(equals));
            _getHashCode = getHashCode ?? throw new ArgumentNullException(nameof(getHashCode));
            _isNull = isNull;
        }

        /// <summary>
        /// Determines whether the specified structs are equal
        /// </summary>
        /// <param name="x">The first struct of type T to compare</param>
        /// <param name="y">The second struct of type T to compare</param>
        /// <returns>True if the specified structs are equal; otherwise, false</returns>
        public bool Equals(T x, T y)
        {
            return
                (_isNull?.Invoke(x) == true) ? (_isNull?.Invoke(y) == true) :
                (_isNull?.Invoke(y) == true) ? false :
                _equals(x, y);
        }

        /// <summary>
        /// Determines whether the specified nullable structs are equal
        /// </summary>
        /// <param name="x">The first nullable struct of type T to compare</param>
        /// <param name="y">The second nullable struct of type T to compare</param>
        /// <returns>True if the specified nullable structs are equal; otherwise, false</returns>
        public bool Equals(T? x, T? y)
        {
            return
                ((x is null) || (_isNull?.Invoke(x.Value) == true)) ? ((y is null) || (_isNull?.Invoke(y.Value) == true)) :
                ((y is null) || (_isNull?.Invoke(y.Value) == true)) ? false :
                _equals(x.Value, y.Value);
        }

        /// <summary>
        /// Returns a hash code for the specified struct
        /// </summary>
        /// <param name="obj">The struct for which a hash code is to be returned</param>
        /// <returns>A hash code for the specified struct</returns>
        public int GetHashCode(T obj)
        {
            return (_isNull?.Invoke(obj) == true) ? 0 : _getHashCode(obj);
        }

        /// <summary>
        /// Returns a hash code for the specified nullable struct
        /// </summary>
        /// <param name="obj">The struct for which a hash code is to be returned</param>
        /// <returns>A hash code for the specified nullable struct</returns>
        public int GetHashCode(T? obj)
        {
            return ((obj is null) || (_isNull?.Invoke(obj.Value) == true)) ? 0 : _getHashCode(obj.Value);
        }
    }

    /// <summary>
    /// Defines methods to support the comparison of nullable structs for equality (non generic static class)
    /// </summary>
    public static class StructEqualityComparer
    {
        /// <summary>
        /// Creates a StructEqualityComparer&lt;T&gt; for a struct that implements IComparable&lt;T&gt;
        /// </summary>
        /// <param name="getHashCode">Method that returns a hash code for the specified struct</param>
        /// <param name="isNull">Method that determines whether the specified non-null struct should be handled as null</param>
        /// <typeparam name="T">The type of structs to compare</typeparam>
        /// <returns>An instance of StructEqualityComparer&lt;T&gt;</returns>
        public static StructEqualityComparer<T> CreateFromComparable<T>(Func<T, int> getHashCode, Func<T, bool> isNull = null) where T : struct, IComparable<T>
        {
            return new StructEqualityComparer<T>(
                equals: (x, y) => x.CompareTo(y) == 0, // Null check is done by StructEqualityComparer<T>.Equals
                getHashCode: getHashCode,
                isNull: isNull);
        }
    }
}
