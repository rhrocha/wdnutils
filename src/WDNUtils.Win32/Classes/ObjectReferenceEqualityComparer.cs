using System.Collections.Generic;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Equality comparer implementation for object instances, to use the instances as the key in dictionaries and hash sets
    /// </summary>
    public class ObjectReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
