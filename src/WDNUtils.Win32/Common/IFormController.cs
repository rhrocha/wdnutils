using System;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Form controller, to lock the controls during async operations, and prevent discarding changes without alerting the user
    /// </summary>
    /// <typeparam name="T">Control base type</typeparam>
    public interface IFormController<T> where T : class
    {
        /// <summary>
        /// Indicates if the form is locked
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Indicates if the form is loaded
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Indicates if the form can change the focused control
        /// </summary>
        bool IsFocusable { get; }

        /// <summary>
        /// Indicates if the data loaded in the form is invalid and cannot be saved, so there is no need to show the "Discard changes?" dialog.
        /// </summary>
        bool IsDataInvalid { get; }

        /// <summary>
        /// Indicates if the data in the form was changed by the user, so the "Discard changes?" dialog must be displayed before overwriting the data.
        /// </summary>
        bool IsDataChanged { get; }

        /// <summary>
        /// Custom "Discard changes?" dialog box for this instance
        /// </summary>
        Func<T, bool> DiscardChangesDialog { get; set; }

        /// <summary>
        /// Indicates if the "discard changes" dialog box should be suppressed
        /// </summary>
        Func<bool> BypassCheckDataChanged { get; set; }

        /// <summary>
        /// The control that should be active when the form is unlocked
        /// </summary>
        T ActiveControl { get; set; }

        /// <summary>
        /// Scan the controls of the form that should be locked/unlocked by this class
        /// </summary>
        /// <returns>A reference to this instance</returns>
        IFormController<T> ScanControls(bool resetCustom = false);

        /// <summary>
        /// Sets a custom locking contidion for the controls
        /// </summary>
        /// <param name="getLocked">Custom locking condition (if null, the default locking will be used)</param>
        /// <param name="controls">List of controls</param>
        /// <returns>A reference to this instance</returns>
        IFormController<T> SetControlLocking(Func<bool, bool> getLocked, params T[] controls);

        /// <summary>
        /// Removes a control from the locking list (also from the custom locking list)
        /// </summary>
        /// <param name="controls">List of controls</param>
        /// <returns>A reference to this instance</returns>
        IFormController<T> RemoveControlLocking(params T[] controls);

        /// <summary>
        /// Locks the form and returns a IDisposable object that unlocks the form when disposed.
        /// This method must be called from the thread UI.
        /// </summary>
        /// <returns>An IDisposable object that unlocks the form when disposed</returns>
        IDisposable RunWithFormLocked();

        /// <summary>
        /// Add controls to be monitored
        /// </summary>
        /// <param name="controls">Controls to be monitored</param>
        /// <returns>A reference to this instance</returns>
        IFormController<T> AddDataControl(params T[] controls);

        /// <summary>
        /// Check form data status before discarding the changes, and show the "Discard changes?" dialog if necessary
        /// </summary>
        /// <returns>True if the form data can be discarded, or false if the form data must be kept</returns>
        bool CheckDataChanged();

        /// <summary>
        /// Clear form data changed and invalid status
        /// </summary>
        void ClearDataStatus();

        /// <summary>
        /// Set form data as changed
        /// </summary>
        /// <param name="ignoreFormLocker">Indicates if <see cref="IsDataChanged"/> should be set as 'true' even if the form is locked by the FormLocker</param>
        void SetDataChanged(bool ignoreFormLocker = false);

        /// <summary>
        /// Set data as invalid, so there is no need to show the "Discard changes?" dialog
        /// </summary>
        void SetDataInvalid();
    }
}
