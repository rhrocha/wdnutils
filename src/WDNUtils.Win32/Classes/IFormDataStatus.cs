namespace WDNUtils.Win32
{
    /// <summary>
    /// Form input data monitor, to prevent discarding changes without alerting the user
    /// </summary>
    public interface IFormDataStatus
    {
        /// <summary>
        /// Indicates if the data loaded in the form is invalid and cannot be saved, so there is no need to show the "Discard changes?" dialog.
        /// </summary>
        bool IsDataInvalid { get; }

        /// <summary>
        /// Indicates if the data in the form was changed by the user, so the "Discard changes?" dialog must be displayed before overwriting the data.
        /// </summary>
        bool IsDataChanged { get; }

        /// <summary>
        /// Add controls to be monitored
        /// </summary>
        /// <param name="controls">Controls to be monitored</param>
        /// <returns>A reference to this instance</returns>
        IFormDataStatus AddControl(params object[] controls);

        /// <summary>
        /// Check form data status before discarding the changes, and show the "Discard changes?" dialog if necessary
        /// </summary>
        /// <returns>True if the form data can be discarded, or false if the form data must be kept</returns>
        bool CheckDataChanged();

        /// <summary>
        /// Clear form data changed and invalid status
        /// </summary>
        void ClearStatus();

        /// <summary>
        /// Set form data as changed
        /// </summary>
        /// <param name="ignoreFormLocker">Indicates if this call should be ignored if the form is locked by the FormLocker</param>
        void SetDataChanged(bool ignoreFormLocker);

        /// <summary>
        /// Set data as invalid, so there is no need to show the "Discard changes?" dialog
        /// </summary>
        void SetDataInvalid();
    }
}
