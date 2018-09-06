using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace WDNUtils.Common
{
    /// <summary>
    /// Provides support for async lazy initialization (asynchronous implementation of <see cref="Lazy{T}"/>)
    /// </summary>
    /// <typeparam name="T">The type of object that is being lazily initialized</typeparam>
    [Serializable]
    public class AsyncLazy<T>
    {
        #region Attributes

        /// <summary>
        /// Contains null if the value was not initialized, the created value in a <see cref="Tuple{T}"/>, or <see cref="ExceptionDispatchInfo"/> if an exception was thrown when creating the value
        /// </summary>
        private object _value;

        /// <summary>
        /// The value factory, null if the value was already created
        /// </summary>
        [NonSerialized]
        private Func<Task<T>> _valueFactory;

        /// <summary>
        /// Semaphore to synchronize the creation/publication of values in the mode <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>
        /// </summary>
        [NonSerialized]
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// Current execution context flag to detect recursive reentrancy in the method <see cref="AsyncLazy{T}.GetValueAsync"/> for the modes <see cref="LazyThreadSafetyMode.None"/> and <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>
        /// </summary>
        [NonSerialized]
        private readonly AsyncLocal<bool> _blockRecursion;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the <see cref="AsyncLazy{T}"/> has been initialized
        /// </summary>
        public bool IsValueCreated => (_value != null) && (_value is Tuple<T>);

        /// <summary>
        /// Indicates if exceptions threw by the value factory should be captured to be thrown when GetValueAsync is called again, instead of calling the value factory again.
        /// By default, it is disabled in the mode <see cref="LazyThreadSafetyMode.PublicationOnly"/>, or if the value factory is the T's default constructor
        /// </summary>
        protected virtual bool CaptureException(object valueFactory) =>
            (!(_blockRecursion is null)) && // Mode is not LazyThreadSafetyMode.PublicationOnly
            (!ReferenceEquals(valueFactory, DefaultCreateValue)); // Value factory is not the T's default constructor

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a specified initialization function and a specified thread-safety mode
        /// </summary>
        /// <param name="valueFactory">The awaitable delegate invoked to produce the lazily-initialized value when it is needed</param>
        /// <param name="mode">The lazy thread-safety mode</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mode"/> contains an invalid value</exception>
        public AsyncLazy(Func<Task<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            _value = null;
            _valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));

            if (mode == LazyThreadSafetyMode.ExecutionAndPublication)
            {
                _semaphore = new SemaphoreSlim(1);
                _blockRecursion = new AsyncLocal<bool>();
            }
            else if (mode == LazyThreadSafetyMode.PublicationOnly)
            {
                _semaphore = null;
                _blockRecursion = null;
            }
            else if (mode == LazyThreadSafetyMode.None)
            {
                _semaphore = null;
                _blockRecursion = new AsyncLocal<bool>();
            }
            else
                throw new ArgumentOutOfRangeException(nameof(mode));
        }

        #region Other Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor for lazy initialization
        /// </summary>
        public AsyncLazy()
            : this(valueFactory: DefaultCreateValue, mode: LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor and a specified thread-safety mode
        /// </summary>
        /// <param name="isThreadSafe">True if this instance should be usable by multiple threads concurrently; false if the instance will only be used by one thread at a time</param>
        public AsyncLazy(bool isThreadSafe)
            : this(valueFactory: DefaultCreateValue, mode: isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses <typeparamref name="T"/>'s default constructor and a specified thread-safety mode
        /// </summary>
        /// <param name="mode">The lazy thread-safety mode</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="mode"/> contains an invalid value</exception>
        public AsyncLazy(LazyThreadSafetyMode mode)
            : this(valueFactory: DefaultCreateValue, mode: mode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a specified initialization function
        /// </summary>
        /// <param name="valueFactory">The awaitable delegate invoked to produce the lazily-initialized value when it is needed</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference (Nothing in Visual Basic).</exception>
        public AsyncLazy(Func<Task<T>> valueFactory)
            : this(valueFactory: valueFactory, mode: LazyThreadSafetyMode.ExecutionAndPublication)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class that uses a specified initialization function and a specified thread-safety mode
        /// </summary>
        /// <param name="valueFactory">The awaitable delegate invoked to produce the lazily-initialized value when it is needed</param>
        /// <param name="isThreadSafe">True if this instance should be usable by multiple threads concurrently; false if the instance will only be used by one thread at a time</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a null reference</exception>
        public AsyncLazy(Func<Task<T>> valueFactory, bool isThreadSafe)
            : this(valueFactory: valueFactory, mode: isThreadSafe ? LazyThreadSafetyMode.ExecutionAndPublication : LazyThreadSafetyMode.None)
        {
        }

        #endregion

        #endregion

        #region Serialization support

        /// <summary>
        /// Forces initialization during serialization
        /// </summary> 
        /// <param name="context">The StreamingContext for the serialization operation</param> 
        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            Task.Run(async () => await GetValueAsync().ConfigureAwait(false)).Wait();
        }

        #endregion

        #region Default constructor for generic parameter type

        /// <summary>
        /// Default constructor for generic parameter type
        /// </summary>
        protected static readonly Func<Task<T>> DefaultCreateValue =
            () =>
            {
                try
                {
                    return Task.FromResult<T>((T)Activator.CreateInstance(typeof(T)));
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            };

        #endregion

        #region ToString implementation

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return ((!(_value is null)) && (_value is Tuple<T> value))
                ? value.Item1?.ToString()
                : null;
        }

        #endregion

        #region Get value

        /// <summary>
        /// Gets the lazily initialized value of the current <see cref="AsyncLazy{T}"/>
        /// </summary>
        /// <param name="continueOnCapturedContext">True to attempt to run the value factory task in the captured context; default is false, to prevent deadlocks.
        /// Should be true only if the value factory uses resources that does not support cross-thread operations (like UI components). Applied only for mode <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/> when acquiring the synchronization semaphore lock; all other modes never switch context before calling the value factory.</param>
        /// <returns>The lazily initialized value of the current <see cref="AsyncLazy{T}"/></returns>
        /// <exception cref="MemberAccessException">The <see cref="AsyncLazy{T}"/> instance is initialized to use the default constructor of the type that is being lazily initialized, and permissions to access the constructor are missing</exception>
        /// <exception cref="MissingMemberException">The <see cref= "AsyncLazy{T}" /> instance is initialized to use the default constructor of the type that is being lazily initialized, and that type does not have a public, parameterless constructor</exception>
        /// <exception cref="InvalidOperationException">The initialization function tries to access<see cref="AsyncLazy{T}.GetValueAsync"/> on this instance</exception>
        /// <remarks>Please <see cref="LazyThreadSafetyMode"/> for more information on how <see cref="AsyncLazy{T}"/> will behave if an exception is thrown from initialization delegate</remarks>
        public async Task<T> GetValueAsync(bool continueOnCapturedContext = false)
        {
            #region Check if the value was already initialized

            if (_value is Tuple<T> value)
            {
                return value.Item1;
            }
            else if (_value is ExceptionDispatchInfo ex)
            {
                ex.Throw();
                throw new Exception(); // Just to prevent compiler warnings, since ex.Throw() will throw an exception
            }

            #endregion

            if (_blockRecursion is null)
            {
                #region LazyThreadSafetyMode.PublicationOnly

                var valueFactory = _valueFactory;

                // Prevent calling the value factory after a value is created successfully (may fail to block in concurrent calls, by design)
                if (_valueFactory is null)
                {
                    if (_value is ExceptionDispatchInfo ex)
                    {
                        ex.Throw();
                        throw new Exception(); // Just to prevent compiler warnings, since ex.Throw() will throw an exception
                    }
                    else
                    {
                        return ((Tuple<T>)_value).Item1;
                    }
                }

                try
                {
                    value = new Tuple<T>(await valueFactory().ConfigureAwait(continueOnCapturedContext));

                    // Block concurrent or new publications of the created value
                    if (!InterlockedEx.TryCompareExchange(ref _value, value, null))
                        return ((Tuple<T>)_value).Item1;

                    // Prevent calling the value factory after a value is created successfully
                    Interlocked.MemoryBarrier();
                    _valueFactory = null;

                    return value.Item1;
                }
                catch (Exception ex)
                {
                    if (CaptureException(valueFactory))
                    {
                        // Block concurrent or new publications of the created value
                        if (InterlockedEx.TryCompareExchange(ref _value, ExceptionDispatchInfo.Capture(ex), null))
                        {
                            // Prevent calling the value factory after a value is created successfully
                            Interlocked.MemoryBarrier();
                            _valueFactory = null;
                        }
                        else
                        {
                            if (_value is ExceptionDispatchInfo ex2)
                            {
                                ex2.Throw();
                                throw new Exception(); // Just to prevent compiler warnings, since ex.Throw() will throw an exception
                            }
                            else
                            {
                                return ((Tuple<T>)_value).Item1;
                            }
                        }
                    }

                    throw;
                }

                #endregion
            }
            else if (_semaphore is null)
            {
                #region LazyThreadSafetyMode.None

                // #2 - Allow concurrent calls to the value factory
                // #3 - Allow concurrent publications of the created value
                // #4 - Allow new publications of the created value
                // #5 - Prevent calling the value factory after a value is created successfully

                var valueFactory = _valueFactory;

                // Prevent calling the value factory after a value is created successfully (may fail to block in concurrent calls, by design)
                if (_valueFactory is null)
                {
                    if (_value is ExceptionDispatchInfo ex)
                    {
                        ex.Throw();
                        throw new Exception(); // Just to prevent compiler warnings, since ex.Throw() will throw an exception
                    }
                    else
                    {
                        return ((Tuple<T>)_value).Item1;
                    }
                }

                // Block recursive reentrant calls to the value factory
                if (_blockRecursion.Value == true)
                    throw new InvalidOperationException();

                _blockRecursion.Value = true;

                try
                {
                    value = Tuple.Create(await valueFactory().ConfigureAwait(continueOnCapturedContext));

                    _value = value;

                    // Prevent calling the value factory after a value is created successfully (may fail to block in concurrent calls, by design)
                    Interlocked.MemoryBarrier();
                    _valueFactory = null;

                    return value.Item1;
                }
                catch (Exception ex)
                {
                    if (CaptureException(valueFactory))
                    {
                        _value = ExceptionDispatchInfo.Capture(ex);

                        // Prevent calling the value factory after a value is created successfully (may fail to block in concurrent calls, by design)
                        Interlocked.MemoryBarrier();
                        _valueFactory = null;
                    }

                    throw;
                }

                #endregion
            }
            else
            {
                #region LazyThreadSafetyMode.ExecutionAndPublication

                var valueFactory = _valueFactory;

                // Prevent concurrent calls to the value factory, and concurrent or new publications of the created value, without using the semaphore
                if (valueFactory is null)
                {
                    if (_value is Tuple<T> value2)
                    {
                        return value2.Item1;
                    }
                    else if (_value is ExceptionDispatchInfo ex)
                    {
                        ex.Throw();
                        throw new Exception(); // Just to prevent compiler warnings, since ex.Throw() will throw an exception
                    }
                }

                // Block recursive reentrant calls to the value factory
                if (_blockRecursion.Value == true)
                    throw new InvalidOperationException();

                _blockRecursion.Value = true;

                // Block concurrent calls to the value factory, and concurrent or new publications of the created value
                await _semaphore.WaitAsync().ConfigureAwait(continueOnCapturedContext);

                try
                {
                    if (_value is Tuple<T> value2)
                    {
                        return value2.Item1;
                    }
                    else if (_value is ExceptionDispatchInfo ex)
                    {
                        ex.Throw();
                        throw new Exception(); // Just to prevent compiler warnings, since ex.Throw() will throw an exception
                    }

                    try
                    {
                        value = Tuple.Create(await valueFactory().ConfigureAwait(continueOnCapturedContext));

                        _value = value;

                        // Prevent concurrent calls to the value factory, and concurrent or new publications of the created value, without using the semaphore
                        Thread.MemoryBarrier();
                        _valueFactory = null;

                        return value.Item1;
                    }
                    catch (Exception ex)
                    {
                        if (CaptureException(valueFactory))
                        {
                            _value = ExceptionDispatchInfo.Capture(ex);

                            // Prevent concurrent calls to the value factory, and concurrent or new publications of the created value, without using the semaphore
                            Thread.MemoryBarrier();
                            _valueFactory = null;
                        }

                        throw;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }

                #endregion
            }
        }

        #endregion
    }
}
