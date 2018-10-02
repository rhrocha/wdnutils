using System;
using System.Collections.Generic;
using System.Threading;

namespace WDNUtils.Common
{
    /// <summary>
    /// Cached property with automated refresh for referencing properties
    /// </summary>
    /// <typeparam name="T">Type of the property value</typeparam>
    public sealed class CachedProperty<T> : CachedProperty
    {
        #region Attributes

        /// <summary>
        /// Value factory
        /// </summary>
        private readonly Func<T> _valueFactory;

        /// <summary>
        /// Last generated value, null if not generated
        /// </summary>
        private Tuple<T> _currentValue;

        /// <summary>
        /// Current custom value, null if not defined
        /// </summary>
        private Tuple<T> _customValue;

        #endregion

        #region Properties

        /// <summary>
        /// The cached value for this property; the value is generated on demand using the value factory, unless a custom value was already set using the setter of this property
        /// </summary>
        public T Value
        {
            get
            {
                if (_currentValue is null)
                {
                    AddListenerReferencedProperties();
                    Thread.MemoryBarrier();
                    _currentValue = _customValue ?? Tuple.Create(_valueFactory());
                    Thread.MemoryBarrier();
                    NotifyDependencies();
                }

                return _currentValue.Item1;
            }

            set
            {
                _customValue = Tuple.Create(value);
                Thread.MemoryBarrier();
                _currentValue = null;
                Thread.MemoryBarrier();
                NotifyDependencies();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of CachedProperty with a specified value factory and references to other cached properties.
        /// When the values of the referenced properties are changed, the cached value of this class is discarded and a new
        /// one is generated on next access of Value property.
        /// </summary>
        /// <param name="valueFactory">Value factory that generates the values</param>
        /// <param name="references">Functions that returns the cached properties referenced by this property</param>
        /// <remarks>The referenced properties are retrieved using a Func&lt;CachedProperty&gt; instead of
        /// an object reference to prevent NullReferenceException when the static CachedProperty objects
        /// are declared out of the order in the static initialization</remarks>
        public CachedProperty(Func<T> valueFactory, params Func<CachedProperty>[] references)
            : base(references)
        {
            _valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
            _currentValue = null;
            _customValue = null;
        }

        #endregion

        #region Remove custom value, clear current value and notify dependent properties

        /// <summary>
        /// Remove custom value, clear current value and notify dependent properties
        /// </summary>
        public void ClearCustomValue()
        {
            _customValue = null;
            Thread.MemoryBarrier();
            _currentValue = null;
            Thread.MemoryBarrier();
            NotifyDependencies();
        }

        #endregion

        #region Clear current value and notify dependent properties

        /// <summary>
        /// Clear current value and notify dependent properties
        /// </summary>
        public override void Clear()
        {
            _currentValue = null;
            Thread.MemoryBarrier();
            NotifyDependencies();
        }

        #endregion
    }

    /// <summary>
    /// Cached property base class
    /// </summary>
    public abstract class CachedProperty
    {
        #region Events

        /// <summary>
        /// Event that is called when the values of the referenced properties are changed
        /// </summary>
        private event EventHandler PropertyChanged;

        #endregion

        #region Attributes

        /// <summary>
        /// Lock to prevent concurrency when adding the event handlers for the referenced properties
        /// </summary>
        private object _lock = new object();

        /// <summary>
        /// >Functions that returns the cached properties referenced by this property; null if the event handlers for the referenced properties were already added
        /// </summary>
        private Func<CachedProperty>[] _references;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the cached property base class
        /// </summary>
        /// <param name="references">Functions that returns the cached properties referenced by this property</param>
        /// <remarks>The referenced properties are retrieved using a Func&lt;CachedProperty&gt; instead of
        /// an object reference to prevent NullReferenceException when the static CachedProperty objects
        /// are declared out of the order in the static initialization</remarks>
        internal CachedProperty(Func<CachedProperty>[] references)
        {
            _references = references ?? throw new ArgumentNullException(nameof(references));
        }

        #endregion

        #region Add listeners into referenced properties

        /// <summary>
        /// Adding the event handlers for the referenced properties
        /// </summary>
        internal void AddListenerReferencedProperties()
        {
            var references = _references;

            if (references == null)
                return;

            lock (_lock)
            {
                references = _references;

                if (references == null)
                    return;

                try
                {
                    var properties = new List<CachedProperty>(references.Length);

                    foreach (var reference in references)
                    {
                        properties.Add(reference());
                    }

                    foreach (var property in properties)
                    {
                        property.PropertyChanged += Property_PropertyChanged;
                    }

                    _references = null;
                }
                catch (Exception)
                {
                    _references = references;
                    throw;
                }
            }
        }

        /// <summary>
        /// Event handler for PropertyChanged event of referenced properties
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">An object that contains no event data</param>
        private void Property_PropertyChanged(object sender, EventArgs e)
        {
            Clear();
        }

        #endregion

        #region Notify dependent properties that the value changed

        /// <summary>
        /// Notify dependent properties that the value changed
        /// </summary>
        internal void NotifyDependencies()
        {
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Clear current value and notify dependent properties

        /// <summary>
        /// Clear current value and notify dependent properties
        /// </summary>
        public abstract void Clear();

        #endregion
    }
}
