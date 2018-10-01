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

            // NULL

            Assert.IsFalse(NumberUtils.TryParseHex(null, out result));
            Assert.IsFalse(NumberUtils.TryParseHex(null, 1, 6, out result));

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
            Assert.IsFalse(NumberUtils.TryParseHex(string.Empty, 1, 6, out result));

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
                NumberUtils.ParseHex(@"01234567890123456");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            try
            {
                NumberUtils.ParseHex(@"01234567890123456", 0, 17);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                // OK
            }

            Assert.IsFalse(NumberUtils.TryParseHex(@"01234567890123456", out result));
            Assert.IsFalse(NumberUtils.TryParseHex(@"01234567890123456", 0, 17, out result));
        }
    }
}
