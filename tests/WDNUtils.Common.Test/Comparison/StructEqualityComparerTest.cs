using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class StructEqualityComparerTest
    {
        [TestMethod]
        public async Task TestStructEqualityComparer()
        {
            var taskList = new List<Task>();

            taskList.Add(Task.Run(() => Test<long>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<int>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<short>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<sbyte>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<ulong>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<uint>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<ushort>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<byte>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<decimal>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<double>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<float>(1, 1, 10, 10, 0, null, v => v.GetHashCode(), v => v == 0)));
            taskList.Add(Task.Run(() => Test<char>('\u0001', '\u0001', '\u000A', '\u000A', '\0', null, v => v.GetHashCode(), v => v == '\0')));
            taskList.Add(Task.Run(() => Test<DateTime>(new DateTime(1990, 1, 1), new DateTime(1990, 1, 1), new DateTime(2099, 1, 1), new DateTime(2099, 1, 1), new DateTime(2000, 1, 1), null, v => v.GetHashCode(), v => v == new DateTime(2000, 1, 1))));
            taskList.Add(Task.Run(() => Test<TimeSpan>(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), TimeSpan.Zero, null, v => v.GetHashCode(), v => v == TimeSpan.Zero)));

            // Custom GetHashCode implementation

            taskList.Add(Task.Run(() => Test<long>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<int>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<short>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<sbyte>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<ulong>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<uint>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<ushort>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<byte>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<decimal>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<double>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<float>(1, 1, 10, 10, 0, null, v => (int)(v % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<char>('\u0001', '\u0001', '\u000A', '\u000A', '\0', null, v => (int)(v % 100), v => v == '\0')));
            taskList.Add(Task.Run(() => Test<DateTime>(new DateTime(2000, 1, 1), new DateTime(2000, 1, 1), new DateTime(2000, 1, 10), new DateTime(2000, 1, 10), new DateTime(1999, 12, 31), null, v => (int)(v.Ticks % 100), v => v == new DateTime(1999, 12, 31))));
            taskList.Add(Task.Run(() => Test<TimeSpan>(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10), TimeSpan.Zero, null, v => (int)(v.Ticks % 100), v => v == TimeSpan.Zero)));

            // Custom Equals implementation with absolute values (positive == negative)

            taskList.Add(Task.Run(() => Test<long>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<int>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<short>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<sbyte>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<decimal>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<double>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<float>(1, -1, 10, -10, 0, (x, y) => Math.Abs(x) == Math.Abs(y), v => (int)(Math.Abs(v) % 100), v => v == 0)));
            taskList.Add(Task.Run(() => Test<TimeSpan>(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1).Negate(), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10).Negate(), TimeSpan.Zero, (x, y) => Math.Abs(x.Ticks) == Math.Abs(y.Ticks), v => (int)(Math.Abs(v.Ticks) % 100), v => v == TimeSpan.Zero)));

            await Task.WhenAll(taskList);
        }

        #region Internal methods

        public void Test<T>(T x1, T x2, T y1, T y2, T zero, Func<T, T, bool> equals, Func<T, int> getHashCode, Func<T, bool> isNull) where T : struct, IComparable<T>
        {
            var comparer = (equals is null)
                ? StructEqualityComparer.CreateFromComparable(getHashCode: getHashCode ?? ((obj) => ((object)obj).GetHashCode()), isNull: null)
                : new StructEqualityComparer<T>(equals: equals, getHashCode: getHashCode ?? ((obj) => ((object)obj).GetHashCode()), isNull: null);

            TestInternal<T>(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: null, comparer: comparer);
            TestInternal<T>(expected: true, v1: zero, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: zero, comparer: comparer);
            TestInternal<T>(expected: true, v1: x1, v2: x1, comparer: comparer);
            TestInternal<T>(expected: true, v1: x1, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: zero, comparer: comparer);
            TestInternal<T>(expected: true, v1: x2, v2: x1, comparer: comparer);
            TestInternal<T>(expected: true, v1: x2, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: x2, comparer: comparer);
            TestInternal<T>(expected: true, v1: y1, v2: y1, comparer: comparer);
            TestInternal<T>(expected: true, v1: y1, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: x2, comparer: comparer);
            TestInternal<T>(expected: true, v1: y2, v2: y1, comparer: comparer);
            TestInternal<T>(expected: true, v1: y2, v2: y2, comparer: comparer);

            comparer = (equals is null)
                ? StructEqualityComparer.CreateFromComparable(getHashCode: getHashCode ?? ((obj) => ((object)obj).GetHashCode()), isNull: isNull)
                : new StructEqualityComparer<T>(equals: equals, getHashCode: getHashCode ?? ((obj) => ((object)obj).GetHashCode()), isNull: isNull);

            TestInternal<T>(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal<T>(expected: true, v1: null, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: null, v2: y2, comparer: comparer);
            TestInternal<T>(expected: true, v1: zero, v2: null, comparer: comparer);
            TestInternal<T>(expected: true, v1: zero, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: zero, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: zero, comparer: comparer);
            TestInternal<T>(expected: true, v1: x1, v2: x1, comparer: comparer);
            TestInternal<T>(expected: true, v1: x1, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: x1, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: zero, comparer: comparer);
            TestInternal<T>(expected: true, v1: x2, v2: x1, comparer: comparer);
            TestInternal<T>(expected: true, v1: x2, v2: x2, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: y1, comparer: comparer);
            TestInternal<T>(expected: false, v1: x2, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: y1, v2: x2, comparer: comparer);
            TestInternal<T>(expected: true, v1: y1, v2: y1, comparer: comparer);
            TestInternal<T>(expected: true, v1: y1, v2: y2, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: null, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: zero, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: x1, comparer: comparer);
            TestInternal<T>(expected: false, v1: y2, v2: x2, comparer: comparer);
            TestInternal<T>(expected: true, v1: y2, v2: y1, comparer: comparer);
            TestInternal<T>(expected: true, v1: y2, v2: y2, comparer: comparer);
        }

        private void TestInternal<T>(bool expected, T? v1, T? v2, StructEqualityComparer<T> comparer) where T : struct
        {
            if ((expected) && (comparer.GetHashCode(obj: v1) != comparer.GetHashCode(obj: v2)))
            {
                var xStr = (v1 is null) ? "null" : $@"""{v1}""";
                var yStr = (v2 is null) ? "null" : $@"""{v2}""";

                Assert.Fail($@"{nameof(StructEqualityComparer.GetHashCode)}(obj: {xStr}) != {nameof(StructEqualityComparer.GetHashCode)}(obj: {yStr})");
            }

            if (comparer.Equals(x: v1, y: v2) != expected)
            {
                var xStr = (v1 is null) ? "null" : $@"""{v1}""";
                var yStr = (v2 is null) ? "null" : $@"""{v2}""";

                Assert.Fail($@"{nameof(StructEqualityComparer.Equals)}(x: {xStr}, y: {yStr}) != {expected}");
            }

            if ((v1 is null) || (v2 is null))
                return;

            if ((expected) && (comparer.GetHashCode(obj: v1.Value) != comparer.GetHashCode(obj: v2.Value)))
            {
                var xStr = $@"""{v1.Value}""";
                var yStr = $@"""{v2.Value}""";

                Assert.Fail($@"{nameof(StructEqualityComparer.GetHashCode)}(obj: {xStr}) != {nameof(StructEqualityComparer.GetHashCode)}(obj: {yStr})");
            }

            if (comparer.Equals(x: v1.Value, y: v2.Value) != expected)
            {
                var xStr = $@"""{v1.Value}""";
                var yStr = $@"""{v2.Value}""";

                Assert.Fail($@"{nameof(StructEqualityComparer.Equals)}(x: {xStr}, y: {yStr}) != {expected}");
            }
        }

        #endregion
    }
}
