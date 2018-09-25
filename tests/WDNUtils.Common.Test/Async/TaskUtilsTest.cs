using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class TaskUtilsTest
    {
        [TestMethod]
        public async Task TestTaskUtilsAwait()
        {
            await Task.WhenAll(
                TestAwaitActionOk(),
                TestAwaitActionOkAsync(),
                TestAwaitActionException(),
                TestAwaitActionExceptionAsync(),
                TestAwaitActionTimeout(),
                TestAwaitActionTimeoutAsync(),
                TestAwaitActionTimeoutNotify(),
                TestAwaitActionTimeoutNotifyAsync(),
                TestAwaitCancelableActionOk(),
                TestAwaitCancelableActionOkAsync(),
                TestAwaitCancelableActionException(),
                TestAwaitCancelableActionExceptionAsync(),
                TestAwaitCancelableActionTimeout(),
                TestAwaitCancelableActionTimeoutAsync(),
                TestAwaitCancelableActionTimeoutNotify(),
                TestAwaitCancelableActionTimeoutNotifyAsync(),
                TestAwaitFunctionOk(),
                TestAwaitFunctionOkAsync(),
                TestAwaitFunctionException(),
                TestAwaitFunctionExceptionAsync(),
                TestAwaitFunctionTimeout(),
                TestAwaitFunctionTimeoutAsync(),
                TestAwaitFunctionTimeoutNotify(),
                TestAwaitFunctionTimeoutNotifyAsync(),
                TestAwaitCancelableFunctionOk(),
                TestAwaitCancelableFunctionOkAsync(),
                TestAwaitCancelableFunctionException(),
                TestAwaitCancelableFunctionExceptionAsync(),
                TestAwaitCancelableFunctionTimeout(),
                TestAwaitCancelableFunctionTimeoutNotify(),
                TestAwaitCancelableFunctionTimeoutNotifyAsync());
        }

        [TestMethod]
        public void TestTaskUtilsWaitUntil()
        {
            // Cannot run this test in parallel because it locks the current thread by calling Task.Wait(TimeSpan)
            TestWaitUntil(timeout: false, exceptionBefore: false, exceptionAfter: false);
            TestWaitUntil(timeout: false, exceptionBefore: false, exceptionAfter: true);
            TestWaitUntil(timeout: false, exceptionBefore: true, exceptionAfter: false);
            TestWaitUntil(timeout: true, exceptionBefore: false, exceptionAfter: false);
            TestWaitUntil(timeout: true, exceptionBefore: false, exceptionAfter: true);
            TestWaitUntil(timeout: true, exceptionBefore: true, exceptionAfter: false);
        }

        [TestMethod]
        public async Task TestTaskUtilsFireAndForget()
        {
            await Task.WhenAll(
                TestFireAndForgetAction(asynchronous: false, delay: false, exception: false),
                TestFireAndForgetAction(asynchronous: false, delay: false, exception: true),
                TestFireAndForgetAction(asynchronous: false, delay: true, exception: false),
                TestFireAndForgetAction(asynchronous: false, delay: true, exception: true),
                TestFireAndForgetAction(asynchronous: true, delay: false, exception: false),
                TestFireAndForgetAction(asynchronous: true, delay: false, exception: true),
                TestFireAndForgetAction(asynchronous: true, delay: true, exception: false),
                TestFireAndForgetAction(asynchronous: true, delay: true, exception: true),
                TestFireAndForgetFunction(asynchronous: false, delay: false, exception: false),
                TestFireAndForgetFunction(asynchronous: false, delay: false, exception: true),
                TestFireAndForgetFunction(asynchronous: false, delay: true, exception: false),
                TestFireAndForgetFunction(asynchronous: false, delay: true, exception: true),
                TestFireAndForgetFunction(asynchronous: true, delay: false, exception: false),
                TestFireAndForgetFunction(asynchronous: true, delay: false, exception: true),
                TestFireAndForgetFunction(asynchronous: true, delay: true, exception: false),
                TestFireAndForgetFunction(asynchronous: true, delay: true, exception: true));
        }

        #region Internal methods

        private static readonly TimeSpan TimeSpan1 = TimeSpan.FromMilliseconds(Math.Max(25, 100 / Environment.ProcessorCount));
        private static readonly TimeSpan TimeSpan2 = TimeSpan.FromTicks(TimeSpan1.Ticks * 2);

        #region Await Action

        private async static Task TestAwaitActionOk()
        {
            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));
        }

        private async static Task TestAwaitActionOkAsync()
        {
            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));
        }

        private async static Task TestAwaitActionException()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan2,
                    notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitActionExceptionAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitActionTimeout()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async () => await Task.Delay(TimeSpan2),
                    timeout: TimeSpan1,
                    notifyTimeout: (Action<Task>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitActionTimeoutAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async () => await Task.Delay(TimeSpan2),
                    timeout: TimeSpan1,
                    notifyTimeout: (Func<Task, Task>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitActionTimeoutNotify()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan2),
                timeout: TimeSpan1,
                notifyTimeout: (task) => finished = true);

            Assert.IsTrue(finished);
        }

        private async static Task TestAwaitActionTimeoutNotifyAsync()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan2),
                timeout: TimeSpan1,
                notifyTimeout: (task) => { finished = true; return Task.Delay(0); });

            Assert.IsTrue(finished);
        }

        private async static Task TestAwaitCancelableActionOk()
        {
            await TaskUtils.WaitTimeout(
                async (cancelToken) => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));
        }

        private async static Task TestAwaitCancelableActionOkAsync()
        {
            await TaskUtils.WaitTimeout(
                async (cancelToken) => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));
        }

        private async static Task TestAwaitCancelableActionException()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan2,
                    notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableActionExceptionAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableActionTimeout()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); },
                    timeout: TimeSpan1,
                    notifyTimeout: (Action<Task>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableActionTimeoutAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); },
                    timeout: TimeSpan1,
                    notifyTimeout: (Func<Task, Task>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableActionTimeoutNotify()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); },
                timeout: TimeSpan1,
                notifyTimeout: (task) => { finished = true; try { task.Wait(); } catch (AggregateException ex) { Assert.IsTrue(typeof(OperationCanceledException).IsAssignableFrom(ex.InnerException.GetType())); } });

            Assert.IsTrue(finished);
        }

        private async static Task TestAwaitCancelableActionTimeoutNotifyAsync()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); },
                timeout: TimeSpan1,
                notifyTimeout: async (task) => { finished = true; try { await task; } catch (OperationCanceledException) { } });

            Assert.IsTrue(finished);
        }

        #endregion

        #region Await Function

        private async static Task TestAwaitFunctionOk()
        {
            var result = await TaskUtils.WaitTimeout(
                    func: async () => { await Task.Delay(TimeSpan1); return 1; },
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private async static Task TestAwaitFunctionOkAsync()
        {

            var result = await TaskUtils.WaitTimeout(
                func: async () => { await Task.Delay(TimeSpan1); return 1; },
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private async static Task TestAwaitFunctionException()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitFunctionExceptionAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: (Func<Task<int>>)(async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); }),
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitFunctionTimeout()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: async () => { await Task.Delay(TimeSpan2); return 1; },
                    timeout: TimeSpan1,
                    notifyTimeout: (Func<Task, int>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitFunctionTimeoutAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: async () => { await Task.Delay(TimeSpan2); return 1; },
                    timeout: TimeSpan1,
                    notifyTimeout: (Func<Task, Task<int>>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitFunctionTimeoutNotify()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async () => { await Task.Delay(TimeSpan2); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: (task) => -1);

            Assert.AreEqual(-1, result);
        }

        private async static Task TestAwaitFunctionTimeoutNotifyAsync()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async () => { await Task.Delay(TimeSpan2); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: (task) => Task.FromResult(-1));

            Assert.AreEqual(-1, result);
        }

        private async static Task TestAwaitCancelableFunctionOk()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan1); return 1; },
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private async static Task TestAwaitCancelableFunctionOkAsync()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan1); return 1; },
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private async static Task TestAwaitCancelableFunctionException()
        {
            try
            {
                var result = await TaskUtils.WaitTimeout(
                    func: async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableFunctionExceptionAsync()
        {
            try
            {
                var result = await TaskUtils.WaitTimeout(
                    func: (Func<CancellationToken, Task<int>>)(async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); }),
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableFunctionTimeout()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); return 1; },
                    timeout: TimeSpan1,
                    notifyTimeout: (Func<Task, int>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableFunctionTimeoutAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); return 1; },
                    timeout: TimeSpan1,
                    notifyTimeout: (Func<Task, Task<int>>)null);

                throw new ApplicationException();
            }
            catch (TimeoutException)
            {
                // Success
            }
        }

        private async static Task TestAwaitCancelableFunctionTimeoutNotify()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: (task) => { try { task.Wait(); } catch (AggregateException ex) { Assert.IsTrue(typeof(OperationCanceledException).IsAssignableFrom(ex.InnerException.GetType())); } return -1; });

            Assert.AreEqual(-1, result);
        }

        private async static Task TestAwaitCancelableFunctionTimeoutNotifyAsync()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: async (task) => { try { await task; } catch (OperationCanceledException) { }; return -1; });

            Assert.AreEqual(-1, result);
        }

        #endregion

        #region WaitUntil

        private void TestWaitUntil(bool timeout, bool exceptionBefore, bool exceptionAfter)
        {
            try
            {
                var task = Task.Run(async () =>
                {
                    if (exceptionBefore) throw new ApplicationException();
                    await Task.Delay(timeout ? TimeSpan2 : TimeSpan1).ConfigureAwait(false);
                    if (exceptionAfter) throw new InvalidOperationException();
                });

                var completed = TaskUtils.WaitUntil(
                    task: task,
                    timestamp: DateTime.Now.Add(timeout ? TimeSpan1 : TimeSpan2));

                Assert.IsTrue(
                    ((!timeout) && (!exceptionBefore) && (!exceptionAfter) && (completed)) ||
                    ((timeout) && (!exceptionBefore) && (!exceptionAfter) && (!completed)) ||
                    ((timeout) && (!exceptionBefore) && (exceptionAfter) && (!completed)));
            }
            catch (AggregateException ex)
            {
                Assert.IsTrue(
                    ((!timeout) && (!exceptionBefore) && (exceptionAfter)) ||
                    ((!timeout) && (exceptionBefore) && (!exceptionAfter)) ||
                    ((timeout) && (exceptionBefore) && (!exceptionAfter)));

                Assert.AreEqual(exceptionBefore ? typeof(ApplicationException) : typeof(InvalidOperationException), ex.InnerException.GetType());
            }
        }

        #endregion

        #region FireAndForget

        private async static Task TestFireAndForgetAction(bool asynchronous, bool delay, bool exception)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            if (asynchronous)
            {
                TaskUtils.FireAndForgetAsync(
                    func: () => { if (exception) throw new ApplicationException(); taskCompletionSource.SetResult(false); return Task.Delay(0); },
                    dueTime: (delay) ? TimeSpan2 : TimeSpan.Zero,
                    notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
            }
            else
            {
                TaskUtils.FireAndForget(
                    func: () => { if (exception) throw new ApplicationException(); taskCompletionSource.SetResult(false); },
                    dueTime: (delay) ? TimeSpan2 : TimeSpan.Zero,
                    notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
            }

            await Task.Delay(TimeSpan1);

            Assert.AreNotEqual(delay, taskCompletionSource.Task.IsCompleted);

            Assert.AreEqual(exception, await taskCompletionSource.Task);
        }

        private async static Task TestFireAndForgetFunction(bool asynchronous, bool delay, bool exception)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            if (asynchronous)
            {
                TaskUtils.FireAndForgetAsync(
                    function: () => { if (exception) throw new ApplicationException(); return Task.FromResult(false); },
                    notifyCompletion: (value) => taskCompletionSource.SetResult(value),
                    dueTime: (delay) ? TimeSpan2 : TimeSpan.Zero,
                    notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
            }
            else
            {
                TaskUtils.FireAndForget(
                    function: () => { if (exception) throw new ApplicationException(); return false; },
                    notifyCompletion: (value) => taskCompletionSource.SetResult(value),
                    dueTime: (delay) ? TimeSpan2 : TimeSpan.Zero,
                    notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
            }

            await Task.Delay(TimeSpan1);

            Assert.AreNotEqual(delay, taskCompletionSource.Task.IsCompleted);

            Assert.AreEqual(exception, await taskCompletionSource.Task);
        }

        #endregion

        #endregion
    }
}
