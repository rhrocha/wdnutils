using System;

namespace WDNUtils.Common
{
    #region MutableTuple factory

    /// <summary>
    /// Mutable implementation of <see cref="Tuple"/>
    /// </summary>
    public static class MutableTuple
    {
        #region MutableTuple<T1>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1}"/></returns>
        public static MutableTuple<T1> Create<T1>(T1 item1)
            => new MutableTuple<T1>(item1: item1);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1}"/> with the initial values for the tuple component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1}"/></returns>
        public static MutableTuple<T1> AsMutable<T1>(this Tuple<T1> tuple)
            => new MutableTuple<T1>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2}"/></returns>
        public static MutableTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
            => new MutableTuple<T1, T2>(item1: item1, item2: item2);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2}"/></returns>
        public static MutableTuple<T1, T2> AsMutable<T1, T2>(this Tuple<T1, T2> tuple)
            => new MutableTuple<T1, T2>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2, T3>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <typeparam name="T3">The type of the tuple's third component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3}"/></returns>
        public static MutableTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
            => new MutableTuple<T1, T2, T3>(item1: item1, item2: item2, item3: item3);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3}"/></returns>
        public static MutableTuple<T1, T2, T3> AsMutable<T1, T2, T3>(this Tuple<T1, T2, T3> tuple)
            => new MutableTuple<T1, T2, T3>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2, T3, T4>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <typeparam name="T3">The type of the tuple's third component</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4}"/></returns>
        public static MutableTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
            => new MutableTuple<T1, T2, T3, T4>(item1: item1, item2: item2, item3: item3, item4: item4);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4}"/></returns>
        public static MutableTuple<T1, T2, T3, T4> AsMutable<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple)
            => new MutableTuple<T1, T2, T3, T4>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2, T3, T4, T5>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <typeparam name="T3">The type of the tuple's third component</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
            => new MutableTuple<T1, T2, T3, T4, T5>(item1: item1, item2: item2, item3: item3, item4: item4, item5: item5);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5> AsMutable<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple)
            => new MutableTuple<T1, T2, T3, T4, T5>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2, T3, T4, T5, T6>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <typeparam name="T3">The type of the tuple's third component</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <param name="item6">The value of the tuple's sixth component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
            => new MutableTuple<T1, T2, T3, T4, T5, T6>(item1: item1, item2: item2, item3: item3, item4: item4, item5: item5, item6: item6);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6> AsMutable<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple)
            => new MutableTuple<T1, T2, T3, T4, T5, T6>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2, T3, T4, T5, T6, T7>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <typeparam name="T3">The type of the tuple's third component</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth component</typeparam>
        /// <typeparam name="T7">The type of the tuple's seventh component</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <param name="item6">The value of the tuple's sixth component</param>
        /// <param name="item7">The value of the tuple's seventh component</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
            => new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(item1: item1, item2: item2, item3: item3, item4: item4, item5: item5, item6: item6, item7: item7);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7> AsMutable<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            => new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(tuple: tuple);

        #endregion

        #region MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/>
        /// </summary>
        /// <typeparam name="T1">The type of the tuple's first component</typeparam>
        /// <typeparam name="T2">The type of the tuple's second component</typeparam>
        /// <typeparam name="T3">The type of the tuple's third component</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth component</typeparam>
        /// <typeparam name="T7">The type of the tuple's seventh component</typeparam>
        /// <typeparam name="TRest">Any generic Tuple object that defines the types of the tuple's remaining components</typeparam>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <param name="item6">The value of the tuple's sixth component</param>
        /// <param name="item7">The value of the tuple's seventh component</param>
        /// <param name="rest">Any generic Tuple object that contains the values of the tuple's remaining components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/></returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> Create<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
            => new MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1: item1, item2: item2, item3: item3, item4: item4, item5: item5, item6: item6, item7: item7, rest: rest);

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> from a 
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> with the initial values for the tuple components</param>
        /// <returns>An instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/></returns>
        /// <remarks>The value of tuple.Rest will not be converted from <see cref="Tuple"/> to <see cref="MutableTuple"/> automatically</remarks>
        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> AsMutable<T1, T2, T3, T4, T5, T6, T7, TRest>(this Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            => new MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(tuple: tuple);

        #endregion
    }

    #endregion

    #region MutableTuple<T1>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    public sealed class MutableTuple<T1>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        public MutableTuple(T1 item1)
        {
            Item1 = item1;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1}"/> from a <see cref="Tuple{T1}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1}"/> with the initial values for the tuple component</param>
        public MutableTuple(Tuple<T1> tuple)
        {
            Item1 = tuple.Item1;
        }
    }

    #endregion

    #region MutableTuple<T1, T2>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    public sealed class MutableTuple<T1, T2>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        public MutableTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2}"/> from a <see cref="Tuple{T1, T2}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2}"/> with the initial values for the tuple components</param>
        public MutableTuple(Tuple<T1, T2> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
        }
    }

    #endregion

    #region MutableTuple<T1, T2, T3>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2, T3}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component</typeparam>
    public sealed class MutableTuple<T1, T2, T3>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// The value of the tuple's third component
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3}"/> from a <see cref="Tuple{T1, T2, T3}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3}"/> with the initial values for the tuple components</param>
        public MutableTuple(Tuple<T1, T2, T3> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
        }
    }

    #endregion

    #region MutableTuple<T1, T2, T3, T4>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2, T3, T4}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
    public sealed class MutableTuple<T1, T2, T3, T4>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// The value of the tuple's third component
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// The value of the tuple's fourth component
        /// </summary>
        public T4 Item4 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4}"/> from a <see cref="Tuple{T1, T2, T3, T4}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4}"/> with the initial values for the tuple components</param>
        public MutableTuple(Tuple<T1, T2, T3, T4> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
            Item4 = tuple.Item4;
        }
    }

    #endregion

    #region MutableTuple<T1, T2, T3, T4, T5>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2, T3, T4, T5}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
    public sealed class MutableTuple<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// The value of the tuple's third component
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// The value of the tuple's fourth component
        /// </summary>
        public T4 Item4 { get; set; }

        /// <summary>
        /// The value of the tuple's fifth component
        /// </summary>
        public T5 Item5 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5}"/> from a <see cref="Tuple{T1, T2, T3, T4, T5}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5}"/> with the initial values for the tuple components</param>
        public MutableTuple(Tuple<T1, T2, T3, T4, T5> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
            Item4 = tuple.Item4;
            Item5 = tuple.Item5;
        }
    }

    #endregion

    #region MutableTuple<T1, T2, T3, T4, T5, T6>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth component</typeparam>
    public sealed class MutableTuple<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// The value of the tuple's third component
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// The value of the tuple's fourth component
        /// </summary>
        public T4 Item4 { get; set; }

        /// <summary>
        /// The value of the tuple's fifth component
        /// </summary>
        public T5 Item5 { get; set; }

        /// <summary>
        /// The value of the tuple's sixth component
        /// </summary>
        public T6 Item6 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <param name="item6">The value of the tuple's sixth component</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6}"/> from a <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5, T6}"/> with the initial values for the tuple components</param>
        public MutableTuple(Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
            Item4 = tuple.Item4;
            Item5 = tuple.Item5;
            Item6 = tuple.Item6;
        }
    }

    #endregion

    #region MutableTuple<T1, T2, T3, T4, T5, T6, T7>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth component</typeparam>
    /// <typeparam name="T7">The type of the tuple's seventh component</typeparam>
    public sealed class MutableTuple<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// The value of the tuple's third component
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// The value of the tuple's fourth component
        /// </summary>
        public T4 Item4 { get; set; }

        /// <summary>
        /// The value of the tuple's fifth component
        /// </summary>
        public T5 Item5 { get; set; }

        /// <summary>
        /// The value of the tuple's sixth component
        /// </summary>
        public T6 Item6 { get; set; }

        /// <summary>
        /// The value of the tuple's seventh component
        /// </summary>
        public T7 Item7 { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <param name="item6">The value of the tuple's sixth component</param>
        /// <param name="item7">The value of the tuple's seventh component</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/> from a <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7}"/> with the initial values for the tuple components</param>
        public MutableTuple(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
            Item4 = tuple.Item4;
            Item5 = tuple.Item5;
            Item6 = tuple.Item6;
            Item7 = tuple.Item7;
        }
    }

    #endregion

    #region MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>

    /// <summary>
    /// Mutable implementation of <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/>, without implementing equatable or comparable interfaces due to its mutable nature (should not be used as keys in sets or dictionaries).
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component</typeparam>
    /// <typeparam name="T3">The type of the tuple's third component</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth component</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth component</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth component</typeparam>
    /// <typeparam name="T7">The type of the tuple's seventh component</typeparam>
    /// <typeparam name="TRest">Any generic Tuple object that defines the types of the tuple's remaining components</typeparam>
    public sealed class MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
        /// <summary>
        /// The value of the tuple's first component
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The value of the tuple's second component
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// The value of the tuple's third component
        /// </summary>
        public T3 Item3 { get; set; }

        /// <summary>
        /// The value of the tuple's fourth component
        /// </summary>
        public T4 Item4 { get; set; }

        /// <summary>
        /// The value of the tuple's fifth component
        /// </summary>
        public T5 Item5 { get; set; }

        /// <summary>
        /// The value of the tuple's sixth component
        /// </summary>
        public T6 Item6 { get; set; }

        /// <summary>
        /// The value of the tuple's seventh component
        /// </summary>
        public T7 Item7 { get; set; }

        /// <summary>
        /// The value of the tuple's remaining components
        /// </summary>
        public TRest Rest { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/>
        /// </summary>
        /// <param name="item1">The value of the tuple's first component</param>
        /// <param name="item2">The value of the tuple's second component</param>
        /// <param name="item3">The value of the tuple's third component</param>
        /// <param name="item4">The value of the tuple's fourth component</param>
        /// <param name="item5">The value of the tuple's fifth component</param>
        /// <param name="item6">The value of the tuple's sixth component</param>
        /// <param name="item7">The value of the tuple's seventh component</param>
        /// <param name="rest">Any generic Tuple object that contains the values of the tuple's remaining components</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            Item5 = item5;
            Item6 = item6;
            Item7 = item7;
            Rest = rest;
        }

        /// <summary>
        /// Creates an instance of <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> from a <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/>
        /// </summary>
        /// <param name="tuple">The <see cref="Tuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> with the initial values for the tuple components</param>
        /// <remarks>The value of tuple.Rest will not be converted from <see cref="Tuple"/> to <see cref="MutableTuple"/> automatically</remarks>
        public MutableTuple(Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            Item1 = tuple.Item1;
            Item2 = tuple.Item2;
            Item3 = tuple.Item3;
            Item4 = tuple.Item4;
            Item5 = tuple.Item5;
            Item6 = tuple.Item6;
            Item7 = tuple.Item7;
            Rest = tuple.Rest;
        }
    }

    #endregion
}
