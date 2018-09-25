using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class StructComparerTest
    {
        [TestMethod]
        public async Task TestStructComparer()
        {
            var taskList = new List<Task>();

            taskList.Add(Task.Run(() => Test<long>(long.MinValue, long.MaxValue, 0, val => -val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<int>(int.MinValue, int.MaxValue, 0, val => -val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<short>(short.MinValue, short.MaxValue, 0, val => (short)-val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<sbyte>(sbyte.MinValue, sbyte.MaxValue, 0, val => (sbyte)-val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<ulong>(ulong.MinValue, ulong.MaxValue, 0, null, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<uint>(uint.MinValue, uint.MaxValue, 0, null, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<ushort>(ushort.MinValue, ushort.MaxValue, 0, null, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<byte>(byte.MinValue, byte.MaxValue, 0, null, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<decimal>(decimal.MinValue, decimal.MaxValue, 0, val => -val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<double>(double.MinValue, double.MaxValue, 0, val => -val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<float>(float.MinValue, float.MaxValue, 0, val => -val, 1, 5, 10, 50, 100)));
            taskList.Add(Task.Run(() => Test<char>(char.MinValue, char.MaxValue, '\0', null, '0', 'A', 'Z', 'a', 'z')));
            taskList.Add(Task.Run(() => Test<DateTime>(DateTime.MinValue, DateTime.MaxValue, new DateTime(0), null, new DateTime(1990, 1, 1), new DateTime(2012, 2, 29), new DateTime(2012, 12, 31), new DateTime(2999, 12, 31))));
            taskList.Add(Task.Run(() => Test<TimeSpan>(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero, val => val.Negate(), TimeSpan.FromMilliseconds(100), TimeSpan.FromMinutes(1), TimeSpan.FromHours(1), TimeSpan.FromDays(366))));

            await Task.WhenAll(taskList);
        }

        public void Test<T>(T minValue, T maxValue, T zero, Func<T, T> negative, params T[] valueList) where T : struct, IComparable<T>
        {
            // Null equality
            TestEqual<T>(v1: null, v2: null, nullIsLower: null, isNull: null);

            // Null sort order
            TestBigger<T>(v1: zero, v2: null, nullIsLower: true, isNull: null);
            TestLower<T>(v1: zero, v2: null, nullIsLower: false, isNull: null);

            // Custom null values
            TestEqual<T>(v1: zero, v2: null, nullIsLower: null, isNull: val => val.CompareTo(zero) == 0);

            foreach (var value in valueList)
            {
                // Null sort order
                TestBigger<T>(v1: value, v2: null, nullIsLower: true, isNull: null);
                TestLower<T>(v1: value, v2: null, nullIsLower: false, isNull: null);

                // Value comparison
                TestBigger<T>(v1: value, v2: zero, nullIsLower: false, isNull: null);

                if (negative != null)
                {
                    // Null sort order
                    TestBigger<T>(v1: negative(value), v2: null, nullIsLower: true, isNull: null);
                    TestLower<T>(v1: negative(value), v2: null, nullIsLower: false, isNull: null);

                    // Value comparison
                    TestLower<T>(v1: negative(value), v2: zero, nullIsLower: false, isNull: null);
                }
            }

            var testList =
                Enumerable.Repeat<T?>(null, 2)
                .Concat((negative == null) ? Enumerable.Empty<T?>() : valueList.Reverse().Select(value => (T?)negative(value)))
                .Concat(Enumerable.Repeat<T?>(zero, 2))
                .Concat(valueList.Select(value => (T?)value))
                .ToList();

            TestListSort(nullIsLower: true, values: testList);

            testList =
                ((negative == null) ? Enumerable.Empty<T?>() : valueList.Reverse().Select(value => (T?)negative(value)))
                .Concat(Enumerable.Repeat<T?>(zero, 2))
                .Concat(valueList.Select(value => (T?)value))
                .Concat(Enumerable.Repeat<T?>(null, 2))
                .ToList();

            TestListSort(nullIsLower: false, values: testList);
        }

        #region Internal methods

        private void TestEqual<T>(T? v1, T? v2, bool? nullIsLower, Func<T, bool> isNull) where T : struct, IComparable<T>
        {
            var message = TestCompareInternal(result: 0, v1: v1, v2: v2, nullIsLower: nullIsLower, isNull: isNull);

            if (message != null)
                Assert.Fail(message);
        }

        private void TestBigger<T>(T? v1, T? v2, bool? nullIsLower, Func<T, bool> isNull) where T : struct, IComparable<T>
        {
            var message = TestCompareInternal(result: 1, v1: v1, v2: v2, nullIsLower: nullIsLower, isNull: isNull);

            if (message != null)
                Assert.Fail(message);
        }

        private void TestLower<T>(T? v1, T? v2, bool? nullIsLower, Func<T, bool> isNull) where T : struct, IComparable<T>
        {
            var message = TestCompareInternal(result: -1, v1: v1, v2: v2, nullIsLower: nullIsLower, isNull: isNull);

            if (message != null)
                Assert.Fail(message);
        }

        private void TestListSort<T>(bool nullIsLower, List<T?> values) where T : struct, IComparable<T>
        {
            var valueList = new List<T?>(values);

            // Suffle list

            var rnd = new Random();

            for (var index = valueList.Count * 2; index > 0; index--)
            {
                var item1 = rnd.Next(valueList.Count);
                var item2 = rnd.Next(valueList.Count);

                var aux = valueList[item1];
                valueList[item1] = valueList[item2];
                valueList[item2] = aux;
            }

            // Sort list

            valueList.Sort(new StructComparer<T>(nullIsLower: nullIsLower, isNull: null));

            for (int index = 0; index < values.Count; index++)
            {
                if ((values[index].HasValue != valueList[index].HasValue) || ((values[index].HasValue) && (values[index].Value.CompareTo(valueList[index].Value) != 0)))
                {
                    var v1 = (values[index] == null) ? "null" : $@"""{values[index]}""";
                    var v2 = (valueList[index] == null) ? "null" : $@"""{valueList[index]}""";

                    Assert.Fail($@"{nameof(TestListSort)} {nameof(values)}[{index}]: {v1} != {v2} ");
                }
            }
        }

        private string TestCompareInternal<T>(int result, T? v1, T? v2, bool? nullIsLower, Func<T, bool> isNull) where T : struct, IComparable<T>
        {
            if (nullIsLower.HasValue)
            {
                return TestCompareInternal1(nullIsLowerValue: nullIsLower.Value);
            }
            else
            {
                return TestCompareInternal1(nullIsLowerValue: false)
                    ?? TestCompareInternal1(nullIsLowerValue: true);
            }

            string TestCompareInternal1(bool nullIsLowerValue)
            {
                if (result != Math.Sign(StructComparer.Compare(v1: v1, v2: v2, nullIsLower: nullIsLowerValue, isNull: isNull)))
                {
                    var v1str = (v1 == null) ? "null" : $@"""{v1}""";
                    var v2str = (v2 == null) ? "null" : $@"""{v2}""";

                    return $@"{nameof(StructComparer)}.{nameof(StructComparer.Compare)}<{typeof(T).GetType().Name}>({nameof(v1)}: {v1str}, {nameof(v2)}: {v2str}, {nameof(nullIsLower)}: {(nullIsLowerValue ? "true" : "false")}) != {result}";
                }

                if (result != -Math.Sign(StructComparer.Compare(v1: v2, v2: v1, nullIsLower: nullIsLowerValue, isNull: isNull)))
                {
                    var v1str = (v1 == null) ? "null" : $@"""{v1}""";
                    var v2str = (v2 == null) ? "null" : $@"""{v2}""";

                    return $@"{nameof(StructComparer)}.{nameof(StructComparer.Compare)}<{typeof(T).GetType().Name}>({nameof(v1)}: {v2str}, {nameof(v2)}: {v1str}, {nameof(nullIsLower)}: {(nullIsLowerValue ? "true" : "false")}) != {result}";
                }

                return null;
            }
        }

        #endregion
    }
}
