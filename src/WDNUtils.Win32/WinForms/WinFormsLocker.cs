using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WDNUtils.Common;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Thread-safe form locker for WinForms
    /// </summary>
    /// <remarks>
    /// The form locker instance must be created in the constructor after calling InitializeComponent(),
    /// because it will scan all form controls that should be locked/unlocked.
    /// </remarks>
    public sealed class WinFormsLocker : IFormLocker
    {
        #region Constants

        /// <summary>
        /// Control types to be searched by the control scanner
        /// </summary>
        private static readonly IList<Type> LockableControls = new List<Type>()
        {
            typeof(ButtonBase),
            typeof(DataGridView),
            typeof(DateTimePicker),
            typeof(ListControl),
            typeof(ListView),
            typeof(MenuStrip),
            typeof(MonthCalendar),
            typeof(ScrollBar),
            typeof(TextBoxBase),
            typeof(ToolBar),
            typeof(ToolStrip),
            typeof(TreeView),
            typeof(WebBrowserBase),
            typeof(ILockableControl)
        }.AsReadOnly();

        private static readonly IList<Type> RefreshLayoutControls = new List<Type>()
        {
            typeof(DataGridView)
        }.AsReadOnly();

        private static readonly Func<bool, bool> DefaultGetLocked = (locked) => locked;

        private const int LockStatus_Unlocked = 0;
        private const int LockStatus_Changing = 1;
        private const int LockStatus_Locked = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if the form is locked
        /// </summary>
        public bool IsLocked => (lockStatus != LockStatus_Unlocked);

        private Control ActiveControlAfterUnlock { get; set; } = null;

        /// <summary>
        /// The control that should be active when the form is unlocked
        /// </summary>
        public object ActiveControl
        {
            get => (IsLocked) ? ActiveControlAfterUnlock : Form.ActiveControl;

            set
            {
                if (!(value is Control control))
                    throw new InvalidCastException();

                if (!IsLocked)
                {
                    ActiveControlAfterUnlock = control;
                }
                else
                {
                    Form.ActiveControl = control;
                }
            }
        }

        internal Form Form { get; private set; }
        private Dictionary<Control, Func<bool, bool>> ControlList { get; set; }
        private TextBox KeepFormFocus { get; set; }

        #endregion

        #region Attributes

        private readonly object _lockControlList = new object();
        private long _lockCount = 0;
        private int lockStatus = LockStatus_Unlocked;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of WinFormsLocker
        /// </summary>
        /// <param name="form">Form to be locked/unlocked (null to create a dummy form locker)</param>
        /// <param name="addKeepFormFocus">Indicates if a read-only hidden textbox should be added to keep the keyboard focus when all controls of the form are disabled</param>
        public WinFormsLocker(Form form, bool addKeepFormFocus = true)
        {
            Form = form;

            if ((addKeepFormFocus) && (!(Form is null)))
            {
                KeepFormFocus = new TextBox()
                {
                    Location = new Point(Screen.AllScreens.Min(screen => screen.Bounds.Left) - 8000, Screen.AllScreens.Min(screen => screen.Bounds.Top) - 8000),
                    Name = $@"{form.Name}_{nameof(WinFormsLocker)}_{nameof(KeepFormFocus)}",
                    ReadOnly = true,
                    TabStop = false,
                    Size = new Size(100, 25),
                    TabIndex = int.MaxValue
                };

                Form.Controls.Add(KeepFormFocus);
            }
            else
            {
                KeepFormFocus = null;
            }

            ScanControls();
        }

        #endregion

        #region Scan form controls

        /// <summary>
        /// Scan the controls of the form that should be locked/unlocked by this class
        /// </summary>
        /// <returns>A reference to this instance</returns>
        public IFormLocker ScanControls(bool resetCustom = false)
        {
            var newControlList = new Dictionary<Control, Func<bool, bool>>(
                comparer: new ObjectReferenceEqualityComparer<Control>());

            if (!(KeepFormFocus is null))
            {
                newControlList[KeepFormFocus] = null;
            }

            lock (_lockControlList)
            {
                var customControlList = (resetCustom) ? null :
                    ControlList
                        .Where(item => (!ReferenceEquals(item.Value, DefaultGetLocked)))
                        .ToDictionary(
                            item => item.Key,
                            item => item.Value,
                            new ObjectReferenceEqualityComparer<Control>());

                var controlStack = new Stack<Control>(Form?.Controls?.OfType<Control>() ?? Enumerable.Empty<Control>());

                while (controlStack.Count > 0)
                {
                    var control = controlStack.Pop();

                    if ((control is null) || (newControlList.ContainsKey(control)))
                        continue;

                    if ((LockableControls.Any(type => type.IsAssignableFrom(control.GetType()))) || (customControlList?.ContainsKey(control) == true))
                    {
                        newControlList[control] = customControlList?.GetOrDefault(control, DefaultGetLocked);
                    }
                    else if (control.Controls?.Count > 0)
                    {
                        foreach (var subControl in control.Controls.OfType<Control>())
                        {
                            controlStack.Push(subControl);
                        }
                    }
                }

                ControlList = newControlList;
            }

            return this;
        }

        #endregion

        #region Set custom locking conditions for the controls

        /// <summary>
        /// Sets a custom locking contidion for the controls
        /// </summary>
        /// <param name="getLocked">Custom locking condition (if null, the default locking will be used)</param>
        /// <param name="controls">List of controls</param>
        /// <returns>A reference to this instance</returns>
        public IFormLocker SetControlLocking(Func<bool, bool> getLocked, params object[] controls)
        {
            lock (_lockControlList)
            {
                var newControlList = new Dictionary<Control, Func<bool, bool>>(
                    dictionary: ControlList,
                    comparer: new ObjectReferenceEqualityComparer<Control>());

                foreach (var control in controls)
                {
                    if ((!(control is Control winformsControl)) || (ReferenceEquals(control, KeepFormFocus)))
                        continue;

                    newControlList[winformsControl] = getLocked ?? DefaultGetLocked;
                }

                ControlList = newControlList;
            }

            return this;
        }

        #endregion

        #region Remove a control from the locking list

        /// <summary>
        /// Removes a control from the locking list (also from the custom locking list)
        /// </summary>
        /// <param name="controls">List of controls</param>
        /// <returns>A reference to this instance</returns>
        public IFormLocker RemoveControl(params object[] controls)
        {
            lock (_lockControlList)
            {
                var newControlList = new Dictionary<Control, Func<bool, bool>>(
                    dictionary: ControlList,
                    comparer: new ObjectReferenceEqualityComparer<Control>());

                foreach (var control in controls)
                {
                    if ((!(control is Control winformsControl)) || (ReferenceEquals(control, KeepFormFocus)))
                        continue;

                    newControlList[winformsControl] = null;
                }

                ControlList = newControlList;
            }

            return this;
        }

        #endregion

        #region Lock/unlock controls

        /// <summary>
        /// Update the status of the form controls and the mouse cursor
        /// </summary>
        /// <param name="locked">Indicate if the controls should be locked (disabled or set as readonly) or unlocked (enabled or unset as readonly)</param>
        /// <param name="ignoreExceptions">Indicates if exceptions should be ignored (internal usage only)</param>
        private void SetLockedStatusControls(bool locked, bool ignoreExceptions = false)
        {
            try
            {
                if (locked)
                {
                    #region Set wait mouse cursor

                    try
                    {
                        if (Form.Cursor != Cursors.WaitCursor)
                        {
                            Form.Cursor = Cursors.WaitCursor;
                        }
                    }
                    catch (Exception)
                    {
                        if (!ignoreExceptions)
                            throw;
                    }

                    #endregion

                    #region Enable TabStop for the KeepFormFocus text box

                    try
                    {
                        if (KeepFormFocus?.TabStop == false)
                        {
                            KeepFormFocus.TabStop = true;
                        }
                    }
                    catch (Exception)
                    {
                        if (!ignoreExceptions)
                            throw;
                    }

                    #endregion
                }

                #region Update status of all form controls

                var controlList = ControlList;

                var refreshLayoutControlList = new List<Control>(controlList.Count);

                foreach (var item in controlList)
                {
                    try
                    {
                        #region Check control and getLocked method

                        if ((!(item.Value is Func<bool, bool> getLocked)) || (!(item.Key is Control control)))
                            continue;

                        var controlLocked = getLocked(locked);

                        #endregion

                        #region Update control status

                        if (control is TextBoxBase textBox)
                        {
                            if (textBox.ReadOnly == controlLocked)
                                continue;

                            #region Change text box ReadOnly and TabStop properties

                            try
                            {
                                textBox.ReadOnly = controlLocked;
                            }
                            catch (Exception)
                            {
                                if (!ignoreExceptions)
                                    throw;
                            }

                            try
                            {
                                textBox.TabStop = !controlLocked;
                            }
                            catch (Exception)
                            {
                                if (!ignoreExceptions)
                                    throw;
                            }

                            #endregion
                        }
                        else
                        {
                            if (control.Enabled != controlLocked)
                                continue;

                            #region Change control Enabled property

                            try
                            {
                                control.Enabled = !controlLocked;
                            }
                            catch (Exception)
                            {
                                if (!ignoreExceptions)
                                    throw;
                            }

                            #endregion

                            #region Check if the control layout must be refreshed

                            if (!controlLocked)
                            {
                                var controlType = control.GetType();

                                if (RefreshLayoutControls.Any(type => type.IsAssignableFrom(controlType)))
                                {
                                    refreshLayoutControlList.Add(control);
                                }
                            }

                            #endregion
                        }

                        #endregion
                    }
                    catch (Exception)
                    {
                        if (!ignoreExceptions)
                            throw;
                    }
                }

                #endregion

                #region Refresh the layout of the controls that have corrupted scrollbar status after being unlocked

                foreach (var control in refreshLayoutControlList)
                {
                    try
                    {
                        // Update layout to recalculate scrollbar size and offset
                        control.PerformLayout();

                        // Repaint the control because the scrollbars may shift the contents
                        control.Invalidate();
                    }
                    catch (Exception)
                    {
                        if (!ignoreExceptions)
                            throw;
                    }
                }

                #endregion

                if (!locked)
                {
                    #region Disable TabStop for the KeepFormFocus text box

                    try
                    {
                        if (KeepFormFocus?.TabStop == true)
                        {
                            KeepFormFocus.TabStop = false;
                        }
                    }
                    catch (Exception)
                    {
                        if (!ignoreExceptions)
                            throw;
                    }

                    #endregion

                    #region Set default mouse cursor

                    try
                    {
                        if (Form.Cursor != Cursors.Default)
                        {
                            Form.Cursor = Cursors.Default;
                        }
                    }
                    catch (Exception)
                    {
                        if (!ignoreExceptions)
                            throw;
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                if (!ignoreExceptions)
                {
                    // Try to revert the changes in the controls on failure
                    SetLockedStatusControls(locked: !locked, ignoreExceptions: true);

                    throw;
                }
            }
        }

        #endregion

        #region Increment the form lock count and lock the controls if necessary

        /// <summary>
        /// Increment the form lock count and lock the controls if necessary
        /// </summary>
        private void LockControls()
        {
            if ((Interlocked.Increment(ref _lockCount) <= 0) ||
                (!InterlockedEx.TryCompareExchange(ref lockStatus, newValue: LockStatus_Changing, oldValue: LockStatus_Unlocked)))
                return;

            Control activeControlAfterUnlock;

            try
            {
                activeControlAfterUnlock = Form.ActiveControl;
                Thread.MemoryBarrier();

                if ((Form.IsHandleCreated) && (!Form.IsDisposed))
                {
                    SetLockedStatusControls(locked: true);
                }
                else
                {
                    // Cannot lock the controls because the form is not loaded yet, or it was already unloaded (closed)
                    lockStatus = LockStatus_Unlocked;
                    return;
                }
            }
            catch (ObjectDisposedException)
            {
                // Cannot lock the controls because the form is not loaded yet, or it was already unloaded (closed)
                lockStatus = LockStatus_Unlocked;
                return;
            }

            Thread.MemoryBarrier();
            lockStatus = LockStatus_Locked;
            ActiveControlAfterUnlock = activeControlAfterUnlock;
        }

        #endregion

        #region Decrement the form lock count and lock the controls if necessary

        /// <summary>
        /// Decrement the form lock count and lock the controls if necessary
        /// </summary>
        private void UnlockControls()
        {
            if ((Interlocked.Decrement(location: ref _lockCount) > 0) ||
                (!InterlockedEx.TryCompareExchange(ref lockStatus, newValue: LockStatus_Changing, oldValue: LockStatus_Locked)))
                return;

            Control activeControlAfterUnlock;

            try
            {
                activeControlAfterUnlock = ActiveControlAfterUnlock;
                Thread.MemoryBarrier();

                if ((Form.IsHandleCreated) && (!Form.IsDisposed))
                {
                    SetLockedStatusControls(locked: false);
                }
                else
                {
                    // Cannot unlock the controls because the form is not loaded yet, or it was already unloaded (closed)
                    lockStatus = LockStatus_Locked;
                    return;
                }
            }
            catch (ObjectDisposedException)
            {
                // Cannot unlock the controls because the form is not loaded yet, or it was already unloaded (closed)
                lockStatus = LockStatus_Locked;
                return;
            }

            Thread.MemoryBarrier();
            lockStatus = LockStatus_Unlocked;

            try
            {
                // Set the active control only if the this is the active form (or the active MDI child whose MDI parent is the active form), to prevent bringing a background form to the foreground

                if ((!(activeControlAfterUnlock is null)) &&
                    (Form.IsHandleCreated) &&
                    (!Form.IsDisposed) &&
                    ((Form.ActiveForm == Form) || ((Form.ActiveForm == Form.MdiParent) && (Form.MdiParent?.ActiveMdiChild == Form))))
                {
                    Form.ActiveControl = activeControlAfterUnlock;
                }
            }
            catch (ObjectDisposedException)
            {
                // Nothing to do, failed to set the active control because the form is not loaded yet, or it was already unloaded (closed)
            }
        }

        #endregion

        #region Locks the form and returns a IDisposable object that unlocks the form when disposed

        /// <summary>
        /// Locks the form and returns a IDisposable object that unlocks the form when disposed.
        /// This method must be called from the thread UI.
        /// </summary>
        /// <returns>An IDisposable object that unlocks the form when disposed</returns>
        public IDisposable RunWithFormLocked()
        {
            if (Form is null)
                return new WinFormsLockerDiposable(null);

            LockControls();
            return new WinFormsLockerDiposable(this);
        }

        #endregion

        #region Disposable class to lock/unlock the form controls

        /// <summary>
        /// IDisposable object that unlocks the form when disposed
        /// </summary>
        private class WinFormsLockerDiposable : IDisposable
        {
            #region Properties

            /// <summary>
            /// FormLocker assigned to this instance
            /// </summary>
            private WinFormsLocker FormLocker = null;

            #endregion

            #region Constructor

            /// <summary>
            /// Creates a new instance of WinFormsLockerDiposable for a FormLocker object
            /// </summary>
            /// <param name="formLocker">FormLocker assigned to this instance</param>
            internal WinFormsLockerDiposable(WinFormsLocker formLocker)
            {
                FormLocker = formLocker;
            }

            #endregion

            #region Invoke the method UnlockControls

            /// <summary>
            /// Invoke the method UnlockControls of the assigned WinFormsLocker (if any),
            /// and remove the assignment of the WinFormsLocker to prevent invoking it again
            /// </summary>
            private void InvokeUnlockControls()
            {
                // Get current FormLocker reference and change it null as an atomic operation,
                // then invoke FormLocker.UnlockControls() if it was not null before
                Interlocked.Exchange(ref FormLocker, null)?.UnlockControls();
            }

            #endregion

            #region IDisposable Support

            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // Nothing do to
                    }

                    InvokeUnlockControls();

                    disposedValue = true;
                }
            }

            void IDisposable.Dispose()
            {
                Dispose(true);
            }

            #endregion
        }

        #endregion
    }
}
