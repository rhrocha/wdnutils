using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class RandomUtilsTest
    {
        private const int TestCount = 10000;

        [TestMethod]
        public async Task TestRandomUtils()
        {
            var taskNext = TestMethod<decimal>(() => RandomUtils.Next(), (v) => (v >= 0) && (v < Int32.MaxValue));
            var taskNextMax = TestMethod<decimal>(() => RandomUtils.Next(321), (v) => (v >= 0) && (v < 321));
            var taskNextRange = TestMethod<decimal>(() => RandomUtils.Next(123, 321), (v) => (v >= 123) && (v < 321));
            var taskNextTest = TestMethod<decimal>(() => RandomUtils.Next(123, 123), (v) => v == 123);
            var taskNextInt32 = TestMethod<decimal>(() => RandomUtils.NextInt32(), (v) => (v >= Int32.MinValue) && (v < Int32.MaxValue));
            var taskNextInt32Max = TestMethod<decimal>(() => RandomUtils.NextInt32(321), (v) => (v >= Int32.MinValue) && (v < 321));
            var taskNextInt32Range = TestMethod<decimal>(() => RandomUtils.NextInt32(123, 321), (v) => (v >= 123) && (v < 321));
            var taskNextInt32Test = TestMethod<decimal>(() => RandomUtils.NextInt32(123, 123), (v) => v == 123);
            var taskNextInt64 = TestMethod<decimal>(() => RandomUtils.NextInt64(), (v) => (v >= Int64.MinValue) && (v < Int64.MaxValue));
            var taskNextInt64Max = TestMethod<decimal>(() => RandomUtils.NextInt64(641), (v) => (v >= Int64.MinValue) && (v < 321));
            var taskNextInt64Range = TestMethod<decimal>(() => RandomUtils.NextInt64(123, 321), (v) => (v >= 123) && (v < 321));
            var taskNextInt64Test = TestMethod<decimal>(() => RandomUtils.NextInt64(123, 123), (v) => v == 123);
            var taskTimeSpan = TestMethod<TimeSpan>(() => RandomUtils.NextTimeSpan(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20)), (v) => (v >= TimeSpan.FromSeconds(5)) && (v <= TimeSpan.FromSeconds(20)));
            var taskDateTime = TestMethod<DateTime>(() => RandomUtils.NextDateTime(new DateTime(2000, 1, 1), new DateTime(2020, 12, 31)), (v) => (v >= new DateTime(2000, 1, 1)) && (v <= new DateTime(2020, 12, 31)));
            var taskArray = TestMethod<byte[]>(() => RandomUtils.NextBytes(50), (v) => v.Length == 50);
            var taskArrayRange = TestMethod<byte[]>(() => RandomUtils.NextBytes(10, 50), (v) => (v.Length >= 10) && (v.Length <= 50));
            var taskArrayTest = TestMethod<byte[]>(() => RandomUtils.NextBytes(20, 20), (v) => v.Length == 20);

            await Task.WhenAll(
                taskNext,
                taskNextMax,
                taskNextRange,
                taskNextTest,
                taskNextInt32,
                taskNextInt32Max,
                taskNextInt32Range,
                taskNextInt32Test,
                taskNextInt64,
                taskNextInt64Max,
                taskNextInt64Range,
                taskNextInt64Test,
                taskTimeSpan,
                taskDateTime,
                taskArray,
                taskArrayRange);
        }

        private async Task TestMethod<T>(Func<T> getValue, Func<T, bool> checkValue)
        {
            for (int index = 0; index < TestCount; index++)
            {
                Assert.IsTrue(checkValue(getValue()));

                if ((index % 100) == 0)
                {
                    await Task.Delay(1);
                }
            }
        }
    }
}
