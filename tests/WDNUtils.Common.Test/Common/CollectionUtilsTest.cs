using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class CollectionUtilsTest
    {
        [TestMethod]
        public void TestCollectionUtilsDictionary()
        {
            var valueList = new Dictionary<int, int>() { { 0, 0 }, { 1, 1 }, { 3, 3 } };

            Assert.AreEqual(0, valueList.GetOrDefault(0));
            Assert.AreEqual(1, valueList.GetOrDefault(1));
            Assert.AreEqual(0, valueList.GetOrDefault(2));
            Assert.AreEqual(3, valueList.GetOrDefault(3));

            Assert.AreEqual(0, valueList.GetOrDefault(0, -1));
            Assert.AreEqual(1, valueList.GetOrDefault(1, -1));
            Assert.AreEqual(-1, valueList.GetOrDefault(2, -1));
            Assert.AreEqual(3, valueList.GetOrDefault(3, -1));

            Assert.IsNotNull(valueList.GetOrNull(0));
            Assert.IsNotNull(valueList.GetOrNull(1));
            Assert.IsNull(valueList.GetOrNull(2));
            Assert.IsNotNull(valueList.GetOrNull(3));

            var textList = new Dictionary<string, string>() { { @"0", @"0" }, { @"1", @"1" }, { @"3", @"3" } };

            Assert.AreEqual(@"0", textList.GetOrDefault(@"0"));
            Assert.AreEqual(@"1", textList.GetOrDefault(@"1"));
            Assert.IsNull(textList.GetOrDefault(@"2"));
            Assert.AreEqual(@"3", textList.GetOrDefault(@"3"));

            Assert.AreEqual(@"0", textList.GetOrDefault(@"0", @"-1"));
            Assert.AreEqual(@"1", textList.GetOrDefault(@"1", @"-1"));
            Assert.AreEqual(@"-1", textList.GetOrDefault(@"2", @"-1"));
            Assert.AreEqual(@"3", textList.GetOrDefault(@"3", @"-1"));
        }

        [TestMethod]
        public void TestCollectionUtilsNullable()
        {
            var emptyList = new List<int>();
            var valueList = new List<int>() { 0, 1 };
            var nullableList = new List<int?>() { 0, 1 };

            Assert.IsTrue(Enumerable.SequenceEqual(valueList.AsNullable(), nullableList));

            Assert.IsNull(emptyList.FirstOrNull());
            Assert.IsNull(emptyList.FirstOrNull(item => true));
            Assert.IsNull(emptyList.FirstOrNull(item => false));
            Assert.IsNull(emptyList.LastOrNull());
            Assert.IsNull(emptyList.LastOrNull(item => true));
            Assert.IsNull(emptyList.LastOrNull(item => false));
            Assert.IsNull(emptyList.SingleOrNull());
            Assert.IsNull(emptyList.SingleOrNull(item => true));
            Assert.IsNull(emptyList.SingleOrNull(item => false));

            Assert.AreEqual(0, valueList.FirstOrNull());
            Assert.AreEqual(0, valueList.FirstOrNull(item => item == 0));
            Assert.AreEqual(1, valueList.FirstOrNull(item => item == 1));
            Assert.AreEqual(null, valueList.FirstOrNull(item => item == 2));
            Assert.AreEqual(1, valueList.LastOrNull());
            Assert.AreEqual(0, valueList.LastOrNull(item => item == 0));
            Assert.AreEqual(1, valueList.LastOrNull(item => item == 1));
            Assert.AreEqual(null, valueList.LastOrNull(item => item == 2));

            try
            {
                valueList.SingleOrNull();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                // OK
            }
            Assert.AreEqual(0, valueList.SingleOrNull(item => item == 0));
            Assert.AreEqual(1, valueList.SingleOrNull(item => item == 1));
            Assert.AreEqual(null, valueList.SingleOrNull(item => item == 2));
        }

        [TestMethod]
        public void TestCollectionUtilsShuffle()
        {
            var valueList = new List<int>() { 0, 1, 2, 4, 8, 16, 32 };

            var test = valueList.ToList().Shuffle();

            while (Enumerable.SequenceEqual(test, valueList))
            {
                test = test.Shuffle();
            }

            Assert.IsTrue(Enumerable.SequenceEqual(valueList.OrderBy(item => item), test.OrderBy(item => item)));

            test = valueList.TakeShuffle(5).ToList();

            Assert.AreEqual(5, test.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(test.OrderBy(item => item), test.Intersect(valueList).OrderBy(item => item)));
        }

        [TestMethod]
        public void TestCollectionUtilsSplitList()
        {
            for (int repeat = 0; repeat < 1000; repeat++)
            {
                int listSize = (repeat > 5) ? RandomUtils.NextInt32(0, 100) : repeat;
                int sublistLength = RandomUtils.NextInt32(1, 100);

                var valueList = RandomUtils.NextBytes(listSize);

                var sublists = valueList.SplitList(sublistLength).ToList();

                Assert.AreEqual(Convert.ToInt32(Math.Ceiling((float)listSize / sublistLength)), sublists.Count);

                int offset = 0;
                var lastSublistSize = listSize % sublistLength;

                if (lastSublistSize == 0)
                {
                    lastSublistSize = sublistLength;
                }

                for (int index = 0; index < sublists.Count; index++)
                {
                    Assert.AreEqual(
                        (index < (sublists.Count - 1)) ? sublistLength : lastSublistSize,
                        sublists[index].Count);

                    Assert.IsTrue(Enumerable.SequenceEqual(valueList.Skip(offset).Take(sublistLength), sublists[index]));

                    offset += sublists[index].Count;
                }
            }
        }

        private class TestItem : IEquatable<TestItem>
        {
            public int Key { get; set; }
            public int Value { get; set; }

            bool IEquatable<TestItem>.Equals(TestItem other)
            {
                return Value == other?.Value;
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        [TestMethod]
        public void TestCollectionUtilsDuplicates()
        {
            var testList = new List<TestItem>()
            {
                new TestItem() { Key = 0, Value = 1 },
                new TestItem() { Key = 2, Value = 1 },
                new TestItem() { Key = 0, Value = 2 },
                new TestItem() { Key = 1, Value = 0 },
                new TestItem() { Key = 2, Value = 0 },
                new TestItem() { Key = 1, Value = 2 },
            };

            var firstKeyList = new List<TestItem>()
            {
                new TestItem() { Key = 0, Value = 1 },
                new TestItem() { Key = 2, Value = 1 },
                new TestItem() { Key = 1, Value = 0 },
            };

            var firstValueList = new List<TestItem>()
            {
                new TestItem() { Key = 0, Value = 1 },
                new TestItem() { Key = 0, Value = 2 },
                new TestItem() { Key = 1, Value = 0 },
            };

            Assert.IsTrue(Enumerable.SequenceEqual(firstKeyList, testList.RemoveDuplicatesStable(item => item.Key)));
            Assert.IsTrue(Enumerable.SequenceEqual(firstValueList, testList.RemoveDuplicatesStable()));

            testList = testList.OrderBy(item => item.Key).ThenBy(item => item.Value).ToList();

            firstKeyList = new List<TestItem>()
            {
                new TestItem() { Key = 0, Value = 1 },
                new TestItem() { Key = 1, Value = 0 },
                new TestItem() { Key = 2, Value = 0 },
            };

            firstValueList = new List<TestItem>()
            {
                new TestItem() { Key = 0, Value = 1 },
                new TestItem() { Key = 0, Value = 2 },
                new TestItem() { Key = 1, Value = 0 },
            };

            Assert.IsTrue(Enumerable.SequenceEqual(firstKeyList, testList.RemoveDuplicatesStable(item => item.Key)));
            Assert.IsTrue(Enumerable.SequenceEqual(firstValueList, testList.RemoveDuplicatesStable()));

            testList = testList.OrderBy(item => item.Value).ThenBy(item => item.Key).ToList();

            firstKeyList = new List<TestItem>()
            {
                new TestItem() { Key = 1, Value = 0 },
                new TestItem() { Key = 2, Value = 0 },
                new TestItem() { Key = 0, Value = 1 },
            };

            firstValueList = new List<TestItem>()
            {
                new TestItem() { Key = 1, Value = 0 },
                new TestItem() { Key = 0, Value = 1 },
                new TestItem() { Key = 0, Value = 2 },
            };

            Assert.IsTrue(Enumerable.SequenceEqual(firstKeyList, testList.RemoveDuplicatesStable(item => item.Key)));
            Assert.IsTrue(Enumerable.SequenceEqual(firstValueList, testList.RemoveDuplicatesStable()));
        }

        [TestMethod]
        public void TestCollectionUtilsConcatSplit()
        {
            for (int index = 0; index < 10; index++)
            {
                var input = RandomUtils.NextBytes(0, 10);

                var size1 = RandomUtils.NextInt32(0, input.Length);
                var size2 = RandomUtils.NextInt32(size1, input.Length) - size1;

                input.ArraySplit(size1, out var output1, out var outputTemp);
                outputTemp.ArraySplit(size2, out var output2, out var output3);

                Assert.AreEqual(size1, output1.Length);
                Assert.AreEqual(size2, output2.Length);
                Assert.AreEqual(input.Length - size1 - size2, output3.Length);

                var output = output1.Concat(output2).Concat(output3).ToArray();

                Assert.AreEqual(input.Length, output.Length);
                Assert.IsTrue(Enumerable.SequenceEqual(input, output));

                var concat1 = CollectionUtils.ArrayConcat(output1);

                Assert.AreEqual(output1.Length, concat1.Length);
                Assert.IsTrue(Enumerable.SequenceEqual(output1, concat1));

                var concatTemp = CollectionUtils.ArrayConcat(output2, output3);

                Assert.AreEqual(outputTemp.Length, concatTemp.Length);
                Assert.IsTrue(Enumerable.SequenceEqual(outputTemp, concatTemp));

                var concat = CollectionUtils.ArrayConcat(output1, output2, output3);

                Assert.AreEqual(input.Length, concat.Length);
                Assert.IsTrue(Enumerable.SequenceEqual(input, concat));
            }
        }
    }
}
