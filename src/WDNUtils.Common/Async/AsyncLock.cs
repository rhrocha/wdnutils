using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WDNUtils.Common
{
    /// <summary>
    /// Lock implementation for async methods
    /// </summary>
    /// <remarks>
    /// Calling async method inside a lock statement is not allowed:
    ///
    ///     private readonly object _lock = new object();
    ///
    ///     lock (_lock)
    ///     {
    ///         await action();
    ///     }
    ///
    /// Now this can be done:
    ///
    ///     private readonly AsyncLock _lock = new AsyncLock();
    ///
    ///     await _lock.InvokeWithLock(async () =>
    ///     {
    ///         await action();
    ///     });
    /// </remarks>
    public sealed class AsyncLock
    {
        #region Attributes

        private readonly object _lock = new object();

        #endregion

        #region Properties

        private Queue<TaskCompletionSource<object>> AwaitLockList { get; set; }

        private bool Locked { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of AsyncLock
        /// </summary>
        public AsyncLock()
        {
            AwaitLockList = new Queue<TaskCompletionSource<object>>();
            Locked = false;
        }

        #endregion

        #region Run async action with lock

        /// <summary>
        /// Run an async action with lock
        /// </summary>
        /// <param name="action">Method to be called with the lock acquired</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the notifyTimeout task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the notifyTimeout uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Nothing (awaitable task)</returns>
        public async Task InvokeWithLock(Func<Task> action, bool continueOnCapturedContext = false)
        {
            TaskCompletionSource<object> taskCompletionSource;

            lock (_lock)
            {
                if (!Locked)
                {
                    Locked = true;
                    taskCompletionSource = null;
                }
                else
                {
                    taskCompletionSource = new TaskCompletionSource<object>();
                    AwaitLockList.Enqueue(taskCompletionSource);
                }
            }

            try
            {
                if (!(taskCompletionSource is null))
                {
                    await taskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext);
                }

                await action().ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                lock (_lock)
                {
                    taskCompletionSource = (AwaitLockList.Count <= 0) ? null : AwaitLockList.Dequeue();

                    if (taskCompletionSource is null)
                    {
                        Locked = false;
                    }
                    else
                    {
                        taskCompletionSource.SetResult(null);
                    }
                }
            }
        }

        #endregion

        #region Run async function with lock

        /// <summary>
        /// Run an async function with lock
        /// </summary>
        /// <param name="func">Function to be called with the lock acquired</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the notifyTimeout task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the notifyTimeout uses resources that does not support cross-thread operations (like UI components).</param>
        /// <returns>Result of the function</returns>
        public async Task<T> InvokeWithLock<T>(Func<Task<T>> func, bool continueOnCapturedContext = false)
        {
            TaskCompletionSource<object> taskCompletionSource;

            lock (_lock)
            {
                if (!Locked)
                {
                    Locked = true;
                    taskCompletionSource = null;
                }
                else
                {
                    taskCompletionSource = new TaskCompletionSource<object>();
                    AwaitLockList.Enqueue(taskCompletionSource);
                }
            }

            try
            {
                if (!(taskCompletionSource is null))
                {
                    await taskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext);
                }

                return await func().ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                lock (_lock)
                {
                    taskCompletionSource = (AwaitLockList.Count <= 0) ? null : AwaitLockList.Dequeue();

                    if (taskCompletionSource is null)
                    {
                        Locked = false;
                    }
                    else
                    {
                        taskCompletionSource.SetResult(null);
                    }
                }
            }
        }

        #endregion
    }
}
