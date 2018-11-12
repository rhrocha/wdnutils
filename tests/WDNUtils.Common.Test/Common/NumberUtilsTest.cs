using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Threading;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class NumberUtilsTest
    {
        [TestMethod]
        public void TestNumberUtilsNumberToString()
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"de-DE");

            Assert.AreEqual(@"null", NumberUtils.ByteToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.SByteToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.Int16ToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.UInt16ToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.Int32ToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.UInt32ToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.Int64ToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.UInt64ToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.DecimalToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.SingleToStringISO(null, @"null"));
            Assert.AreEqual(@"null", NumberUtils.DoubleToStringISO(null, @"null"));

            Assert.AreEqual(@"0", NumberUtils.ByteToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.SByteToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.Int16ToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.UInt16ToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.Int32ToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.UInt32ToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.Int64ToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.UInt64ToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.DecimalToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.SingleToStringISO(0));
            Assert.AreEqual(@"0", NumberUtils.DoubleToStringISO(0));

            Assert.AreEqual(@"0", NumberUtils.ByteToStringISO(byte.MinValue));
            Assert.AreEqual(@"-128", NumberUtils.SByteToStringISO(sbyte.MinValue));
            Assert.AreEqual(@"-32768", NumberUtils.Int16ToStringISO(short.MinValue));
            Assert.AreEqual(@"0", NumberUtils.UInt16ToStringISO(ushort.MinValue));
            Assert.AreEqual(@"-2147483648", NumberUtils.Int32ToStringISO(int.MinValue));
            Assert.AreEqual(@"0", NumberUtils.UInt32ToStringISO(uint.MinValue));
            Assert.AreEqual(@"-9223372036854775808", NumberUtils.Int64ToStringISO(long.MinValue));
            Assert.AreEqual(@"0", NumberUtils.UInt64ToStringISO(ulong.MinValue));
            Assert.AreEqual(@"-79228162514264337593543950335", NumberUtils.DecimalToStringISO(decimal.MinValue));
            Assert.AreEqual(@"-7922816251426433759354395.0335", NumberUtils.DecimalToStringISO(decimal.MinValue / 10000.0m));
            Assert.AreEqual(@"-3.402823E+38", NumberUtils.SingleToStringISO(float.MinValue));
            Assert.AreEqual(@"-1.79769313486232E+308", NumberUtils.DoubleToStringISO(double.MinValue));

            Assert.AreEqual(@"255", NumberUtils.ByteToStringISO(byte.MaxValue));
            Assert.AreEqual(@"127", NumberUtils.SByteToStringISO(sbyte.MaxValue));
            Assert.AreEqual(@"32767", NumberUtils.Int16ToStringISO(short.MaxValue));
            Assert.AreEqual(@"65535", NumberUtils.UInt16ToStringISO(ushort.MaxValue));
            Assert.AreEqual(@"2147483647", NumberUtils.Int32ToStringISO(int.MaxValue));
            Assert.AreEqual(@"4294967295", NumberUtils.UInt32ToStringISO(uint.MaxValue));
            Assert.AreEqual(@"9223372036854775807", NumberUtils.Int64ToStringISO(long.MaxValue));
            Assert.AreEqual(@"18446744073709551615", NumberUtils.UInt64ToStringISO(ulong.MaxValue));
            Assert.AreEqual(@"79228162514264337593543950335", NumberUtils.DecimalToStringISO(decimal.MaxValue));
            Assert.AreEqual(@"7922816251426433759354395.0335", NumberUtils.DecimalToStringISO(decimal.MaxValue / 10000.0m));
            Assert.AreEqual(@"3.402823E+38", NumberUtils.SingleToStringISO(float.MaxValue));
            Assert.AreEqual(@"1.79769313486232E+308", NumberUtils.DoubleToStringISO(double.MaxValue));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        [TestMethod]
        public void TestNumberUtilsStringToNumber()
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"de-DE");

            Assert.AreEqual(null, NumberUtils.ToByteISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToSByteISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToInt16ISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToUInt16ISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToInt32ISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToUInt32ISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToInt64ISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToUInt64ISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToDecimalISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToSingleISO(@"null", @"null"));
            Assert.AreEqual(null, NumberUtils.ToDoubleISO(@"null", @"null"));

            Assert.AreEqual((byte)0, NumberUtils.ToByteISO(@"0"));
            Assert.AreEqual((sbyte)0, NumberUtils.ToSByteISO(@"0"));
            Assert.AreEqual((short)0, NumberUtils.ToInt16ISO(@"0"));
            Assert.AreEqual((ushort)0, NumberUtils.ToUInt16ISO(@"0"));
            Assert.AreEqual((int)0, NumberUtils.ToInt32ISO(@"0"));
            Assert.AreEqual((uint)0, NumberUtils.ToUInt32ISO(@"0"));
            Assert.AreEqual((long)0, NumberUtils.ToInt64ISO(@"0"));
            Assert.AreEqual((ulong)0, NumberUtils.ToUInt64ISO(@"0"));
            Assert.AreEqual((decimal)0, NumberUtils.ToDecimalISO(@"0"));
            Assert.AreEqual((float)0, NumberUtils.ToSingleISO(@"0"));
            Assert.AreEqual((double)0, NumberUtils.ToDoubleISO(@"0"));

            Assert.AreEqual(byte.MinValue, NumberUtils.ToByteISO(@"0"));
            Assert.AreEqual(sbyte.MinValue, NumberUtils.ToSByteISO(@"-128"));
            Assert.AreEqual(short.MinValue, NumberUtils.ToInt16ISO(@"-32768"));
            Assert.AreEqual(ushort.MinValue, NumberUtils.ToUInt16ISO(@"0"));
            Assert.AreEqual(int.MinValue, NumberUtils.ToInt32ISO(@"-2147483648"));
            Assert.AreEqual(uint.MinValue, NumberUtils.ToUInt32ISO(@"0"));
            Assert.AreEqual(long.MinValue, NumberUtils.ToInt64ISO(@"-9223372036854775808"));
            Assert.AreEqual(ulong.MinValue, NumberUtils.ToUInt64ISO(@"0"));
            Assert.AreEqual(decimal.MinValue, NumberUtils.ToDecimalISO(@"-79228162514264337593543950335"));
            Assert.AreEqual(decimal.MinValue / 10000.0m, NumberUtils.ToDecimalISO(@"-7922816251426433759354395.0335"));
            Assert.AreEqual(float.MinValue, NumberUtils.ToSingleISO(@"-3.40282347E+38"));
            Assert.AreEqual(double.MinValue, NumberUtils.ToDoubleISO(@"-1.7976931348623157E+308"));

            Assert.AreEqual(byte.MaxValue, NumberUtils.ToByteISO(@"255"));
            Assert.AreEqual(sbyte.MaxValue, NumberUtils.ToSByteISO(@"127"));
            Assert.AreEqual(short.MaxValue, NumberUtils.ToInt16ISO(@"32767"));
            Assert.AreEqual(ushort.MaxValue, NumberUtils.ToUInt16ISO(@"65535"));
            Assert.AreEqual(int.MaxValue, NumberUtils.ToInt32ISO(@"2147483647"));
            Assert.AreEqual(uint.MaxValue, NumberUtils.ToUInt32ISO(@"4294967295"));
            Assert.AreEqual(long.MaxValue, NumberUtils.ToInt64ISO(@"9223372036854775807"));
            Assert.AreEqual(ulong.MaxValue, NumberUtils.ToUInt64ISO(@"18446744073709551615"));
            Assert.AreEqual(decimal.MaxValue, NumberUtils.ToDecimalISO(@"79228162514264337593543950335"));
            Assert.AreEqual(decimal.MaxValue / 10000.0m, NumberUtils.ToDecimalISO(@"7922816251426433759354395.0335"));
            Assert.AreEqual(float.MaxValue, NumberUtils.ToSingleISO(@"3.40282347E+38"));
            Assert.AreEqual(double.MaxValue, NumberUtils.ToDoubleISO(@"1.7976931348623157E+308"));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        [TestMethod]
        public void TestNumberUtilsParseHex()
        {
            ulong result;

            // Zero

            Assert.AreEqual(0ul, NumberUtils.ParseHex(@"0"));
            Assert.IsTrue(NumberUtils.TryParseHex(@"0", out result));
            Assert.AreEqual(0ul, result);

            // Substrings

            Assert.AreEqual(0xAABBCCul, NumberUtils.ParseHex(@"AABBCC"));
            Assert.IsTrue(NumberUtils.TryParseHex(@"AABBCC", out result));
            Assert.AreEqual(0xAABBCCul, result);

            Assert.AreEqual(0xAAul, NumberUtils.ParseHex(@"AABBCC", 0, 2));
            Assert.IsTrue(NumberUtils.TryParseHex(@"AABBCC", 0, 2, out result));
            Assert.AreEqual(0xAAul, result);

            Assert.AreEqual(0xAABBul, NumberUtils.ParseHex(@"AABBCC", 0, 4));
            Assert.IsTrue(NumberUtils.TryParseHex(@"AABBCC", 0, 4, out result));
            Assert.AreEqual(0xAABBul, result);

            Assert.AreEqual(0xBBCCul, NumberUtils.ParseHex(@"AABBCC", 2, 4));
            Assert.IsTrue(NumberUtils.TryParseHex(@"AABBCC", 2, 4, out result));
            Assert.AreEqual(0xBBCCul, result);

            Assert.AreEqual(0xCCul, NumberUtils.ParseHex(@"AABBCC", 4, 2));
            Assert.IsTrue(NumberUtils.TryParseHex(@"AABBCC", 4, 2, out result));
            Assert.AreEqual(0xCCul, result);

            // Case insensitive

            Assert.AreEqual(0x0123456789ABCDEFul, NumberUtils.ParseHex(@"0123456789ABCDEF"));
            Assert.IsTrue(NumberUtils.TryParseHex(@"0123456789ABCDEF", out result));
            Assert.AreEqual(0x0123456789ABCDEFul, result);

            Assert.AreEqual(0x0123456789ABCDEFul, NumberUtils.ParseHex(@"0123456789abcdef"));
            Assert.IsTrue(NumberUtils.TryParseHex(@"0123456789abcdef", out result));
            Assert.AreEqual(0x0123456789ABCDEFul, result);

            // Ignore leading zeros

            Assert.AreEqual(0x0123456789ABCDEFul, NumberUtils.ParseHex(@"000000123456789ABCDEF"));
            Assert.IsTrue(NumberUtils.TryParseHex(@"000000123456789ABCDEF", out result));
            Assert.AreEqual(0x0123456789ABCDEFul, result);

            // NULL

            Assert.IsFalse(NumberUtils.TryParseHex(null, out result));

            try
            {
                Assert.IsFalse(NumberUtils.TryParseHex(null, 1, 6, out result));
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.ParseHex(null);
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.ParseHex(null, 1, 6);
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            // Empty

            Assert.IsFalse(NumberUtils.TryParseHex(string.Empty, out result));

            try
            {
                Assert.IsFalse(NumberUtils.TryParseHex(string.Empty, 1, 6, out result));
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.ParseHex(string.Empty);
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.ParseHex(string.Empty, 1, 6);
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            // Invalid start index

            try
            {
                NumberUtils.ParseHex(@"AABBCC", -1, 6);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.TryParseHex(@"AABBCC", -1, 6, out result);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            // Invalid length

            try
            {
                NumberUtils.ParseHex(@"AABBCC", 0, 7);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.TryParseHex(@"AABBCC", 0, 7, out result);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            // Invalid last index

            try
            {
                NumberUtils.ParseHex(@"AABBCC", 2, 5);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.TryParseHex(@"AABBCC", 2, 5, out result);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            // Invalid string size

            try
            {
                NumberUtils.ParseHex(@"12345678901234567");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.ParseHex(@"12345678901234567", 0, 17);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            Assert.IsFalse(NumberUtils.TryParseHex(@"12345678901234567", out result));
            Assert.IsFalse(NumberUtils.TryParseHex(@"12345678901234567", 0, 17, out result));
        }

        [TestMethod]
        public void TestNumberUtilsBitOperations()
        {
            Assert.AreEqual(true, byte.MinValue.HasBits(byte.MinValue));
            Assert.AreEqual(true, byte.MaxValue.HasBits(byte.MaxValue));
            Assert.AreEqual(true, sbyte.MinValue.HasBits(sbyte.MinValue));
            Assert.AreEqual(true, sbyte.MaxValue.HasBits(sbyte.MaxValue));
            Assert.AreEqual(true, ushort.MinValue.HasBits(ushort.MinValue));
            Assert.AreEqual(true, ushort.MaxValue.HasBits(ushort.MaxValue));
            Assert.AreEqual(true, short.MinValue.HasBits(short.MinValue));
            Assert.AreEqual(true, short.MaxValue.HasBits(short.MaxValue));
            Assert.AreEqual(true, uint.MinValue.HasBits(uint.MinValue));
            Assert.AreEqual(true, uint.MaxValue.HasBits(uint.MaxValue));
            Assert.AreEqual(true, int.MinValue.HasBits(int.MinValue));
            Assert.AreEqual(true, int.MaxValue.HasBits(int.MaxValue));
            Assert.AreEqual(true, ulong.MinValue.HasBits(ulong.MinValue));
            Assert.AreEqual(true, ulong.MaxValue.HasBits(ulong.MaxValue));
            Assert.AreEqual(true, long.MinValue.HasBits(long.MinValue));
            Assert.AreEqual(true, long.MaxValue.HasBits(long.MaxValue));

            Assert.AreEqual((byte)0, byte.MinValue.ClearBits(byte.MinValue));
            Assert.AreEqual((byte)0, byte.MaxValue.ClearBits(byte.MaxValue));
            Assert.AreEqual((sbyte)0, sbyte.MinValue.ClearBits(sbyte.MinValue));
            Assert.AreEqual((sbyte)0, sbyte.MaxValue.ClearBits(sbyte.MaxValue));
            Assert.AreEqual((ushort)0, ushort.MinValue.ClearBits(ushort.MinValue));
            Assert.AreEqual((ushort)0, ushort.MaxValue.ClearBits(ushort.MaxValue));
            Assert.AreEqual((short)0, short.MinValue.ClearBits(short.MinValue));
            Assert.AreEqual((short)0, short.MaxValue.ClearBits(short.MaxValue));
            Assert.AreEqual((uint)0, uint.MinValue.ClearBits(uint.MinValue));
            Assert.AreEqual((uint)0, uint.MaxValue.ClearBits(uint.MaxValue));
            Assert.AreEqual((int)0, int.MinValue.ClearBits(int.MinValue));
            Assert.AreEqual((int)0, int.MaxValue.ClearBits(int.MaxValue));
            Assert.AreEqual((ulong)0, ulong.MinValue.ClearBits(ulong.MinValue));
            Assert.AreEqual((ulong)0, ulong.MaxValue.ClearBits(ulong.MaxValue));
            Assert.AreEqual((long)0, long.MinValue.ClearBits(long.MinValue));
            Assert.AreEqual((long)0, long.MaxValue.ClearBits(long.MaxValue));

            Assert.AreEqual(byte.MinValue, ((byte)0).SetBits(byte.MinValue));
            Assert.AreEqual(byte.MaxValue, ((byte)0).SetBits(byte.MaxValue));
            Assert.AreEqual(sbyte.MinValue, ((sbyte)0).SetBits(sbyte.MinValue));
            Assert.AreEqual(sbyte.MaxValue, ((sbyte)0).SetBits(sbyte.MaxValue));
            Assert.AreEqual(ushort.MinValue, ((ushort)0).SetBits(ushort.MinValue));
            Assert.AreEqual(ushort.MaxValue, ((ushort)0).SetBits(ushort.MaxValue));
            Assert.AreEqual(short.MinValue, ((short)0).SetBits(short.MinValue));
            Assert.AreEqual(short.MaxValue, ((short)0).SetBits(short.MaxValue));
            Assert.AreEqual(uint.MinValue, ((uint)0).SetBits(uint.MinValue));
            Assert.AreEqual(uint.MaxValue, ((uint)0).SetBits(uint.MaxValue));
            Assert.AreEqual(int.MinValue, ((int)0).SetBits(int.MinValue));
            Assert.AreEqual(int.MaxValue, ((int)0).SetBits(int.MaxValue));
            Assert.AreEqual(ulong.MinValue, ((ulong)0).SetBits(ulong.MinValue));
            Assert.AreEqual(ulong.MaxValue, ((ulong)0).SetBits(ulong.MaxValue));
            Assert.AreEqual(long.MinValue, ((long)0).SetBits(long.MinValue));
            Assert.AreEqual(long.MaxValue, ((long)0).SetBits(long.MaxValue));

            for (int index = sbyte.MinValue; index <= sbyte.MaxValue; index++)
            {
                sbyte value = (sbyte)index;

                TestHasBits(value, value, true);
                TestSetBits(0, value, value);
                TestClearBits(value, value, 0);
            }

            TestHasBits(0b0000000, 0b0000000, true);
            TestHasBits(0b0000000, 0b0101010, false);
            TestHasBits(0b0000000, 0b1010101, false);
            TestHasBits(0b0000000, 0b1111111, false);

            TestHasBits(0b0101010, 0b0000000, true);
            TestHasBits(0b0101010, 0b0101010, true);
            TestHasBits(0b0101010, 0b1010101, false);
            TestHasBits(0b0101010, 0b1111111, false);

            TestHasBits(0b1010101, 0b0000000, true);
            TestHasBits(0b1010101, 0b0101010, false);
            TestHasBits(0b1010101, 0b1010101, true);
            TestHasBits(0b1010101, 0b1111111, false);

            TestHasBits(0b1111111, 0b0000000, true);
            TestHasBits(0b1111111, 0b0101010, true);
            TestHasBits(0b1111111, 0b1010101, true);
            TestHasBits(0b1111111, 0b1111111, true);

            TestSetBits(0b0000000, 0b0000000, 0b0000000);
            TestSetBits(0b0000000, 0b0101010, 0b0101010);
            TestSetBits(0b0000000, 0b1010101, 0b1010101);
            TestSetBits(0b0000000, 0b1111111, 0b1111111);

            TestSetBits(0b0101010, 0b0000000, 0b0101010);
            TestSetBits(0b0101010, 0b0101010, 0b0101010);
            TestSetBits(0b0101010, 0b1010101, 0b1111111);
            TestSetBits(0b0101010, 0b1111111, 0b1111111);

            TestSetBits(0b1010101, 0b0000000, 0b1010101);
            TestSetBits(0b1010101, 0b0101010, 0b1111111);
            TestSetBits(0b1010101, 0b1010101, 0b1010101);
            TestSetBits(0b1010101, 0b1111111, 0b1111111);

            TestSetBits(0b1111111, 0b0000000, 0b1111111);
            TestSetBits(0b1111111, 0b1010101, 0b1111111);
            TestSetBits(0b1111111, 0b0101010, 0b1111111);
            TestSetBits(0b1111111, 0b1111111, 0b1111111);

            TestClearBits(0b0000000, 0b0000000, 0b0000000);
            TestClearBits(0b0000000, 0b0101010, 0b0000000);
            TestClearBits(0b0000000, 0b1010101, 0b0000000);
            TestClearBits(0b0000000, 0b1111111, 0b0000000);

            TestClearBits(0b0101010, 0b0000000, 0b0101010);
            TestClearBits(0b0101010, 0b0101010, 0b0000000);
            TestClearBits(0b0101010, 0b1010101, 0b0101010);
            TestClearBits(0b0101010, 0b1111111, 0b0000000);

            TestClearBits(0b1010101, 0b0000000, 0b1010101);
            TestClearBits(0b1010101, 0b0101010, 0b1010101);
            TestClearBits(0b1010101, 0b1010101, 0b0000000);
            TestClearBits(0b1010101, 0b1111111, 0b0000000);

            TestClearBits(0b1111111, 0b0000000, 0b1111111);
            TestClearBits(0b1111111, 0b0101010, 0b1010101);
            TestClearBits(0b1111111, 0b1010101, 0b0101010);
            TestClearBits(0b1111111, 0b1111111, 0b0000000);

            void TestHasBits(sbyte value, sbyte bitMask, bool result)
            {
                if ((value >= 0) && (bitMask >= 0))
                {
                    Assert.AreEqual(result, checked((byte)value).HasBits((byte)bitMask));
                    Assert.AreEqual(result, checked((ushort)value).HasBits((ushort)bitMask));
                    Assert.AreEqual(result, checked((uint)value).HasBits((uint)bitMask));
                    Assert.AreEqual(result, checked((ulong)value).HasBits((ulong)bitMask));
                }

                Assert.AreEqual(result, checked((sbyte)value).HasBits((sbyte)bitMask));
                Assert.AreEqual(result, checked((short)value).HasBits((short)bitMask));
                Assert.AreEqual(result, checked((int)value).HasBits((int)bitMask));
                Assert.AreEqual(result, checked((long)value).HasBits((long)bitMask));
            }

            void TestSetBits(sbyte value, sbyte bitMask, decimal result)
            {
                if ((value >= 0) && (bitMask >= 0))
                {
                    Assert.AreEqual(result, checked((byte)value).SetBits((byte)bitMask));
                    Assert.AreEqual(result, checked((ushort)value).SetBits((ushort)bitMask));
                    Assert.AreEqual(result, checked((uint)value).SetBits((uint)bitMask));
                    Assert.AreEqual(result, checked((ulong)value).SetBits((ulong)bitMask));
                }

                Assert.AreEqual(result, checked((sbyte)value).SetBits((sbyte)bitMask));
                Assert.AreEqual(result, checked((short)value).SetBits((short)bitMask));
                Assert.AreEqual(result, checked((int)value).SetBits((int)bitMask));
                Assert.AreEqual(result, checked((long)value).SetBits((long)bitMask));
            }

            void TestClearBits(sbyte value, sbyte bitMask, decimal result)
            {
                if ((value >= 0) && (bitMask >= 0))
                {
                    Assert.AreEqual(result, checked((byte)value).ClearBits((byte)bitMask));
                    Assert.AreEqual(result, checked((ushort)value).ClearBits((ushort)bitMask));
                    Assert.AreEqual(result, checked((uint)value).ClearBits((uint)bitMask));
                    Assert.AreEqual(result, checked((ulong)value).ClearBits((ulong)bitMask));
                }

                Assert.AreEqual(result, checked((sbyte)value).ClearBits((sbyte)bitMask));
                Assert.AreEqual(result, checked((short)value).ClearBits((short)bitMask));
                Assert.AreEqual(result, checked((int)value).ClearBits((int)bitMask));
                Assert.AreEqual(result, checked((long)value).ClearBits((long)bitMask));
            }
        }
    }
}
