using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Cannot run all tests in parallel in debug mode, the large number of tasks will break the elapsed time checks
            if (Debugger.IsAttached)
            {
                await TestAwaitActionOk();
                await TestAwaitActionOkAsync();
                await TestAwaitActionException();
                await TestAwaitActionExceptionAsync();
                await TestAwaitActionTimeout();
                await TestAwaitActionTimeoutAsync();
                await TestAwaitActionTimeoutNotify();
                await TestAwaitActionTimeoutNotifyAsync();
                await TestAwaitCancelableActionOk();
                await TestAwaitCancelableActionOkAsync();
                await TestAwaitCancelableActionException();
                await TestAwaitCancelableActionExceptionAsync();
                await TestAwaitCancelableActionTimeout();
                await TestAwaitCancelableActionTimeoutAsync();
                await TestAwaitCancelableActionTimeoutNotify();
                await TestAwaitCancelableActionTimeoutNotifyAsync();
                await TestAwaitFunctionOk();
                await TestAwaitFunctionOkAsync();
                await TestAwaitFunctionException();
                await TestAwaitFunctionExceptionAsync();
                await TestAwaitFunctionTimeout();
                await TestAwaitFunctionTimeoutAsync();
                await TestAwaitFunctionTimeoutNotify();
                await TestAwaitFunctionTimeoutNotifyAsync();
                await TestAwaitCancelableFunctionOk();
                await TestAwaitCancelableFunctionOkAsync();
                await TestAwaitCancelableFunctionException();
                await TestAwaitCancelableFunctionExceptionAsync();
                await TestAwaitCancelableFunctionTimeout();
                await TestAwaitCancelableFunctionTimeoutNotify();
                await TestAwaitCancelableFunctionTimeoutNotifyAsync();
            }
            else
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
            var taskList = new List<Task>();

            for (int index = 0; index < (2 << 6); index++)
            {
                taskList.Add(TestFireAndForget(
                    function: (index >> 5) % 2 == 0,
                    asynchronous: (index >> 4) % 2 == 0,
                    notify: (index >> 3) % 2 == 0,
                    notifyAsync: (index >> 2) % 2 == 0,
                    delay: (index >> 1) % 2 == 0,
                    exception: index % 2 == 0));

                // Cannot run all tests in parallel in debug mode, the large number of tasks will break the elapsed time checks
                if (Debugger.IsAttached)
                {
                    await Task.WhenAll(taskList[taskList.Count - 1]);
                }
            }

            await Task.WhenAll(taskList);
        }

        #region Internal methods

        private static readonly TimeSpan TimeSpan1 = TimeSpan.FromMilliseconds(Math.Max(25, 100 / Environment.ProcessorCount));
        private static readonly TimeSpan TimeSpan2 = TimeSpan.FromTicks(TimeSpan1.Ticks * 2);
        private static readonly TimeSpan TimeSpan4 = TimeSpan.FromTicks(TimeSpan2.Ticks * 2);

        #region Await Action

        private static async Task TestAwaitActionOk()
        {
            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));
        }

        private static async Task TestAwaitActionOkAsync()
        {
            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));
        }

        private static async Task TestAwaitActionException()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan4,
                    notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitActionExceptionAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan4,
                    notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitActionTimeout()
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

        private static async Task TestAwaitActionTimeoutAsync()
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

        private static async Task TestAwaitActionTimeoutNotify()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan2),
                timeout: TimeSpan1,
                notifyTimeout: (task) => finished = true);

            Assert.IsTrue(finished);
        }

        private static async Task TestAwaitActionTimeoutNotifyAsync()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async () => await Task.Delay(TimeSpan2),
                timeout: TimeSpan1,
                notifyTimeout: (task) => { finished = true; return Task.Delay(0); });

            Assert.IsTrue(finished);
        }

        private static async Task TestAwaitCancelableActionOk()
        {
            await TaskUtils.WaitTimeout(
                async (cancelToken) => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));
        }

        private static async Task TestAwaitCancelableActionOkAsync()
        {
            await TaskUtils.WaitTimeout(
                async (cancelToken) => await Task.Delay(TimeSpan1),
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));
        }

        private static async Task TestAwaitCancelableActionException()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan4,
                    notifyTimeout: (Action<Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitCancelableActionExceptionAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan4,
                    notifyTimeout: (Func<Task, Task>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitCancelableActionTimeout()
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

        private static async Task TestAwaitCancelableActionTimeoutAsync()
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

        private static async Task TestAwaitCancelableActionTimeoutNotify()
        {
            var finished = false;

            await TaskUtils.WaitTimeout(
                async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); },
                timeout: TimeSpan1,
                notifyTimeout: (task) => { finished = true; try { task.Wait(); } catch (AggregateException ex) { Assert.IsTrue(typeof(OperationCanceledException).IsAssignableFrom(ex.InnerException.GetType())); } });

            Assert.IsTrue(finished);
        }

        private static async Task TestAwaitCancelableActionTimeoutNotifyAsync()
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

        private static async Task TestAwaitFunctionOk()
        {
            var result = await TaskUtils.WaitTimeout(
                    func: async () => { await Task.Delay(TimeSpan1); return 1; },
                    timeout: TimeSpan2,
                    notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private static async Task TestAwaitFunctionOkAsync()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async () => { await Task.Delay(TimeSpan1); return 1; },
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private static async Task TestAwaitFunctionException()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan4,
                    notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitFunctionExceptionAsync()
        {
            try
            {
                await TaskUtils.WaitTimeout(
                    func: (Func<Task<int>>)(async () => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); }),
                    timeout: TimeSpan4,
                    notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitFunctionTimeout()
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

        private static async Task TestAwaitFunctionTimeoutAsync()
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

        private static async Task TestAwaitFunctionTimeoutNotify()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async () => { await Task.Delay(TimeSpan2); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: (task) => -1);

            Assert.AreEqual(-1, result);
        }

        private static async Task TestAwaitFunctionTimeoutNotifyAsync()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async () => { await Task.Delay(TimeSpan2); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: (task) => Task.FromResult(-1));

            Assert.AreEqual(-1, result);
        }

        private static async Task TestAwaitCancelableFunctionOk()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan1); return 1; },
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private static async Task TestAwaitCancelableFunctionOkAsync()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan1); return 1; },
                timeout: TimeSpan2,
                notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

            Assert.AreEqual(1, result);
        }

        private static async Task TestAwaitCancelableFunctionException()
        {
            try
            {
                var result = await TaskUtils.WaitTimeout(
                    func: async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); },
                    timeout: TimeSpan4,
                    notifyTimeout: (Func<Task, int>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitCancelableFunctionExceptionAsync()
        {
            try
            {
                var result = await TaskUtils.WaitTimeout(
                    func: (Func<CancellationToken, Task<int>>)(async (cancelToken) => { await Task.Delay(TimeSpan1); throw new InvalidOperationException(); }),
                    timeout: TimeSpan4,
                    notifyTimeout: (Func<Task, Task<int>>)((task) => throw new ApplicationException()));

                throw new ApplicationException();
            }
            catch (InvalidOperationException)
            {
                // Success
            }
        }

        private static async Task TestAwaitCancelableFunctionTimeout()
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

        private static async Task TestAwaitCancelableFunctionTimeoutAsync()
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

        private static async Task TestAwaitCancelableFunctionTimeoutNotify()
        {
            var result = await TaskUtils.WaitTimeout(
                func: async (cancelToken) => { await Task.Delay(TimeSpan2); cancelToken.ThrowIfCancellationRequested(); Assert.Fail(@"Cancellation token failed"); return 1; },
                timeout: TimeSpan1,
                notifyTimeout: (task) => { try { task.Wait(); } catch (AggregateException ex) { Assert.IsTrue(typeof(OperationCanceledException).IsAssignableFrom(ex.InnerException.GetType())); } return -1; });

            Assert.AreEqual(-1, result);
        }

        private static async Task TestAwaitCancelableFunctionTimeoutNotifyAsync()
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
                    await Task.Delay(timeout ? TimeSpan4 : TimeSpan1).ConfigureAwait(false);
                    if (exceptionAfter) throw new InvalidOperationException();
                });

                var completed = TaskUtils.WaitUntil(
                    task: task,
                    timestamp: DateTime.Now.Add(timeout ? TimeSpan1 : TimeSpan4));

                Assert.IsTrue(
                    ((!timeout) && (!exceptionBefore) && (!exceptionAfter) && (completed)) ||
                    ((timeout) && (!exceptionBefore) && (!exceptionAfter) && (!completed)) ||
                    ((timeout) && (!exceptionBefore) && (exceptionAfter) && (!completed)));

                try
                {
                    task.Wait();
                }
                catch (Exception)
                {
                    // Nothing to do
                }
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

        private static async Task TestFireAndForget(bool function, bool asynchronous, bool notify, bool notifyAsync, bool delay, bool exception)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            if (function)
            {
                if (notifyAsync)
                {
                    if (asynchronous)
                    {
                        TaskUtils.FireAndForgetAsync(
                            func: () => { Debug.WriteLine($"{DateTime.Now.Millisecond} Function started!"); if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); return Task.FromResult(false); },
                            notifyCompletion: (notify) ? ((value) => { Debug.WriteLine($"{DateTime.Now.Millisecond} Completion started!"); taskCompletionSource.SetResult(value); return Task.Delay(0); }) : (Func<bool, Task>)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Debug.WriteLine($"{DateTime.Now.Millisecond} Exception started!"); Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                    else
                    {
                        TaskUtils.FireAndForget(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); return false; },
                            notifyCompletion: (notify) ? ((value) => { taskCompletionSource.SetResult(value); return Task.Delay(0); }) : (Func<bool, Task>)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                }
                else
                {
                    if (asynchronous)
                    {
                        TaskUtils.FireAndForgetAsync(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); return Task.FromResult(false); },
                            notifyCompletion: (notify) ? ((value) => taskCompletionSource.SetResult(value)) : (Action<bool>)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                    else
                    {
                        TaskUtils.FireAndForget(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); return false; },
                            notifyCompletion: (notify) ? ((value) => taskCompletionSource.SetResult(value)) : (Action<bool>)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                }
            }
            else
            {
                if (notifyAsync)
                {
                    if (asynchronous)
                    {
                        TaskUtils.FireAndForgetAsync(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); return Task.Delay(0); },
                            notifyCompletion: (notify) ? (() => { taskCompletionSource.SetResult(false); return Task.Delay(0); }) : (Func<Task>)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                    else
                    {
                        TaskUtils.FireAndForget(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); },
                            notifyCompletion: (notify) ? (() => { taskCompletionSource.SetResult(false); return Task.Delay(0); }) : (Func<Task>)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                }
                else
                {
                    if (asynchronous)
                    {
                        TaskUtils.FireAndForgetAsync(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); return Task.Delay(0); },
                            notifyCompletion: (notify) ? (() => taskCompletionSource.SetResult(false)) : (Action)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                    else
                    {
                        TaskUtils.FireAndForget(
                            func: () => { if (exception) throw new ApplicationException(); if (!notify) taskCompletionSource.SetResult(false); },
                            notifyCompletion: (notify) ? (() => taskCompletionSource.SetResult(false)) : (Action)null,
                            dueTime: (!delay) ? TimeSpan.Zero : (exception) ? TimeSpan4 : TimeSpan2,
                            notifyException: (ex) => { Assert.AreEqual(typeof(ApplicationException), ex.GetType()); taskCompletionSource.SetResult(true); });
                    }
                }
            }

            await Task.Delay(exception ? TimeSpan2 : TimeSpan1);

            Assert.AreNotEqual(delay, taskCompletionSource.Task.IsCompleted);

            Assert.AreEqual(exception, await taskCompletionSource.Task);
        }

        #endregion

        #endregion
    }
}
