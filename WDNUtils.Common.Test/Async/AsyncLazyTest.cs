using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            await AsyncLazyTester.TestConcurrent(mode: LazyThreadSafetyMode.None, keepValue: false);
            await AsyncLazyTester.TestConcurrent(mode: LazyThreadSafetyMode.PublicationOnly, keepValue: true);
            await AsyncLazyTester.TestConcurrent(mode: LazyThreadSafetyMode.ExecutionAndPublication, keepValue: true);
        }

        [TestMethod]
        public async Task TestAsyncLazyModes()
        {
            #region LazyThreadSafetyMode.None

            // Recursion is always blocked, exceptions are captured by default for custom value factory only

            // Normal - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: null, mode: LazyThreadSafetyMode.None, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: false, mode: LazyThreadSafetyMode.None, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: true, mode: LazyThreadSafetyMode.None, expectedException1: null, expectedException2: null);

            // Normal - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: null, mode: LazyThreadSafetyMode.None, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: false, mode: LazyThreadSafetyMode.None, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: true, mode: LazyThreadSafetyMode.None, expectedException1: null, expectedException2: null);

            // Throw exception - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: null, mode: LazyThreadSafetyMode.None, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: false, mode: LazyThreadSafetyMode.None, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: true, mode: LazyThreadSafetyMode.None, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

            // Throw exception - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: null, mode: LazyThreadSafetyMode.None, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: false, mode: LazyThreadSafetyMode.None, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: true, mode: LazyThreadSafetyMode.None, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

            // Recursive - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: null, mode: LazyThreadSafetyMode.None, expectedException1: typeof(InvalidOperationException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: false, mode: LazyThreadSafetyMode.None, expectedException1: typeof(InvalidOperationException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: true, mode: LazyThreadSafetyMode.None, expectedException1: typeof(InvalidOperationException), expectedException2: typeof(InvalidOperationException));

            // Recursive - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: null, mode: LazyThreadSafetyMode.None, expectedException1: typeof(InvalidOperationException), expectedException2: typeof(InvalidOperationException));
            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: false, mode: LazyThreadSafetyMode.None, expectedException1: typeof(InvalidOperationException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: true, mode: LazyThreadSafetyMode.None, expectedException1: typeof(InvalidOperationException), expectedException2: typeof(InvalidOperationException));

            #endregion

            #region LazyThreadSafetyMode.PublicationOnly

            // Recursion is not blocked, exceptions are not captured by default

            // Normal - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: null, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: false, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: true, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);

            // Normal - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: null, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: false, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: true, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);

            // Throw exception - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: null, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: false, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: true, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

            // Throw exception - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: null, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: false, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: true, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

            // Recursive - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: null, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: false, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: true, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);

            // Recursive - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: null, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: false, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: true, mode: LazyThreadSafetyMode.PublicationOnly, expectedException1: null, expectedException2: null);

            #endregion

            #region LazyThreadSafetyMode.ExecutionAndPublication

            // Recursion is always blocked, exceptions are captured by default for custom value factory only

            // Normal - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: null, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: false, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: false, captureException: true, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: null, expectedException2: null);

            // Normal - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: null, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: false, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: null, expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: false, captureException: true, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: null, expectedException2: null);

            // Throw exception - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: null, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: false, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: false, throwException: true, captureException: true, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

            // Throw exception - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: null, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: false, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(AssertFailedException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: false, throwException: true, captureException: true, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(AssertFailedException), expectedException2: typeof(AssertFailedException));

            // Recursive - Default constructor

            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: null, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(InvalidOperationException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: false, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(InvalidOperationException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: false, recursive: true, throwException: false, captureException: true, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(InvalidOperationException), expectedException2: typeof(InvalidOperationException));

            // Recursive - Custom value factory

            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: null, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(InvalidOperationException), expectedException2: typeof(InvalidOperationException));
            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: false, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(InvalidOperationException), expectedException2: null);
            await AsyncLazyTester.Test(customFactory: true, recursive: true, throwException: false, captureException: true, mode: LazyThreadSafetyMode.ExecutionAndPublication, expectedException1: typeof(InvalidOperationException), expectedException2: typeof(InvalidOperationException));

            #endregion
        }

        #region Internal methods

        private static class AsyncLazyTester
        {
            public static long RecursiveCounter { get; set; }
            public static bool ThrowException { get; set; }
            public static AsyncLazyEx<LazyValueClass> LazyValue { get; set; }

            public async static Task Test(bool customFactory, bool recursive, bool throwException, bool? captureException, LazyThreadSafetyMode mode, Type expectedException1, Type expectedException2)
            {
                // Each recursive iteration blocks a thread because LazyValueClass will call the method 'GetValueAsync' from
                // the constructor using 'Task.Run(...).Wait()', this cannot be fixed because the constructor cannot be async.
                // So running more recursive iterations will affect the performance of the unit test execution.
                RecursiveCounter = (recursive) ? 1 : 0;

                ThrowException = throwException;
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
                ThrowException = false;

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

            public static bool IsSlowTask { get; set; }

            public async static Task TestConcurrent(LazyThreadSafetyMode mode, bool keepValue)
            {
                var lazyValue = new AsyncLazy<bool>(valueFactory: async () =>
                {
                    var isSlowTask = IsSlowTask;
                    await Task.Delay(TimeSpan.FromMilliseconds(isSlowTask ? 100 : 50)).ConfigureAwait(false);
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

        private class LazyValueClass
        {
            public LazyValueClass()
            {
                if (AsyncLazyTester.ThrowException)
                    throw new AssertFailedException();

                if (AsyncLazyTester.RecursiveCounter-- > 0)
                {
                    try
                    {
                        Task.Run(async () => await AsyncLazyTester.LazyValue.GetValueAsync().ConfigureAwait(false)).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
        }

        #endregion
    }
}
