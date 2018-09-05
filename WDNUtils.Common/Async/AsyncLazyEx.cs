using System;
using System.Threading;
using System.Threading.Tasks;

namespace WDNUtils.Common
{
    /// <summary>
    /// Extension of <see cref="AsyncLazy{T}"/>, with configurable handling of value factory exceptions
    /// </summary>
    /// <typeparam name="T">The type of object that is being lazily initialized</typeparam>
    [Serializable]
    public class AsyncLazyEx<T> : AsyncLazy<T>
    {
        #region Attributes

        /// <summary>
        /// Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called again, instead of calling the value factory again
        /// </summary>
        [NonSerialized]
        private readonly bool? _captureException;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called again, instead of calling the value factory again
        /// </summary>
        protected override bool CaptureException(object valueFactory) => _captureException ?? base.CaptureException(valueFactory);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor for lazy initialization
        /// </summary>
        public AsyncLazyEx()
            : base()
        {
            _captureException = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor for lazy initialization
        /// </summary>
        /// <param name="captureException">Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called
        /// again, instead of calling the value factory again. If null, the default behavior for current mode is used (see <see cref="LazyThreadSafetyMode"/>)</param>
        public AsyncLazyEx(bool? captureException = null)
            : base()
        {
            _captureException = captureException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor and a specified thread-safety mode
        /// </summary>
        /// <param name="isThreadSafe">True if this instance should be usable by multiple threads concurrently; false if the instance will only be used by one thread at a time</param>
        /// <param name="captureException">Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called
        /// again, instead of calling the value factory again. If null, the default behavior for current mode is used (see <see cref="LazyThreadSafetyMode"/>)</param>
        public AsyncLazyEx(bool isThreadSafe, bool? captureException = null)
            : base(isThreadSafe: isThreadSafe)
        {
            _captureException = captureException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor and a specified thread-safety mode
        /// </summary>
        /// <param name="mode">The lazy thread-safety mode</param>
        /// <param name="captureException">Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called
        /// again, instead of calling the value factory again. If null, the default behavior for current mode is used (see <see cref="LazyThreadSafetyMode"/>)</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mode"/> contains an invalid value</exception>
        public AsyncLazyEx(LazyThreadSafetyMode mode, bool? captureException = null)
            : base(mode: mode)
        {
            _captureException = captureException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a specified initialization function
        /// </summary>
        /// <param name="valueFactory">The awaitable delegate invoked to produce the lazily-initialized value when it is needed</param>
        /// <param name="captureException">Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called
        /// again, instead of calling the value factory again. If null, the default behavior for current mode is used (see <see cref="LazyThreadSafetyMode"/>)</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference (Nothing in Visual Basic).</exception>
        public AsyncLazyEx(Func<Task<T>> valueFactory, bool? captureException = null)
            : base(valueFactory: valueFactory)
        {
            _captureException = captureException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a specified initialization function and a specified thread-safety mode
        /// </summary>
        /// <param name="valueFactory">The awaitable delegate invoked to produce the lazily-initialized value when it is needed</param>
        /// <param name="isThreadSafe">True if this instance should be usable by multiple threads concurrently; false if the instance will only be used by one thread at a time</param>
        /// <param name="captureException">Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called
        /// again, instead of calling the value factory again. If null, the default behavior for current mode is used (see <see cref="LazyThreadSafetyMode"/>)</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference</exception>
        public AsyncLazyEx(Func<Task<T>> valueFactory, bool isThreadSafe, bool? captureException = null)
            : base(valueFactory: valueFactory, isThreadSafe: isThreadSafe)
        {
            _captureException = captureException;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a specified initialization function and a specified thread-safety mode
        /// </summary>
        /// <param name="valueFactory">The awaitable delegate invoked to produce the lazily-initialized value when it is needed</param>
        /// <param name="mode">The lazy thread-safety mode</param>
        /// <param name="captureException">Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called
        /// again, instead of calling the value factory again. If null, the default behavior for current mode is used (see <see cref="LazyThreadSafetyMode"/>)</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mode"/> contains an invalid value</exception>
        public AsyncLazyEx(Func<Task<T>> valueFactory, LazyThreadSafetyMode mode, bool? captureException = null)
            : base(valueFactory: valueFactory, mode: mode)
        {
            _captureException = captureException;
        }

        #endregion
    }
}
