using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace WDNUtils.Common
{
    /// <summary>
    /// Defines methods to support the comparison of objects for equality
    /// </summary>
    /// <typeparam name="T">The type of objects to compare</typeparam>
    public class ClassEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        /// <summary>
        /// Default ClassEqualityComparer instance, using the object references to determine the equality 
        /// </summary>
        public static ClassEqualityComparer<T> ReferenceEquality { get; private set; } =
            new ClassEqualityComparer<T>(equals: ReferenceEquals, getHashCode: RuntimeHelpers.GetHashCode);

        /// <summary>
        /// Method that determines whether the specified objects are equal
        /// </summary>
        private readonly Func<T, T, bool> _equals;

        /// <summary>
        /// Method that returns a hash code for the specified object
        /// </summary>
        private readonly Func<T, int> _getHashCode;

        /// <summary>
        /// Method that determines whether the specified non-null object should be handled as null
        /// </summary>
        private readonly Func<T, bool> _isNull;

        /// <summary>
        /// Creates a new instance of ClassEqualityComparer
        /// </summary>
        /// <param name="equals">Method that determines whether the specified objects are equal</param>
        /// <param name="getHashCode">Method that returns a hash code for the specified object</param>
        /// <param name="isNull">Method that determines whether the specified non-null object should be handled as null</param>
        public ClassEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode, Func<T, bool> isNull = null)
        {
            _equals = equals ?? throw new ArgumentNullException(nameof(equals));
            _getHashCode = getHashCode ?? throw new ArgumentNullException(nameof(getHashCode));
            _isNull = isNull;
        }

        /// <summary>
        /// Determines whether the specified objects are equal
        /// </summary>
        /// <param name="x">The first object of type T to compare</param>
        /// <param name="y">The second object of type T to compare</param>
        /// <returns>True if the specified objects are equal; otherwise, false</returns>
        public bool Equals(T x, T y)
        {
            return
                ((x is null) || (_isNull?.Invoke(x) == true)) ? ((y is null) || (_isNull?.Invoke(y) == true)) :
                ((y is null) || (_isNull?.Invoke(y) == true)) ? false :
                _equals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object
        /// </summary>
        /// <param name="obj">The object for which a hash code is to be returned</param>
        /// <returns>A hash code for the specified object</returns>
        public int GetHashCode(T obj)
        {
            return ((obj is null) || (_isNull?.Invoke(obj) == true)) ? 0 : _getHashCode(obj);
        }
    }

    /// <summary>
    /// Defines methods to support the comparison of objects for equality (non generic static class)
    /// </summary>
    public static class ClassEqualityComparer
    {
        /// <summary>
        /// Creates a ClassEqualityComparer&lt;T&gt; from an object that implements IComparable&lt;T&gt;
        /// </summary>
        /// <param name="getHashCode">Method that returns a hash code for the specified object</param>
        /// <param name="isNull">Method that determines whether the specified non-null object should be handled as null</param>
        /// <typeparam name="T">The type of objects to compare</typeparam>
        /// <returns>An instance of ClassEqualityComparer&lt;T&gt;</returns>
        public static ClassEqualityComparer<T> CreateFromComparable<T>(Func<T, int> getHashCode, Func<T, bool> isNull = null) where T : class, IComparable<T>
        {
            return new ClassEqualityComparer<T>(
                equals: (x, y) => x.CompareTo(y) == 0, // Null check is done by ClassEqualityComparer<T>.Equals
                getHashCode: getHashCode,
                isNull: isNull);
        }
    }
}
