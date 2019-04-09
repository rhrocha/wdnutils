using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WDNUtils.Common
{
    /// <summary>
    /// Lock implementation for async methods, blocking reentrance in the asynchronous flow instead of allowing a deadlock to occur.
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

        private AsyncLocal<bool> BlockRecursion { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of AsyncLock
        /// </summary>
        public AsyncLock()
        {
            AwaitLockList = new Queue<TaskCompletionSource<object>>();
            Locked = false;
            BlockRecursion = new AsyncLocal<bool>();
        }

        #endregion

        #region Run async action with lock

        /// <summary>
        /// Run an async action with lock
        /// </summary>
        /// <param name="action">Method to be called with the lock acquired</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the notifyTimeout task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the notifyTimeout uses resources that does not support cross-thread operations (like UI components).</param>
        /// <exception cref="LockRecursionException">A reentrance in the asynchronous flow was detected and blocked (instead of allowing a deadlock to occur)</exception>
        /// <returns>Nothing (awaitable task)</returns>
        public async Task InvokeWithLock(Func<Task> action, bool continueOnCapturedContext = false)
        {
            var isOk = false;

            try
            {
                TaskCompletionSource<object> taskCompletionSource;

                try { }
                finally
                {
                    lock (_lock)
                    {
                        if (BlockRecursion.Value == true)
                            throw new LockRecursionException();

                        BlockRecursion.Value = true;

                        if (!Locked)
                        {
                            taskCompletionSource = null;
                            Thread.MemoryBarrier();
                            Locked = true;
                        }
                        else
                        {
                            taskCompletionSource = new TaskCompletionSource<object>();
                            AwaitLockList.Enqueue(taskCompletionSource);
                        }

                        Thread.MemoryBarrier();
                        isOk = true;
                    }
                }

                if (!(taskCompletionSource is null))
                {
                    await taskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext);
                }

                await action().ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                if (isOk)
                {
                    lock (_lock)
                    {
                        var taskCompletionSource = (AwaitLockList.Count <= 0) ? null : AwaitLockList.Dequeue();

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
        }

        #endregion

        #region Run async function with lock

        /// <summary>
        /// Run an async function with lock
        /// </summary>
        /// <param name="func">Function to be called with the lock acquired</param>
        /// <param name="continueOnCapturedContext">True to attempt to run the notifyTimeout task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the notifyTimeout uses resources that does not support cross-thread operations (like UI components).</param>
        /// <exception cref="LockRecursionException">A reentrance in the asynchronous flow was detected and blocked (instead of allowing a deadlock to occur)</exception>
        /// <returns>Result of the function</returns>
        public async Task<T> InvokeWithLock<T>(Func<Task<T>> func, bool continueOnCapturedContext = false)
        {
            var isOk = false;

            try
            {
                TaskCompletionSource<object> taskCompletionSource;

                try { }
                finally
                {
                    lock (_lock)
                    {
                        if (BlockRecursion.Value == true)
                            throw new LockRecursionException();

                        BlockRecursion.Value = true;

                        if (!Locked)
                        {
                            taskCompletionSource = null;
                            Thread.MemoryBarrier();
                            Locked = true;
                        }
                        else
                        {
                            taskCompletionSource = new TaskCompletionSource<object>();
                            AwaitLockList.Enqueue(taskCompletionSource);
                        }

                        Thread.MemoryBarrier();
                        isOk = true;
                    }
                }

                if (!(taskCompletionSource is null))
                {
                    await taskCompletionSource.Task.ConfigureAwait(continueOnCapturedContext);
                }

                return await func().ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                if (isOk)
                {
                    lock (_lock)
                    {
                        var taskCompletionSource = (AwaitLockList.Count <= 0) ? null : AwaitLockList.Dequeue();

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
        }

        #endregion
    }
}
