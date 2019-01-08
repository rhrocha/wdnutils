using System;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Thread-safe form locker
    /// </summary>
    public interface IFormLocker
    {
        /// <summary>
        /// Indicates if the form is locked
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// The control that should be active when the form is unlocked
        /// </summary>
        object ActiveControl { get; set; }

        /// <summary>
        /// Scan the controls of the form that should be locked/unlocked by this class
        /// </summary>
        /// <returns>A reference to this instance</returns>
        IFormLocker ScanControls(bool resetCustom = false);

        /// <summary>
        /// Sets a custom locking contidion for the controls
        /// </summary>
        /// <param name="getLocked">Custom locking condition (if null, the default locking will be used)</param>
        /// <param name="controls">List of controls</param>
        /// <returns>A reference to this instance</returns>
        IFormLocker SetControlLocking(Func<bool, bool> getLocked, params object[] controls);

        /// <summary>
        /// Removes a control from the locking list (also from the custom locking list)
        /// </summary>
        /// <param name="controls">List of controls</param>
        /// <returns>A reference to this instance</returns>
        IFormLocker RemoveControl(params object[] controls);

        /// <summary>
        /// Locks the form and returns a IDisposable object that unlocks the form when disposed.
        /// This method must be called from the thread UI.
        /// </summary>
        /// <returns>An IDisposable object that unlocks the form when disposed</returns>
        IDisposable RunWithFormLocked();
    }
}
