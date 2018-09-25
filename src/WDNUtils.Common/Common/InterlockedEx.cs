using System;
using System.Threading;

namespace WDNUtils.Common
{
    /// <summary>
    /// Provides atomic operations for variables that are shared by multiple threads (extension of <see cref="Interlocked"/>)
    /// </summary>
    public class InterlockedEx
    {
        /// <summary>
        /// Compares two 32-bit signed integers for equality and, if they are equal, replaces the first value
        /// </summary>
        /// <param name="location1">The destination, whose value is compared with comparand and possibly replaced</param>
        /// <param name="newValue">The value that replaces the destination value if the comparison results in equality</param>
        /// <param name="oldValue">The value that is compared to the value at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange(ref int location1, int newValue, int oldValue)
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }

        /// <summary>
        /// Compares two 64-bit signed integers for equality and, if they are equal, replaces the first value
        /// </summary>
        /// <param name="location1">The destination, whose value is compared with comparand and possibly replaced</param>
        /// <param name="newValue">The value that replaces the destination value if the comparison results in equality</param>
        /// <param name="oldValue">The value that is compared to the value at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange(ref long location1, long newValue, long oldValue)
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }

        /// <summary>
        /// Compares two single-precision floating point numbers for equality and, if they are equal, replaces the first value
        /// </summary>
        /// <param name="location1">The destination, whose value is compared with comparand and possibly replaced</param>
        /// <param name="newValue">The value that replaces the destination value if the comparison results in equality</param>
        /// <param name="oldValue">The value that is compared to the value at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange(ref float location1, float newValue, float oldValue)
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }

        /// <summary>
        /// Compares two double-precision floating point numbers for equality and, if they are equal, replaces the first value
        /// </summary>
        /// <param name="location1">The destination, whose value is compared with comparand and possibly replaced</param>
        /// <param name="newValue">The value that replaces the destination value if the comparison results in equality</param>
        /// <param name="oldValue">The value that is compared to the value at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange(ref double location1, double newValue, double oldValue)
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }

        /// <summary>
        /// Compares two objects for reference equality and, if they are equal, replaces the first object
        /// </summary>
        /// <param name="location1">The destination object that is compared with comparand and possibly replaced</param>
        /// <param name="newValue">The object that replaces the destination object if the comparison results in equality</param>
        /// <param name="oldValue">The object that is compared to the object at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange(ref object location1, object newValue, object oldValue)
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }

        /// <summary>
        /// Compares two platform-specific handles or pointers for equality and, if they are equal, replaces the first one
        /// </summary>
        /// <param name="location1">The destination System.IntPtr, whose value is compared with the value of comparand and possibly replaced by value</param>
        /// <param name="newValue">The System.IntPtr that replaces the destination value if the comparison results in equality</param>
        /// <param name="oldValue">The System.IntPtr that is compared to the value at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange(ref IntPtr location1, IntPtr newValue, IntPtr oldValue)
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }

        /// <summary>
        /// Compares two instances of the specified reference type T for equality and, if they are equal, replaces the first one
        /// </summary>
        /// <typeparam name="T">The type to be used for location1, value, and comparand. This type must be a reference type.</typeparam>
        /// <param name="location1">The destination, whose value is compared with comparand and possibly replaced. This is a reference parameter (ref in C#, ByRef in Visual Basic).</param>
        /// <param name="newValue">The value that replaces the destination value if the comparison results in equality</param>
        /// <param name="oldValue">The value that is compared to the value at location1</param>
        /// <returns>True if the value in location1 was replaced, otherwise false</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer</exception>
        public static bool TryCompareExchange<T>(ref T location1, T newValue, T oldValue) where T : class
        {
            return Interlocked.CompareExchange(location1: ref location1, value: newValue, comparand: oldValue) == oldValue;
        }
    }
}
