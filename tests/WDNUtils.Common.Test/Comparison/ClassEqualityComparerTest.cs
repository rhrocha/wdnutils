using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class ClassEqualityComparerTest
    {
        [TestMethod]
        public async Task TestClassEqualityComparer()
        {
            var taskList = new List<Task>();

            taskList.Add(Task.Run(() => TestObject()));
            taskList.Add(Task.Run(() => TestObjectWithIsNull()));

            taskList.Add(Task.Run(() => TestStringReferenceEquals()));
            taskList.Add(Task.Run(() => TestStringIgnoreCase()));
            taskList.Add(Task.Run(() => TestStringTrim()));
            taskList.Add(Task.Run(() => TestStringTrimIgnoreCase()));

            taskList.Add(Task.Run(() => TestStringReferenceEqualsWithEmptyIsNull()));
            taskList.Add(Task.Run(() => TestStringIgnoreCaseWithEmptyIsNull()));
            taskList.Add(Task.Run(() => TestStringTrimWithEmptyIsNull()));
            taskList.Add(Task.Run(() => TestStringTrimIgnoreCaseWithEmptyIsNull()));

            taskList.Add(Task.Run(() => TestStringReferenceEqualsWithBlankIsNull()));
            taskList.Add(Task.Run(() => TestStringIgnoreCaseWithBlankIsNull()));
            taskList.Add(Task.Run(() => TestStringTrimWithBlankIsNull()));
            taskList.Add(Task.Run(() => TestStringTrimIgnoreCaseWithBlankIsNull()));

            await Task.WhenAll(taskList);
        }

        #region Object comparison

        private void TestObject()
        {
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = obj1;

            var comparer = ClassEqualityComparer<object>.ReferenceEquality;

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: obj1, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: obj2, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: obj3, comparer: comparer);

            TestInternal(expected: false, v1: obj1, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: obj1, v2: obj1, comparer: comparer);
            TestInternal(expected: false, v1: obj1, v2: obj2, comparer: comparer);
            TestInternal(expected: true, v1: obj1, v2: obj3, comparer: comparer);

            TestInternal(expected: false, v1: obj2, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: obj2, v2: obj1, comparer: comparer);
            TestInternal(expected: true, v1: obj2, v2: obj2, comparer: comparer);
            TestInternal(expected: false, v1: obj2, v2: obj3, comparer: comparer);

            TestInternal(expected: false, v1: obj3, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: obj3, v2: obj1, comparer: comparer);
            TestInternal(expected: false, v1: obj3, v2: obj2, comparer: comparer);
            TestInternal(expected: true, v1: obj3, v2: obj3, comparer: comparer);
        }

        private void TestObjectWithIsNull()
        {
            var obj1 = new object();
            var obj2 = new object();
            var obj3 = obj1;

            var comparer = new ClassEqualityComparer<object>(equals: ReferenceEquals, getHashCode: obj => obj.GetHashCode(), isNull: obj => ReferenceEquals(obj, obj2));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: obj1, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: obj2, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: obj3, comparer: comparer);

            TestInternal(expected: false, v1: obj1, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: obj1, v2: obj1, comparer: comparer);
            TestInternal(expected: false, v1: obj1, v2: obj2, comparer: comparer);
            TestInternal(expected: true, v1: obj1, v2: obj3, comparer: comparer);

            TestInternal(expected: true, v1: obj2, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: obj2, v2: obj1, comparer: comparer);
            TestInternal(expected: true, v1: obj2, v2: obj2, comparer: comparer);
            TestInternal(expected: false, v1: obj2, v2: obj3, comparer: comparer);

            TestInternal(expected: false, v1: obj3, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: obj3, v2: obj1, comparer: comparer);
            TestInternal(expected: false, v1: obj3, v2: obj2, comparer: comparer);
            TestInternal(expected: true, v1: obj3, v2: obj3, comparer: comparer);
        }

        #endregion

        #region String comparison

        private void TestStringReferenceEquals()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = ClassEqualityComparer<string>.ReferenceEquality;

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringIgnoreCase()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1, v2, StringComparison.OrdinalIgnoreCase), getHashCode: (obj) => obj.ToUpper().GetHashCode(), isNull: null);

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringTrim()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1.Trim(), v2.Trim(), StringComparison.Ordinal), getHashCode: (obj) => obj.Trim().GetHashCode(), isNull: null);

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringTrimIgnoreCase()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1.Trim(), v2.Trim(), StringComparison.OrdinalIgnoreCase), getHashCode: (obj) => obj.Trim().ToUpper().GetHashCode(), isNull: null);

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        #endregion

        #region String comparison with empty string equals to null

        private void TestStringReferenceEqualsWithEmptyIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>(ReferenceEquals, getHashCode: (obj) => RuntimeHelpers.GetHashCode(obj), isNull: (obj) => string.IsNullOrEmpty(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringIgnoreCaseWithEmptyIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1, v2, StringComparison.OrdinalIgnoreCase), getHashCode: (obj) => obj.ToUpper().GetHashCode(), isNull: (obj) => string.IsNullOrEmpty(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringTrimWithEmptyIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1.Trim(), v2.Trim(), StringComparison.Ordinal), getHashCode: (obj) => obj.Trim().GetHashCode(), isNull: (obj) => string.IsNullOrEmpty(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringTrimIgnoreCaseWithEmptyIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1.Trim(), v2.Trim(), StringComparison.OrdinalIgnoreCase), getHashCode: (obj) => obj.Trim().ToUpper().GetHashCode(), isNull: (obj) => string.IsNullOrEmpty(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        #endregion

        #region String comparison with blank and empty strings equal to null

        private void TestStringReferenceEqualsWithBlankIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>(ReferenceEquals, getHashCode: (obj) => RuntimeHelpers.GetHashCode(obj), isNull: (obj) => string.IsNullOrWhiteSpace(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringIgnoreCaseWithBlankIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1, v2, StringComparison.OrdinalIgnoreCase), getHashCode: (obj) => obj.ToUpper().GetHashCode(), isNull: (obj) => string.IsNullOrWhiteSpace(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringTrimWithBlankIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1.Trim(), v2.Trim(), StringComparison.Ordinal), getHashCode: (obj) => obj.Trim().GetHashCode(), isNull: (obj) => string.IsNullOrWhiteSpace(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        private void TestStringTrimIgnoreCaseWithBlankIsNull()
        {
            var empty = string.Empty;
            var blank = @" ";
            var lower = @"test";
            var upper = @"TEST";

            var comparer = new ClassEqualityComparer<string>((v1, v2) => string.Equals(v1.Trim(), v2.Trim(), StringComparison.OrdinalIgnoreCase), getHashCode: (obj) => obj.Trim().ToUpper().GetHashCode(), isNull: (obj) => string.IsNullOrWhiteSpace(obj));

            TestInternal(expected: true, v1: null, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: null, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: null, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: empty, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: empty, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: empty, v2: upper, comparer: comparer);

            TestInternal(expected: true, v1: blank, v2: null, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: empty, comparer: comparer);
            TestInternal(expected: true, v1: blank, v2: blank, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: lower, comparer: comparer);
            TestInternal(expected: false, v1: blank, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: lower, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: lower, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: lower, v2: upper, comparer: comparer);

            TestInternal(expected: false, v1: upper, v2: null, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: empty, comparer: comparer);
            TestInternal(expected: false, v1: upper, v2: blank, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: lower, comparer: comparer);
            TestInternal(expected: true, v1: upper, v2: upper, comparer: comparer);
        }

        #endregion

        #region Internal methods

        private void TestInternal<T>(bool expected, T v1, T v2, ClassEqualityComparer<T> comparer) where T : class
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
        }

        #endregion
    }
}
