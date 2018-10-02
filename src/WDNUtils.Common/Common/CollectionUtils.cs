using System;
using System.Collections.Generic;
using System.Linq;

namespace WDNUtils.Common
{
    /// <summary>
    /// Collection extensions
    /// </summary>
    public static class CollectionUtils
    {
        #region Dictionary extensions

        /// <summary>
        /// Gets the value associated with the specified key, or the default value if the dictionary does not contains an element with the specified key
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
        /// <param name="dictionary">Collection of key/value pairs</param>
        /// <param name="key">The key whose value to get</param>
        /// <param name="defaultValue">Value to be returned if the dictionary does not contains an element with the specified key ('default(TValue)' if not specified)</param>
        /// <returns>The value associated with the specified key, or the default value if the dictionary does not contains an element with the specified key</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            return (dictionary.TryGetValue(key: key, value: out TValue value)) ? value : defaultValue;
        }

        /// <summary>
        /// Gets the value associated with the specified key, or null if the dictionary does not contains an element with the specified key
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
        /// <param name="dictionary">Collection of key/value pairs</param>
        /// <param name="key">The key whose value to get</param>
        /// <returns>The value associated with the specified key, or null if the dictionary does not contains an element with the specified key</returns>
        public static TValue? GetOrNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : struct
        {
            return (dictionary.TryGetValue(key: key, value: out TValue value)) ? value : (TValue?)null;
        }

        #endregion

        #region Nullable struct sequence extensions

        /// <summary>
        /// Converts a IEnumerable&lt;T&gt; into IEnumerable&lt;Nullable&lt;T&gt;&gt;, where T is a struct or primitive
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to convert</param>
        /// <returns>A sequence of Nullable&lt;T&gt;</returns>
        public static IEnumerable<T?> AsNullable<T>(this IEnumerable<T> list) where T : struct
        {
            return list?.Select(selector: item => (T?)item);
        }

        /// <summary>
        /// Returns the first element of a sequence, or null if the sequence contains no elements
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to return the first element of</param>
        /// <returns>The first element of the sequence, or null if the sequence contains no elements</returns>
        public static T? FirstOrNull<T>(this IEnumerable<T> list) where T : struct
        {
            return list?.Select(selector: item => (T?)item)?.FirstOrDefault();
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or null if no such element is found
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to return an element of</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>The first element in source that passes the test specified by predicate, or null if source is empty or if no element passes the test specified by predicate</returns>
        public static T? FirstOrNull<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : struct
        {
            return list?.Where(predicate: predicate)?.Select(selector: item => (T?)item)?.FirstOrDefault();
        }

        /// <summary>
        /// Returns the last element of a sequence, or null if the sequence contains no elements
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to return the last element of</param>
        /// <returns>The last element of the sequence, or null if the sequence contains no elements</returns>
        public static T? LastOrNull<T>(this IEnumerable<T> list) where T : struct
        {
            return list?.Select(selector: item => (T?)item)?.LastOrDefault();
        }

        /// <summary>
        /// Returns the last element of the sequence that satisfies a condition or null if no such element is found
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to return an element of</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>The last element in source that passes the test specified by predicate, or null if source is empty or if no element passes the test specified by predicate</returns>
        public static T? LastOrNull<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : struct
        {
            return list?.Where(predicate: predicate)?.Select(selector: item => (T?)item)?.LastOrDefault();
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to return the single element of</param>
        /// <returns>The single element of the input sequence, or null if the sequence contains no elements</returns>
        /// <exception cref="InvalidOperationException">The input sequence contains more than one element</exception>
        public static T? SingleOrNull<T>(this IEnumerable<T> list) where T : struct
        {
            return list?.Select(selector: item => (T?)item)?.SingleOrDefault();
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or a null if no such element exists; this method throws an exception if more than one element satisfies the condition
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The sequence to return the single element of</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <returns>The single element of the input sequence that satisfies the condition, or null if no such element is found</returns>
        /// <exception cref="InvalidOperationException">The input sequence contains more than one element that satisfies the condition</exception>
        public static T? SingleOrNull<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : struct
        {
            return list?.Where(predicate: predicate)?.Select(selector: item => (T?)item)?.SingleOrDefault();
        }

        #endregion

        #region List Shuffle

        /// <summary>
        /// Shuffles a list using Fisher–Yates algorithm, and returns itself (it does not create a new list)
        /// </summary>
        /// <typeparam name="T">The type of the elements of source sequence</typeparam>
        /// <param name="list">The list to shuffle</param>
        /// <returns>A reference to the input list (it does not create a new list)</returns>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            for (int index = (list?.Count ?? 0) - 1; index > 0; index--)
            {
                int swap = RandomUtils.Next(maxValue: index + 1);

                if (swap != index)
                {
                    T aux = list[index];
                    list[index] = list[swap];
                    list[swap] = aux;
                }
            }

            return list;
        }

        /// <summary>
        /// Take random elements from a sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements of source sequence</typeparam>
        /// <param name="list">The sequence to take elements from</param>
        /// <param name="count">The number of elements to take</param>
        /// <returns>The random elements from the source sequence</returns>
        public static IEnumerable<T> TakeShuffle<T>(this IEnumerable<T> list, int count)
        {
            T[] array = list.ToArray();

            if (count > array.Length)
            {
                count = array.Length;
            }

            int max = array.Length;

            while (count-- > 0)
            {
                int index = RandomUtils.Next(maxValue: max);

                yield return array[index];

                if (index < (max - 1))
                {
                    array[index] = array[max - 1];
                }

                max--;
            }
        }

        #endregion

        #region Split collection in limited length sublists

        /// <summary>
        /// Splits a list into sublists
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The list to split</param>
        /// <param name="sublistLength">The max length for the sublists</param>
        /// <returns>A sequence of List&lt;T&gt;</returns>
        public static IEnumerable<List<T>> SplitList<T>(this List<T> list, int sublistLength)
        {
            if (sublistLength < 1)
                throw new ArgumentOutOfRangeException(paramName: nameof(sublistLength));

            for (int remaining = list.Count, index = 0; remaining > 0; remaining -= sublistLength, index += sublistLength)
            {
                yield return list.GetRange(
                    index: index,
                    count: (remaining < sublistLength) ? remaining : sublistLength);
            }
        }

        /// <summary>
        /// Splits a sequence into sublists
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="collection">The sequence to split</param>
        /// <param name="sublistLength">The max length for the sublists</param>
        /// <returns>A sequence of List&lt;T&gt;</returns>
        public static IEnumerable<List<T>> SplitList<T>(this IEnumerable<T> collection, int sublistLength)
        {
            return collection.ToList().SplitList(sublistLength: sublistLength);
        }

        #endregion

        #region Remove duplicated items without changing sort order

        /// <summary>
        /// Removes duplicated elements of a list without changing its order
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="list">The source list to remove duplicated elements from</param>
        /// <returns>A sequence with unique elements from the source list</returns>
        public static IEnumerable<T> RemoveDuplicatesStable<T>(this List<T> list) where T : IEquatable<T>
        {
            var keyList = new HashSet<T>();

            foreach (var item in list)
            {
                if (!keyList.Contains(item: item))
                {
                    keyList.Add(item: item);
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Removes duplicated elements of a list without changing its order
        /// </summary>
        /// <typeparam name="TValue">The type of the elements of source</typeparam>
        /// <typeparam name="TKey">The type of the key used for equality comparison</typeparam>
        /// <param name="list">The source list to remove duplicated elements from</param>
        /// <param name="getKey">A function to get the key used for equality comparison for each element</param>
        /// <returns>A sequence with unique elements from the source list</returns>
        public static IEnumerable<TValue> RemoveDuplicatesStable<TValue, TKey>(this List<TValue> list, Func<TValue, TKey> getKey) where TKey : IEquatable<TKey>
        {
            var keyList = new HashSet<TKey>();

            foreach (var item in list)
            {
                var key = getKey(arg: item);

                if (!keyList.Contains(item: key))
                {
                    keyList.Add(item: key);
                    yield return item;
                }
            }
        }

        #endregion

        #region Concatenate arrays

        /// <summary>
        /// Concatenate multiple arrays into a single array
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="sources">The source arrays to be concatenated</param>
        /// <returns>An array with the concatenated elements of the source arrays</returns>
        public static T[] ArrayConcat<T>(params T[][] sources)
        {
            int destinationIndex = 0;
            int destinationSize = 0;

            for (int index = 0; index < sources.Length; index++)
            {
                destinationSize += sources[index].Length;
            }

            var buffer = new T[destinationSize];

            for (int index = 0; index < sources.Length; index++)
            {
                Array.Copy(
                    sourceArray: sources[index],
                    sourceIndex: 0,
                    destinationArray: buffer,
                    destinationIndex: destinationIndex,
                    length: sources[index].Length);

                destinationIndex += sources[index].Length;
            }

            return buffer;
        }

        #endregion

        #region Split arrays

        /// <summary>
        /// Split an array in two subarrays, with a given max length for the first output array
        /// </summary>
        /// <typeparam name="T">The type of the elements of source</typeparam>
        /// <param name="input">The input array to be split</param>
        /// <param name="output1Length">The max length of the first output subarray</param>
        /// <param name="output1">The first output subarray, with the specified max length</param>
        /// <param name="output2">The second output subarray, with the remaining elements</param>
        public static void ArraySplit<T>(this T[] input, int output1Length, out T[] output1, out T[] output2)
        {
            if (output1Length < 0)
                throw new ArgumentOutOfRangeException(paramName: nameof(output1Length));

            if (input.Length > output1Length)
            {
                output1 = new T[output1Length];
                output2 = new T[input.Length - output1Length];
            }
            else
            {
                output1 = new T[input.Length];
                output2 = new T[0];
            }

            Array.Copy(
                sourceArray: input,
                sourceIndex: 0,
                destinationArray: output1,
                destinationIndex: 0,
                length: output1.Length);

            Array.Copy(
                sourceArray: input,
                sourceIndex: output1.Length,
                destinationArray: output2,
                destinationIndex: 0,
                length: output2.Length);
        }

        #endregion
    }
}
