using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Text;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class TextUtilsTest
    {
        [TestMethod]
        public void TestTextUtils()
        {
            Assert.AreEqual($@"{Environment.NewLine}{Environment.NewLine}", TextUtils.DoubleNewLine);

            Assert.AreEqual(@"aeiouy aeiouy aeiou aeiou ao cn AEIOUY AEIOUY AEIOU AEIOU AO CN", TextUtils.RemoveDiacritics(@"áéíóúý äëïöüÿ àèìòù âêîôû ãõ çñ ÁÉÍÓÚÝ ÄËÏÖÜŸ ÀÈÌÒÙ ÂÊÎÔÛ ÃÕ ÇÑ"));

            Assert.AreEqual(@"NC*123", TextUtils.BuildSearchFilter(@" ñç  % 123  ", false, '*'));
            Assert.AreEqual(@"NC*123*", TextUtils.BuildSearchFilter(@" ñç .*  123 ??? ", false, '*'));
            Assert.AreEqual(@"*NC*123", TextUtils.BuildSearchFilter(@" %  ñç ? 123 ", false, '*'));

            Assert.AreEqual(@"*NC*123*", TextUtils.BuildSearchFilter(@" ñç  % 123  ", true, '*'));
            Assert.AreEqual(@"*NC*123*", TextUtils.BuildSearchFilter(@" ñç .*  123 ??? ", true, '*'));
            Assert.AreEqual(@"*NC*123*", TextUtils.BuildSearchFilter(@" %  ñç ? 123 ", true, '*'));
        }

        [TestMethod]
        public void TestTextUtilsSubstring()
        {
            Assert.AreEqual(@"0123456789", @"0123456789".SubstringSafe(0));
            Assert.AreEqual(@"56789", @"0123456789".SubstringSafe(5));
            Assert.AreEqual(@"9", @"0123456789".SubstringSafe(9));
            Assert.AreEqual(@"", @"0123456789".SubstringSafe(10));
            Assert.AreEqual(@"", @"0123456789".SubstringSafe(9999));

            Assert.AreEqual(@"0123456789", @"0123456789".SubstringSafe(0, 10));
            Assert.AreEqual(@"0123456789", @"0123456789".SubstringSafe(0, 9999));
            Assert.AreEqual(@"01234", @"0123456789".SubstringSafe(0, 5));
            Assert.AreEqual(@"567", @"0123456789".SubstringSafe(5, 3));
            Assert.AreEqual(@"56789", @"0123456789".SubstringSafe(5, 5));
            Assert.AreEqual(@"56789", @"0123456789".SubstringSafe(5, 9999));
            Assert.AreEqual(@"9", @"0123456789".SubstringSafe(9, 1));
            Assert.AreEqual(@"9", @"0123456789".SubstringSafe(9, 9999));
            Assert.AreEqual(@"", @"0123456789".SubstringSafe(10, 1));
            Assert.AreEqual(@"", @"0123456789".SubstringSafe(10, 9999));
            Assert.AreEqual(@"", @"0123456789".SubstringSafe(9999, 1));
            Assert.AreEqual(@"", @"0123456789".SubstringSafe(9999, 9999));
        }

        [TestMethod]
        public void TestTextUtilsEncoding()
        {
            var value = $"\n \r \t 0 a A Ñ Ç £ ¢";

            Assert.IsFalse(TextUtils.CanEncode(Encoding.ASCII, value));
            Assert.IsTrue(TextUtils.CanEncode(Encoding.UTF8, value));

            var escapedValue = TextUtils.UnicodeToEscapedAscii(value);

            Assert.AreEqual(@"\u000a \u000d \u0009 0 a A \u00d1 \u00c7 \u00a3 \u00a2", escapedValue);

            Assert.IsTrue(TextUtils.CanEncode(Encoding.ASCII, escapedValue));
            Assert.IsTrue(TextUtils.CanEncode(Encoding.UTF8, escapedValue));

            Assert.AreEqual(value, TextUtils.EscapedAsciiToUnicode(escapedValue));
        }

        [TestMethod]
        public void TestTextUtilsStringBuilder()
        {
            CheckTrim(null, true, true);
            CheckTrim(null, true, false);
            CheckTrim(null, false, true);

            CheckTrim(string.Empty, true, true);
            CheckTrim(string.Empty, true, false);
            CheckTrim(string.Empty, false, true);

            CheckTrim(@"   ", true, true);
            CheckTrim(@"   ", true, false);
            CheckTrim(@"   ", false, true);

            CheckTrim("\t TEST 123 \n ", true, true);
            CheckTrim("\t TEST 123 \n ", true, false);
            CheckTrim("\t TEST 123 \n ", false, true);

            CheckTrim("\t TEST 123 \n ", true, true, '\t');
            CheckTrim("\t TEST 123 \n ", true, false, '\t');
            CheckTrim("\t TEST 123 \n ", false, true, '\t');

            CheckTrim("\t TEST 123 \n ", true, true, '\t', ' ');
            CheckTrim("\t TEST 123 \n ", true, false, '\t', ' ');
            CheckTrim("\t TEST 123 \n ", false, true, '\t', ' ');

            CheckTrim("\t TEST 123 \n ", true, true, '\n', ' ');
            CheckTrim("\t TEST 123 \n ", true, false, '\n', ' ');
            CheckTrim("\t TEST 123 \n ", false, true, '\n', ' ');

            CheckTrim("\t TEST 123 \n ", true, true, '\n', '\t', ' ');
            CheckTrim("\t TEST 123 \n ", true, false, '\n', '\t', ' ');
            CheckTrim("\t TEST 123 \n ", false, true, '\n', '\t', ' ');

            void CheckTrim(string value, bool trimStart, bool trimEnd, params char[] trimChars)
            {
                var v1 = value;
                var v2 = value is null ? null : new StringBuilder(value);

                if ((trimStart) && (trimEnd))
                {
                    v1 = (v1 is null) ? null : (trimChars.Length > 1) ? v1.Trim(trimChars) : (trimChars.Length == 1) ? v1.Trim(trimChars[0]) : v1.Trim();
                    v2 = (trimChars.Length > 1) ? v2.Trim(trimChars) : (trimChars.Length == 1) ? v2.Trim(trimChars[0]) : v2.Trim();
                }
                else if (trimStart)
                {
                    v1 = (v1 is null) ? null : (trimChars.Length > 1) ? v1.TrimStart(trimChars) : (trimChars.Length == 1) ? v1.TrimStart(trimChars[0]) : v1.TrimStart();
                    v2 = (trimChars.Length > 1) ? v2.TrimStart(trimChars) : (trimChars.Length == 1) ? v2.TrimStart(trimChars[0]) : v2.TrimStart();
                }
                else if (trimEnd)
                {
                    v1 = (v1 is null) ? null : (trimChars.Length > 1) ? v1.TrimEnd(trimChars) : (trimChars.Length == 1) ? v1.TrimEnd(trimChars[0]) : v1.TrimEnd();
                    v2 = (trimChars.Length > 1) ? v2.TrimEnd(trimChars) : (trimChars.Length == 1) ? v2.TrimEnd(trimChars[0]) : v2.TrimEnd();
                }
                else
                    throw new InvalidOperationException();

                Assert.AreEqual(v1, v2?.ToString());
            }
        }

        [TestMethod]
        public void TestTextUtilsBase64()
        {
            for (int index = 0; index < 100; index++)
            {
                var input = (index == 0) ? null : RandomUtils.NextBytes(0, (index > 1) ? 200 : 0);

                var base64 = input.ToBase64();

                Assert.AreEqual((input is null) ? null : Convert.ToBase64String(input), base64);

                var output = base64.FromBase64();

                Assert.IsTrue((input is null) ? (output is null) : Enumerable.SequenceEqual(input, output));
            }
        }
    }
}
