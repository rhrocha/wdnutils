using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WDNUtils.Common
{
    /// <summary>
    /// Task extensions
    /// </summary>
    public static class TaskUtils
    {
        #region Await with timeout

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Nothing (awaitable task)</returns>
        public async static Task WaitTimeout(Func<Task> func, TimeSpan timeout, Action<Task> notifyTimeout = null, bool continueOnCapturedContext = false)
        {
            var task = func();

            await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

            if (task.IsCompleted)
            {
                await task.ConfigureAwait(continueOnCapturedContext);
                return;
            }

            if (notifyTimeout is null)
                throw new TimeoutException();

            notifyTimeout(task);
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Nothing (awaitable task)</returns>
        public async static Task WaitTimeout(Func<Task> func, TimeSpan timeout, Func<Task, Task> notifyTimeout, bool continueOnCapturedContext = false)
        {
            var task = func();

            await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

            if (task.IsCompleted)
            {
                await task.ConfigureAwait(continueOnCapturedContext);
                return;
            }

            if (notifyTimeout is null)
                throw new TimeoutException();

            await notifyTimeout(task).ConfigureAwait(continueOnCapturedContext);
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Result of the awaited task (awaitable task)</returns>
        public async static Task<T> WaitTimeout<T>(Func<Task<T>> func, TimeSpan timeout, Func<Task, T> notifyTimeout = null, bool continueOnCapturedContext = false)
        {
            var task = func();

            await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

            if (task.IsCompleted)
                return await task.ConfigureAwait(continueOnCapturedContext);

            if (notifyTimeout is null)
                throw new TimeoutException();

            return notifyTimeout(task);
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Result of the awaited task (awaitable task)</returns>
        public async static Task<T> WaitTimeout<T>(Func<Task<T>> func, TimeSpan timeout, Func<Task, Task<T>> notifyTimeout, bool continueOnCapturedContext = false)
        {
            var task = func();

            await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

            if (task.IsCompleted)
                return await task.ConfigureAwait(continueOnCapturedContext);

            if (notifyTimeout is null)
                throw new TimeoutException();

            return await notifyTimeout(task).ConfigureAwait(continueOnCapturedContext);
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Nothing (awaitable task)</returns>
        public async static Task WaitTimeout(Func<CancellationToken, Task> func, TimeSpan timeout, Action<Task> notifyTimeout = null, bool continueOnCapturedContext = false)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = func(cancellationTokenSource.Token);

                await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

                if (task.IsCompleted)
                {
                    await task.ConfigureAwait(continueOnCapturedContext);
                    return;
                }

                cancellationTokenSource.Cancel();

                if (notifyTimeout is null)
                    throw new TimeoutException();

                notifyTimeout(task);
            }
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Nothing (awaitable task)</returns>
        public async static Task WaitTimeout(Func<CancellationToken, Task> func, TimeSpan timeout, Func<Task, Task> notifyTimeout, bool continueOnCapturedContext = false)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = func(cancellationTokenSource.Token);

                await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

                if (task.IsCompleted)
                {
                    await task.ConfigureAwait(continueOnCapturedContext);
                    return;
                }

                cancellationTokenSource.Cancel();

                if (notifyTimeout is null)
                    throw new TimeoutException();

                await notifyTimeout(task).ConfigureAwait(continueOnCapturedContext);
            }
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Result of the awaited task (awaitable task)</returns>
        public async static Task<T> WaitTimeout<T>(Func<CancellationToken, Task<T>> func, TimeSpan timeout, Func<Task, T> notifyTimeout = null, bool continueOnCapturedContext = false)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = func(cancellationTokenSource.Token);

                await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

                if (task.IsCompleted)
                    return await task.ConfigureAwait(continueOnCapturedContext);

                cancellationTokenSource.Cancel();

                if (notifyTimeout is null)
                    throw new TimeoutException();

                return notifyTimeout(task);
            }
        }

        /// <summary>
        /// Await an async task until a specified timeout
        /// </summary>
        /// <param name="func">Task to be awaited for</param>
        /// <param name="timeout">Timeout for the task await</param>
        /// <param name="notifyTimeout">Action to be called if the task did not finish before the timeout</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Result of the awaited task (awaitable task)</returns>
        public async static Task<T> WaitTimeout<T>(Func<CancellationToken, Task<T>> func, TimeSpan timeout, Func<Task, Task<T>> notifyTimeout, bool continueOnCapturedContext = false)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = func(cancellationTokenSource.Token);

                await Task.WhenAny(task, Task.Delay((timeout < TimeSpan.Zero) ? TimeSpan.Zero : timeout)).ConfigureAwait(continueOnCapturedContext);

                if (task.IsCompleted)
                    return await task.ConfigureAwait(continueOnCapturedContext);

                cancellationTokenSource.Cancel();

                if (notifyTimeout is null)
                    throw new TimeoutException();

                return await notifyTimeout(task).ConfigureAwait(continueOnCapturedContext);
            }
        }

        #endregion

        #region Wait until timestamp

        /// <summary>
        /// Waits for the Task to complete execution until a specified date/time
        /// </summary>
        /// <param name="task">Task to be waited for</param>
        /// <param name="timestamp">Date/time to wait until</param>
        /// <returns>True if the Task completed execution until the specified date/time; otherwise, false</returns>
        public static bool WaitUntil(this Task task, DateTime timestamp)
        {
            var remainingTime = timestamp - DateTime.Now;

            return task.Wait(
                (remainingTime < TimeSpan.Zero) ? TimeSpan.Zero :
                (remainingTime.TotalMilliseconds > int.MaxValue) ? TimeSpan.FromMilliseconds(int.MaxValue) :
                remainingTime);
        }

        #endregion

        #region Fire and forget

        /// <summary>
        /// Run an action after a specified due time, without awaiting for its completion
        /// </summary>
        /// <param name="func">Action to run</param>
        /// <param name="dueTime">Delay before running the action</param>
        /// <param name="notifyException">Exception handler for the action (if null, the exception will be displayed in the debug output)</param>
        public static void FireAndForget(Action func, TimeSpan dueTime = default(TimeSpan), Action<Exception> notifyException = null)
        {
            Task.Run(async () =>
            {
                if (dueTime > TimeSpan.Zero)
                {
                    await Task.Delay(dueTime).ConfigureAwait(false);
                }

                try
                {
                    func();
                }
                catch (Exception ex)
                {
                    (notifyException ?? DebugDisplayException).Invoke(ex);
                }
            });
        }

        /// <summary>
        /// Run an async action after a specified due time, without awaiting for its completion
        /// </summary>
        /// <param name="func">Action to run</param>
        /// <param name="dueTime">Delay before running the action</param>
        /// <param name="notifyException">Exception handler for the action (if null, the exception will be displayed in the debug output)</param>
        public static void FireAndForgetAsync(Func<Task> func, TimeSpan dueTime = default(TimeSpan), Action<Exception> notifyException = null)
        {
            Task.Run(async () =>
            {
                if (dueTime > TimeSpan.Zero)
                {
                    await Task.Delay(dueTime).ConfigureAwait(false);
                }

                try
                {
                    await func().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    (notifyException ?? DebugDisplayException).Invoke(ex);
                }
            });
        }

        /// <summary>
        /// Run a function after a specified due time, without awaiting for its completion
        /// </summary>
        /// <param name="function">Function to run</param>
        /// <param name="notifyCompletion">Listener to receive the value returned by the function</param>
        /// <param name="dueTime">Delay before running the function</param>
        /// <param name="notifyException">Exception handler for the action (if null, the exception will be displayed in the debug output)</param>
        public static void FireAndForget<T>(Func<T> function, Action<T> notifyCompletion, TimeSpan dueTime = default(TimeSpan), Action<Exception> notifyException = null)
        {
            Task.Run(async () =>
            {
                if (dueTime > TimeSpan.Zero)
                {
                    await Task.Delay(dueTime).ConfigureAwait(false);
                }

                try
                {
                    var value = function();

                    notifyCompletion?.Invoke(value);
                }
                catch (Exception ex)
                {
                    (notifyException ?? DebugDisplayException).Invoke(ex);
                }
            });
        }

        /// <summary>
        /// Run an async function after a specified due time, without awaiting for its completion
        /// </summary>
        /// <param name="function">Function to run</param>
        /// <param name="notifyCompletion">Listener to receive the value returned by the function</param>
        /// <param name="dueTime">Delay before running the function</param>
        /// <param name="notifyException">Exception handler for the action (if null, the exception will be displayed in the debug output)</param>
        public static void FireAndForgetAsync<T>(Func<Task<T>> function, Action<T> notifyCompletion, TimeSpan dueTime = default(TimeSpan), Action<Exception> notifyException = null)
        {
            Task.Run(async () =>
            {
                if (dueTime > TimeSpan.Zero)
                {
                    await Task.Delay(dueTime).ConfigureAwait(false);
                }

                try
                {
                    var value = await function().ConfigureAwait(false);

                    notifyCompletion?.Invoke(value);
                }
                catch (Exception ex)
                {
                    (notifyException ?? DebugDisplayException).Invoke(ex);
                }
            });
        }

        /// <summary>
        /// Displays exception information in the debug output
        /// </summary>
        /// <param name="ex">Exception to be displayed</param>
        private static void DebugDisplayException(Exception ex)
        {
            try
            {
                Debug.WriteLine($@"{ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
            catch (Exception)
            {
                // Nothing to do
            }
        }

        #endregion
    }
}
