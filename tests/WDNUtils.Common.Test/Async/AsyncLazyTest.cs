using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class AsyncLazyTest
    {
        [TestMethod]
        public async Task TestAsyncLazyConcurrency()
        {
            var test1 = new AsyncLazyConcurrentTester().Test(mode: LazyThreadSafetyMode.None);
            var test2 = new AsyncLazyConcurrentTester().Test(mode: LazyThreadSafetyMode.PublicationOnly);
            var test3 = new AsyncLazyConcurrentTester().Test(mode: LazyThreadSafetyMode.ExecutionAndPublication);

            await Task.WhenAll(test1, test2, test3);
        }

        [TestMethod]
        public async Task TestAsyncLazyNormal()
        {
            var modeList = (LazyThreadSafetyMode[])Enum.GetValues(typeof(LazyThreadSafetyMode));
            var taskList = new List<Task>();

            foreach (var mode in modeList)
            {
                // Default constructor

                taskList.Add(new AsyncLazyNormalTester().Test(customFactory: false, captureException: null, mode: mode));
                taskList.Add(new AsyncLazyNormalTester().Test(customFactory: false, captureException: false, mode: mode));
                taskList.Add(new AsyncLazyNormalTester().Test(customFactory: false, captureException: true, mode: mode));

                // Custom value factory

                taskList.Add(new AsyncLazyNormalTester().Test(customFactory: true, captureException: null, mode: mode));
                taskList.Add(new AsyncLazyNormalTester().Test(customFactory: true, captureException: false, mode: mode));
                taskList.Add(new AsyncLazyNormalTester().Test(customFactory: true, captureException: true, mode: mode));
            }

            await Task.WhenAll(taskList);
        }

        [TestMethod]
        public async Task TestAsyncLazyException()
        {
            var modeList = (LazyThreadSafetyMode[])Enum.GetValues(typeof(LazyThreadSafetyMode));

            foreach (var mode in modeList)
            {
                // Default constructor

                await AsyncLazyExceptionTester.Test(customFactory: false, captureException: null, mode: mode, expectedException1: typeof(AssertFailedException), expectedException2: null);
                await AsyncLazyExceptionTester.Test(customFactory: false, captureException: false, mode: mode, expectedException1: typeof(AssertFailedException), expectedException2: null);
                await AsyncLazyExceptionTester.Test(customFactory: false, captureException: true, mode: mode, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

                // Custom value factory

                await AsyncLazyExceptionTester.Test(customFactory: true, captureException: null, mode: mode, expectedException1: typeof(AssertFailedException), expectedException2: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(AssertFailedException));
                await AsyncLazyExceptionTester.Test(customFactory: true, captureException: false, mode: mode, expectedException1: typeof(AssertFailedException), expectedException2: null);
                await AsyncLazyExceptionTester.Test(customFactory: true, captureException: true, mode: mode, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public async Task TestAsyncLazyRecursive()
        {
            var modeList = (LazyThreadSafetyMode[])Enum.GetValues(typeof(LazyThreadSafetyMode));

            foreach (var mode in modeList)
            {
                // Default constructor

                await AsyncLazyRecursiveTester.Test(customFactory: false, captureException: null, mode: mode, expectedException1: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException), expectedException2: null);
                await AsyncLazyRecursiveTester.Test(customFactory: false, captureException: false, mode: mode, expectedException1: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException), expectedException2: null);
                await AsyncLazyRecursiveTester.Test(customFactory: false, captureException: true, mode: mode, expectedException1: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException), expectedException2: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException));

                // Custom value factory

                await AsyncLazyRecursiveTester.Test(customFactory: true, captureException: null, mode: mode, expectedException1: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException), expectedException2: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException));
                await AsyncLazyRecursiveTester.Test(customFactory: true, captureException: false, mode: mode, expectedException1: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException), expectedException2: null);
                await AsyncLazyRecursiveTester.Test(customFactory: true, captureException: true, mode: mode, expectedException1: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException), expectedException2: (mode == LazyThreadSafetyMode.PublicationOnly) ? null : typeof(LockRecursionException));
            }
        }

        #region Internal methods

        #region Concurrent

        private class AsyncLazyConcurrentTester
        {
            private static readonly TimeSpan TimeSpan1 = TimeSpan.FromMilliseconds(Math.Max(25, 100 / Environment.ProcessorCount));
            private static readonly TimeSpan TimeSpan2 = TimeSpan.FromTicks(TimeSpan1.Ticks * 2);

            private bool IsSlowTask { get; set; }

            public async Task Test(LazyThreadSafetyMode mode)
            {
                var lazyValue = new AsyncLazy<bool>(valueFactory: async () =>
                {
                    var isSlowTask = IsSlowTask;
                    await Task.Delay(isSlowTask ? TimeSpan2 : TimeSpan1).ConfigureAwait(false);
                    return isSlowTask;
                },
                mode: mode);

                IsSlowTask = true;
                var slowTask = lazyValue.GetValueAsync();

                IsSlowTask = false;
                var fastTask = lazyValue.GetValueAsync();

                await Task.WhenAll(slowTask, fastTask);

                switch (mode)
                {
                    case LazyThreadSafetyMode.None:
                        Assert.AreEqual(false, await fastTask);
                        Assert.AreEqual(true, await slowTask);
                        break;
                    case LazyThreadSafetyMode.PublicationOnly:
                        Assert.AreEqual(false, await fastTask);
                        Assert.AreEqual(false, await slowTask);
                        break;
                    case LazyThreadSafetyMode.ExecutionAndPublication:
                        Assert.AreEqual(true, await fastTask);
                        Assert.AreEqual(true, await slowTask);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        #endregion

        #region Normal

        private class AsyncLazyNormalTester
        {
            public async Task Test(bool customFactory, bool? captureException, LazyThreadSafetyMode mode)
            {
                var lazyValue = (!customFactory)
                    ? new AsyncLazyEx<int>(mode: mode, captureException: captureException)
                    : new AsyncLazyEx<int>(valueFactory: async () => await Task.FromResult<int>(default(int)), mode: mode, captureException: captureException);

                try
                {
                    Assert.AreEqual(default(int), await lazyValue.GetValueAsync());
                }
                catch (Exception ex)
                {
                    Assert.Fail($@"{ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        #endregion

        #region Exception

        private static class AsyncLazyExceptionTester
        {
            private static bool ThrowException { get; set; }

            public static async Task Test(bool customFactory, bool? captureException, LazyThreadSafetyMode mode, Type expectedException1, Type expectedException2)
            {
                ThrowException = true;

                var lazyValue = (!customFactory)
                    ? new AsyncLazyEx<LazyValueClass>(mode: mode, captureException: captureException)
                    : new AsyncLazyEx<LazyValueClass>(valueFactory: async () => await Task.FromResult(new LazyValueClass()), mode: mode, captureException: captureException);

                try
                {
                    await lazyValue.GetValueAsync();

                    Assert.IsNull(expectedException1);
                }
                catch (Exception ex)
                {
                    Assert.AreEqual(expectedException1, ex.GetType());
                }

                ThrowException = false;

                try
                {
                    await lazyValue.GetValueAsync();

                    Assert.IsNull(expectedException2);
                }
                catch (Exception ex)
                {
                    Assert.AreEqual(expectedException2, ex.GetType());
                }
            }

            private class LazyValueClass
            {
                public LazyValueClass()
                {
                    if (ThrowException)
                        throw new AssertFailedException();
                }
            }
        }

        #endregion

        #region Recursive

        private static class AsyncLazyRecursiveTester
        {
            private static long RecursiveCounter { get; set; }
            private static AsyncLazyEx<LazyValueClass> LazyValue { get; set; }

            public static async Task Test(bool customFactory, bool? captureException, LazyThreadSafetyMode mode, Type expectedException1, Type expectedException2)
            {
                // Each recursive iteration blocks a thread because LazyValueClass will call the method 'GetValueAsync' from
                // the constructor using 'Task.Run(...).Wait()', this cannot be fixed because the constructor cannot be async.
                // So running more recursive iterations will affect the performance of the unit test execution.
                RecursiveCounter = 1;

                LazyValue = null;

                LazyValue = (!customFactory)
                    ? new AsyncLazyEx<LazyValueClass>(mode: mode, captureException: captureException)
                    : new AsyncLazyEx<LazyValueClass>(valueFactory: async () => await Task.FromResult(new LazyValueClass()), mode: mode, captureException: captureException);

                try
                {
                    await LazyValue.GetValueAsync();

                    Assert.IsNull(expectedException1);
                }
                catch (Exception ex)
                {
                    Assert.AreEqual(expectedException1, ex.GetType());
                }

                RecursiveCounter = 0;

                try
                {
                    await LazyValue.GetValueAsync();

                    Assert.IsNull(expectedException2);
                }
                catch (Exception ex)
                {
                    Assert.AreEqual(expectedException2, ex.GetType());
                }
            }

            private class LazyValueClass
            {
                public LazyValueClass()
                {
                    if (RecursiveCounter-- > 0)
                    {
                        try
                        {
                            Task.Run(async () => await LazyValue.GetValueAsync().ConfigureAwait(false)).Wait();
                        }
                        catch (AggregateException ex)
                        {
                            throw ex.InnerException;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
