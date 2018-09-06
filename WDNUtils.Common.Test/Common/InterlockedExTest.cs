using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class InterlockedExTest
    {
        [TestMethod]
        public void TestInterlockedEx()
        {
            var intVal = int.MinValue;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref intVal, newValue: int.MaxValue, oldValue: int.MinValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref intVal, newValue: int.MaxValue, oldValue: int.MinValue));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref intVal, newValue: int.MinValue, oldValue: int.MaxValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref intVal, newValue: int.MinValue, oldValue: int.MaxValue));

            var longVal = long.MinValue;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref longVal, newValue: long.MaxValue, oldValue: long.MinValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref longVal, newValue: long.MaxValue, oldValue: long.MinValue));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref longVal, newValue: long.MinValue, oldValue: long.MaxValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref longVal, newValue: long.MinValue, oldValue: long.MaxValue));

            var floatVal = float.MinValue;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref floatVal, newValue: float.MaxValue, oldValue: float.MinValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref floatVal, newValue: float.MaxValue, oldValue: float.MinValue));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref floatVal, newValue: float.MinValue, oldValue: float.MaxValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref floatVal, newValue: float.MinValue, oldValue: float.MaxValue));

            var doubleVal = double.MinValue;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref doubleVal, newValue: double.MaxValue, oldValue: double.MinValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref doubleVal, newValue: double.MaxValue, oldValue: double.MinValue));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref doubleVal, newValue: double.MinValue, oldValue: double.MaxValue));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref doubleVal, newValue: double.MinValue, oldValue: double.MaxValue));

            var object1 = new object();
            var object2 = new object();
            var objectVal = object1;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref objectVal, newValue: object2, oldValue: object1));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref objectVal, newValue: object2, oldValue: object1));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref objectVal, newValue: object1, oldValue: object2));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref objectVal, newValue: object1, oldValue: object2));

            var intPtr1 = new IntPtr(long.MinValue);
            var intPtr2 = new IntPtr(long.MaxValue);
            var intPtrVal = intPtr1;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref intPtrVal, newValue: intPtr2, oldValue: intPtr1));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref intPtrVal, newValue: intPtr2, oldValue: intPtr1));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref intPtrVal, newValue: intPtr1, oldValue: intPtr2));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref intPtrVal, newValue: intPtr1, oldValue: intPtr2));

            var tuple1 = Tuple.Create(int.MinValue, long.MinValue, float.MinValue, double.MinValue);
            var tuple2 = Tuple.Create(int.MaxValue, long.MaxValue, float.MaxValue, double.MaxValue);
            var tuple = tuple1;
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref tuple, newValue: tuple2, oldValue: tuple1));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref tuple, newValue: tuple2, oldValue: tuple1));
            Assert.IsTrue(InterlockedEx.TryCompareExchange(location1: ref tuple, newValue: tuple1, oldValue: tuple2));
            Assert.IsFalse(InterlockedEx.TryCompareExchange(location1: ref tuple, newValue: tuple1, oldValue: tuple2));
        }
    }
}
