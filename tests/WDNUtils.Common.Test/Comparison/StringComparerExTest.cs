using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class StringComparerExTest
    {
        [TestMethod]
        public void TestStringComparerEx()
        {
            var comparisonTypeList = (StringComparisonEx[])Enum.GetValues(typeof(StringComparisonEx));

            foreach (var comparisonType in comparisonTypeList)
            {
                // Null equality
                TestEqual(v1: null, v2: null, comparisonType: comparisonType, nullIsLower: null, isNull: null);

                // Empty equality
                TestEqual(v1: string.Empty, v2: string.Empty, comparisonType: comparisonType, nullIsLower: null, isNull: null);

                // Empty and text comparison
                TestBigger(v1: nameof(Test), v2: string.Empty, comparisonType: comparisonType, nullIsLower: null, isNull: null);

                // Custom null sort order
                TestLower(v1: string.Empty, v2: null, comparisonType: comparisonType, nullIsLower: false, isNull: null);
                TestBigger(v1: string.Empty, v2: null, comparisonType: comparisonType, nullIsLower: true, isNull: null);

                // Custom null values
                TestLower(v1: null, v2: string.Empty, comparisonType: comparisonType, nullIsLower: true, isNull: null);
                TestEqual(v1: null, v2: string.Empty, comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrEmpty(v));
                TestLower(v1: null, v2: " ", comparisonType: comparisonType, nullIsLower: true, isNull: null);
                TestLower(v1: null, v2: " ", comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrEmpty(v));
                TestEqual(v1: null, v2: " ", comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrWhiteSpace(v));
                TestLower(v1: string.Empty, v2: " ", comparisonType: comparisonType, nullIsLower: true, isNull: null);
                TestLower(v1: string.Empty, v2: " ", comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrEmpty(v));
                TestEqual(v1: string.Empty, v2: " ", comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrWhiteSpace(v));
                TestLower(v1: " ", v2: "  ", comparisonType: comparisonType, nullIsLower: true, isNull: null);
                TestLower(v1: " ", v2: "  ", comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrEmpty(v));
                TestEqual(v1: " ", v2: "  ", comparisonType: comparisonType, nullIsLower: true, isNull: (v) => string.IsNullOrWhiteSpace(v));
            }

            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("da-DK");

            // Ordinal (binary): no char conversion, upper case first, ASCII characters first
            // Culture da-DK: "ae" != "æ", "ß" == "ss"
            // Invariant culture: "ae" == "æ", "ß" == "ss"

#pragma warning disable CS0618 // Type or member is obsolete
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.CurrentCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.CurrentCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.InvariantCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.InvariantCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.OrdinalIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.Ordinal, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ABC", v2: "ABC", comparisonType: StringComparisonEx.Natural, nullIsLower: null, isNull: null);

            TestEqual(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.CurrentCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.CurrentCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.InvariantCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.InvariantCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.OrdinalIgnoreCase, nullIsLower: null, isNull: null);
            TestBigger(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.Ordinal, nullIsLower: null, isNull: null);
            TestEqual(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "abc", v2: "ABC", comparisonType: StringComparisonEx.Natural, nullIsLower: null, isNull: null);

            TestLower(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.CurrentCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.CurrentCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.InvariantCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestBigger(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.InvariantCulture, nullIsLower: null, isNull: null);
            TestLower(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.OrdinalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.Ordinal, nullIsLower: null, isNull: null);
            TestLower(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "AE", v2: "æ", comparisonType: StringComparisonEx.Natural, nullIsLower: null, isNull: null);

            TestLower(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.CurrentCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.CurrentCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.InvariantCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.InvariantCulture, nullIsLower: null, isNull: null);
            TestLower(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.OrdinalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.Ordinal, nullIsLower: null, isNull: null);
            TestLower(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "ae", v2: "æ", comparisonType: StringComparisonEx.Natural, nullIsLower: null, isNull: null);

            TestEqual(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.CurrentCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestBigger(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.CurrentCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.InvariantCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestBigger(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.InvariantCulture, nullIsLower: null, isNull: null);
            TestLower(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.OrdinalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.Ordinal, nullIsLower: null, isNull: null);
            TestEqual(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: null, isNull: null);
            TestBigger(v1: "SS", v2: "ß", comparisonType: StringComparisonEx.Natural, nullIsLower: null, isNull: null);

            TestEqual(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.CurrentCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.CurrentCulture, nullIsLower: null, isNull: null);
            TestEqual(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.InvariantCultureIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.InvariantCulture, nullIsLower: null, isNull: null);
            TestLower(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.OrdinalIgnoreCase, nullIsLower: null, isNull: null);
            TestLower(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.Ordinal, nullIsLower: null, isNull: null);
            TestEqual(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: null, isNull: null);
            TestEqual(v1: "ss", v2: "ß", comparisonType: StringComparisonEx.Natural, nullIsLower: null, isNull: null);

            // List sorting using regular comparison

            comparisonTypeList = new StringComparisonEx[] { StringComparisonEx.CurrentCultureIgnoreCase, StringComparisonEx.InvariantCultureIgnoreCase, StringComparisonEx.OrdinalIgnoreCase };

#pragma warning restore CS0618 // Type or member is obsolete

            foreach (var comparisonType in comparisonTypeList)
            {
                // Code value list sorting, with null at beginning
                TestListSort(comparisonType: comparisonType, nullIsLower: true, values: new string[]
                {
                    null,
                    null,
                    @"00050",
                    @"001",
                    @"0040",
                    @"01",
                    @"010X",
                    @"030",
                    @"05B",
                    @"10",
                    @"20",
                    @"5A"
                });

                // File list sorting, with null at end
                TestListSort(comparisonType: comparisonType, nullIsLower: false, values: new string[]
                {
                    @"TEST FILE 01.TXT",
                    @"Test File 01.txt.00001",
                    @"TEST FILE 01.TXT.00010",
                    @"Test File 01.txt.00100",
                    @"TEST FILE 01.TXT.01000",
                    @"Test File 01.txt.10000",
                    @"TEST FILE 1.TXT",
                    @"Test File 1.txt.1",
                    @"TEST FILE 1.TXT.10",
                    @"Test File 1.txt.100",
                    @"TEST FILE 1.TXT.1000",
                    @"Test File 10.txt",
                    @"TEST FILE 10.TXT.000",
                    @"Test File 10.txt.1",
                    @"Test File 10.txt.10",
                    @"TEST FILE 10.TXT.15",
                    @"Test File 10.txt.20",
                    @"TEST FILE 10.TXT.25",
                    @"TEST FILE 10.TXT.5",
                    null,
                    null,
                    null
                });
            }

            // Natural sort order - same of regular comparison
            TestLower("---123---01---", "---123---10---", comparisonType: StringComparisonEx.Natural);
            TestLower("---123---01---", "---123---10---", comparisonType: StringComparisonEx.NaturalIgnoreCase);

            // Natural sort order - digit group comparison
            TestLower("---123---1---", "---123---10---", comparisonType: StringComparisonEx.Natural);
            TestLower("---123---1---", "---123---10---", comparisonType: StringComparisonEx.NaturalIgnoreCase);

            // Leading zero digit precedence
            TestLower("---123---01---", "---0123---1---", comparisonType: StringComparisonEx.Natural);
            TestLower("---123---01---", "---0123---1---", comparisonType: StringComparisonEx.NaturalIgnoreCase);

            // List sorting using natural comparison

            // Code value list sorting, with null at beginning
            TestListSort(comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: true, values: new string[]
            {
                null,
                null,
                @"01",
                @"001",
                @"5A",
                @"05B",
                @"10",
                @"010X",
                @"20",
                @"030",
                @"0040",
                @"00050"
            });

            // File list sorting, with null at end
            TestListSort(comparisonType: StringComparisonEx.NaturalIgnoreCase, nullIsLower: false, values: new string[]
            {
                @"TEST FILE 1.TXT",
                @"TEST FILE 01.TXT",
                @"Test File 1.txt.1",
                @"Test File 01.txt.00001",
                @"TEST FILE 1.TXT.10",
                @"TEST FILE 01.TXT.00010",
                @"Test File 1.txt.100",
                @"Test File 01.txt.00100",
                @"TEST FILE 1.TXT.1000",
                @"TEST FILE 01.TXT.01000",
                @"Test File 01.txt.10000",
                @"Test File 10.txt",
                @"TEST FILE 10.TXT.000",
                @"Test File 10.txt.1",
                @"TEST FILE 10.TXT.5",
                @"Test File 10.txt.10",
                @"TEST FILE 10.TXT.15",
                @"Test File 10.txt.20",
                @"TEST FILE 10.TXT.25",
                null,
                null,
                null
            });

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        #region Internal methods

        private void TestEqual(string v1, string v2, StringComparisonEx comparisonType, bool? nullIsLower = null, Func<string, bool> isNull = null)
        {
            var message = TestCompareInternal(result: 0, v1: v1, v2: v2, comparisonType: comparisonType, nullIsLower: nullIsLower, isNull: isNull);

            if (message != null)
                Assert.Fail(message);
        }

        private void TestBigger(string v1, string v2, StringComparisonEx comparisonType, bool? nullIsLower = null, Func<string, bool> isNull = null)
        {
            var message = TestCompareInternal(result: 1, v1: v1, v2: v2, comparisonType: comparisonType, nullIsLower: nullIsLower, isNull: isNull);

            if (message != null)
                Assert.Fail(message);
        }

        private void TestLower(string v1, string v2, StringComparisonEx comparisonType, bool? nullIsLower = null, Func<string, bool> isNull = null)
        {
            var message = TestCompareInternal(result: -1, v1: v1, v2: v2, comparisonType: comparisonType, nullIsLower: nullIsLower, isNull: isNull);

            if (message != null)
                Assert.Fail(message);
        }

        private void TestListSort(StringComparisonEx comparisonType, bool nullIsLower, params string[] values)
        {
            var valueList = new List<string>(values);

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

            valueList.Sort(new StringComparerEx(comparisonType: comparisonType, nullIsLower: nullIsLower));

            for (int index = 0; index < values.Length; index++)
            {
                if (!string.Equals(values[index], valueList[index], StringComparison.Ordinal))
                {
                    var v1 = (values[index] == null) ? "null" : $@"""{values[index]}""";
                    var v2 = (valueList[index] == null) ? "null" : $@"""{valueList[index]}""";

                    Assert.Fail($@"{nameof(TestListSort)} {nameof(values)}[{index}]: {v1} != {v2} ");
                }
            }
        }

        private string TestCompareInternal(int result, string v1, string v2, StringComparisonEx comparisonType, bool? nullIsLower, Func<string, bool> isNull)
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
                if (result != Math.Sign(StringComparerEx.Compare(v1: v1, v2: v2, comparisonType: comparisonType, nullIsLower: nullIsLowerValue, isNull: isNull)))
                {
                    v1 = (v1 == null) ? "null" : $@"""{v1}""";
                    v2 = (v2 == null) ? "null" : $@"""{v2}""";

                    return $@"{nameof(StringComparerEx)}.{nameof(StringComparerEx.Compare)}({nameof(v1)}: {v1}, {nameof(v2)}: {v2}, {nameof(comparisonType)}: {Enum.GetName(typeof(StringComparisonEx), comparisonType)}, {nameof(nullIsLower)}: {(nullIsLowerValue ? "true" : "false")}) != {result}";
                }

                if (result != -Math.Sign(StringComparerEx.Compare(v1: v2, v2: v1, comparisonType: comparisonType, nullIsLower: nullIsLowerValue, isNull: isNull)))
                {
                    v1 = (v1 == null) ? "null" : $@"""{v1}""";
                    v2 = (v2 == null) ? "null" : $@"""{v2}""";

                    return $@"{nameof(StringComparerEx)}.{nameof(StringComparerEx.Compare)}({nameof(v1)}: {v2}, {nameof(v2)}: {v1}, {nameof(comparisonType)}: {Enum.GetName(typeof(StringComparisonEx), comparisonType)}, {nameof(nullIsLower)}: {(nullIsLowerValue ? "true" : "false")}) != {result}";
                }

                return null;
            }
        }

        #endregion
    }
}
