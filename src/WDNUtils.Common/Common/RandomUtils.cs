using System;
using System.Threading;

namespace WDNUtils.Common
{
    /// <summary>
    /// Random generator utils
    /// </summary>
    public static class RandomUtils
    {
        #region Thread-safe random number generator

        /// <summary>
        /// Thread-safe random number generator (using one instance of System.Random per thread)
        /// </summary>
        public static readonly ThreadLocal<Random> ThreadRandom = new ThreadLocal<Random>(() => new Random(GetSeed()));

        #region Random seed

        /// <summary>
        /// Creates a random seed based on managed thread ID and current date/time
        /// </summary>
        /// <returns>A random seed based on managed thread ID and current date/time</returns>
        private static int GetSeed()
        {
            // Create a 64-bits value based on the thread ID and current date/time
            var value = ((long)Int32.MaxValue - Thread.CurrentThread.ManagedThreadId) << 31;
            value += Int64.MaxValue - DateTime.Now.Ticks;

            // Fit the value into the unsigned 32-bit range
            value = Math.Abs(value) % UInt32.MaxValue;

            // Move the value to signed 32-bit range
            value += Int32.MinValue;

            // Convert to Int32 and return
            return (int)value;
        }

        #endregion

        #endregion

        #region Wrapper methods for thread-safe random number generator

        /// <summary>
        /// Returns a non-negative random integer
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than System.Int32.MaxValue</returns>
        public static int Next()
        {
            return ThreadRandom.Value.Next();
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue; that is, the range of return values ordinarily includes 0 but not maxValue. However, if maxValue equals 0, maxValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxValue is less than 0</exception>
        public static int Next(int maxValue)
        {
            return ThreadRandom.Value.Next(maxValue: maxValue);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue</exception>
        public static int Next(int minValue, int maxValue)
        {
            return ThreadRandom.Value.Next(minValue: minValue, maxValue: maxValue);
        }

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0 and less than 1.0
        /// </summary>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0</returns>
        public static double NextDouble()
        {
            return ThreadRandom.Value.NextDouble();
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers</param>
        public static void NextBytes(byte[] buffer)
        {
            ThreadRandom.Value.NextBytes(buffer: buffer);
        }

        #endregion

        #region Extended range

        /// <summary>
        /// Returns a random integer (may be negative, zero or positive)
        /// </summary>
        /// <returns>A 32-bit signed integer between System.Int32.MinValue and System.Int32.MaxValue</returns>
        public static int NextInt32()
        {
            var buffer = new byte[4];

            NextBytes(buffer);

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Returns a random integer that is less than the specified maximum
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number returned</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to System.Int32.MinValue, and less than maxValue; that is, the range of return values ordinarily includes System.Int32.MinValue but not maxValue. However, if maxValue equals System.Int32.MinValue, maxValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxValue is greater than System.Int32.MaxValue</exception>
        public static int NextInt32(long maxValue)
        {
            return NextInt32(minValue: Int32.MinValue, maxValue: maxValue);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue, minValue is lower than System.Int32.MinValue, or maxValue is greater than System.Int32.MaxValue</exception>
        public static int NextInt32(long minValue, long maxValue)
        {
            if ((minValue < Int32.MinValue) || (minValue > Int32.MaxValue))
                throw new ArgumentOutOfRangeException(nameof(minValue));

            if ((maxValue < Int32.MinValue) || (maxValue > (Int32.MaxValue + 1L)))
                throw new ArgumentOutOfRangeException(nameof(maxValue));

            if (minValue >= maxValue)
                throw new ArgumentOutOfRangeException(nameof(minValue));

            if (minValue == maxValue)
                return (int)minValue;

            var buffer = new byte[4];

            NextBytes(buffer);

            return (int)(minValue + (BitConverter.ToUInt32(buffer, 0) % (maxValue - minValue)));
        }

        /// <summary>
        /// Returns a random integer (may be negative, zero or positive)
        /// </summary>
        /// <returns>A 64-bit signed integer between System.Int64.MinValue and System.Int64.MaxValue</returns>
        public static long NextInt64()
        {
            var buffer = new byte[8];

            NextBytes(buffer);

            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Returns a random integer that is less than the specified maximum
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number returned</param>
        /// <returns>A 64-bit signed integer that is greater than or equal to System.Int64.MinValue, and less than maxValue; that is, the range of return values ordinarily includes System.Int64.MinValue but not maxValue. However, if maxValue equals System.Int64.MinValue, maxValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxValue is greater than System.Int64.MaxValue</exception>
        public static long NextInt64(decimal maxValue)
        {
            return NextInt64(minValue: Int64.MinValue, maxValue: maxValue);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned</param>
        /// <returns>A 64-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue is greater than maxValue, minValue is lower than System.Int64.MinValue, or maxValue is greater than System.Int64.MaxValue</exception>
        public static long NextInt64(decimal minValue, decimal maxValue)
        {
            if ((minValue < Int64.MinValue) || (minValue > Int64.MaxValue))
                throw new ArgumentOutOfRangeException(nameof(minValue));

            if ((maxValue < Int64.MinValue) || (maxValue > (Int64.MaxValue + 1M)))
                throw new ArgumentOutOfRangeException(nameof(minValue));

            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException(nameof(minValue));

            if (minValue == maxValue)
                return (long)minValue;

            var buffer = new byte[8];

            NextBytes(buffer);

            return (long)(minValue + (BitConverter.ToUInt64(buffer, 0)) % (maxValue - minValue));
        }

        #endregion

        #region Byte arrays

        /// <summary>
        /// Returns an array of bytes with random numbers
        /// </summary>
        /// <param name="length">Length of the array of bytes</param>
        /// <returns>An array of bytes with random numbers</returns>
        public static byte[] NextBytes(int length)
        {
            var buffer = new byte[length];

            NextBytes(buffer: buffer);

            return buffer;
        }

        /// <summary>
        /// Returns an array of bytes with random length, filled with random numbers
        /// </summary>
        /// <param name="minLength">The inclusive lower bound of the length for the array of bytes</param>
        /// <param name="maxLength">The exclusive upper bound of the length for the array of bytes</param>
        /// <returns>An array of bytes with random numbers</returns>
        public static byte[] NextBytes(int minLength, int maxLength)
        {
            var buffer = new byte[NextInt32(minLength, maxLength + 1L)];

            NextBytes(buffer: buffer);

            return buffer;
        }

        #endregion

        #region Date/time

        /// <summary>
        /// Returns a random TimeSpan value that is within a specified range
        /// </summary>
        /// <param name="minValueInclusive">The inclusive lower bound of the TimeSpan value returned</param>
        /// <param name="maxValueInclusive">The inclusive upper bound of the TimeSpan value returned (it is not an exclusive upper bound)</param>
        /// <returns>A TimeSpan value that is within a specified range</returns>
        public static TimeSpan NextTimeSpan(TimeSpan minValueInclusive, TimeSpan maxValueInclusive)
        {
            if (maxValueInclusive <= minValueInclusive)
                return minValueInclusive;

            return TimeSpan.FromTicks(NextInt64(minValueInclusive.Ticks, maxValueInclusive.Ticks + 1));
        }

        /// <summary>
        /// Returns a random DateTime value that is within a specified range
        /// </summary>
        /// <param name="minValueInclusive">The inclusive lower bound of the DateTime value returned</param>
        /// <param name="maxValueInclusive">The inclusive upper bound of the DateTime value returned (it is not an exclusive upper bound)</param>
        /// <returns>A DateTime value that is within a specified range</returns>
        public static DateTime NextDateTime(DateTime minValueInclusive, DateTime maxValueInclusive)
        {
            if (maxValueInclusive <= minValueInclusive)
                return minValueInclusive;

            return new DateTime(ticks: NextInt64(minValueInclusive.Ticks, maxValueInclusive.Ticks + 1));
        }

        #endregion
    }
}
