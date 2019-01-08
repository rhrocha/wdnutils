using System;
using System.Windows.Forms;
using WDNUtils.Win32.Localization;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Form input data monitor, to prevent discarding changes without alerting the user, for WinForms
    /// </summary>
    public sealed class WinFormsDataStatus : IFormDataStatus
    {
        #region Properties

        /// <summary>
        /// Form locker for the monitored form
        /// </summary>
        private WinFormsLocker FormLocker { get; set; }

        /// <summary>
        /// Indicates if the data loaded in the form is invalid and cannot be saved, so there is no need to show the "Discard changes?" dialog
        /// </summary>
        public bool IsDataInvalid { get; private set; } = false;

        /// <summary>
        /// Indicates if the data in the form was changed by the user, so the "Discard changes?" dialog must be displayed before overwriting the data
        /// </summary>
        public bool IsDataChanged { get; private set; } = false;

        /// <summary>
        /// Custom "Discard changes?" dialog box for this instance
        /// </summary>
        public Func<Form, bool> DiscardChangesDialog { get; set; }

        /// <summary>
        /// Default "Discard changes?" dialog box for all instances
        /// </summary>
        public Func<Form, bool> DefaultDiscardChangesDialog { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of WinFormsDataStatus
        /// </summary>
        /// <param name="formLocker">Form locker for the monitored form</param>
        public WinFormsDataStatus(WinFormsLocker formLocker)
        {
            FormLocker = formLocker ?? throw new ArgumentNullException(nameof(formLocker));

            if (FormLocker.Form is null)
                throw new InvalidOperationException();

            FormLocker.Form.FormClosing += Form_FormClosing;
            FormLocker.Form.HandleDestroyed += Form_HandleDestroyed;
        }

        #endregion

        #region Form events

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && (!CheckDataChanged()))
            {
                e.Cancel = true;
            }
        }

        private void Form_HandleDestroyed(object sender, EventArgs e)
        {
            if (FormLocker?.Form is Form form)
            {
                form.HandleDestroyed -= Form_HandleDestroyed;
                form.FormClosing -= Form_FormClosing;
            }

            FormLocker = null;
        }

        #endregion

        #region Add input controls to be monitored

        /// <summary>
        /// Add input controls to be monitored
        /// </summary>
        /// <param name="controls">Controls to be monitored</param>
        /// <returns>A reference to this instance</returns>
        public IFormDataStatus AddControl(params object[] controls)
        {
            foreach (var control in controls)
            {
                if (control is null)
                {
                    throw new ArgumentNullException(nameof(controls));
                }
                else if (control is TextBoxBase textBoxBase)
                {
                    textBoxBase.TextChanged += SetDataChanged;
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.CheckedChanged += SetDataChanged;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndexChanged += SetDataChanged;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(control.GetType().Name);
                }
            }

            return this;
        }

        #endregion

        #region Update form text

        private void UpdateFormText()
        {
            try
            {
                var text = FormLocker.Form.Text ?? string.Empty;

                while (text.StartsWith(@"*", StringComparison.Ordinal))
                {
                    text = text.Substring(1);
                }

                FormLocker.Form.Text = ((!IsDataInvalid) && (IsDataChanged)) ? string.Concat(@"*", text) : text;
            }
            catch (ObjectDisposedException)
            {
                // Nothing to do
            }
        }

        #endregion

        #region Check data changed

        /// <summary>
        /// Check form data status before discarding the changes, and show the "Discard changes?" dialog if necessary
        /// </summary>
        /// <returns>True if the form data can be discarded, or false if the form data must be kept</returns>
        public bool CheckDataChanged()
        {
            try
            {
                if ((IsDataInvalid) || (!IsDataChanged))
                    return true;

                if ((DiscardChangesDialog ?? DefaultDiscardChangesDialog ?? DefaultDiscardChangesDialogInternal).Invoke(FormLocker.Form) == false)
                    return false;

                IsDataChanged = false;
                UpdateFormText();

                return true;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
        }

        #endregion

        #region Default "Discard changes?" dialog

        private static bool DefaultDiscardChangesDialogInternal(Form form)
        {
            try
            {
                return MessageBox.Show(
                    owner: form,
                    text: Win32LocalizedText.IFormDataStatus_Dialog_Body,
                    caption: Win32LocalizedText.IFormDataStatus_Dialog_Title,
                    buttons: MessageBoxButtons.YesNo,
                    icon: MessageBoxIcon.None,
                    defaultButton: MessageBoxDefaultButton.Button2) == DialogResult.Yes;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
        }

        #endregion

        #region Clear form data changed and invalid status

        /// <summary>
        /// Clear form data changed and invalid status
        /// </summary>
        public void ClearStatus()
        {
            try
            {
                IsDataInvalid = false;
                IsDataChanged = false;
                UpdateFormText();
            }
            catch (ObjectDisposedException)
            {
                // Nothing to do
            }
        }

        #endregion

        #region Set form data as changed

        /// <summary>
        /// Set form data as changed
        /// </summary>
        /// <param name="ignoreFormLocker">Indicates if this call should be ignored if the form is locked by the FormLocker</param>
        public void SetDataChanged(bool ignoreFormLocker)
        {
            try
            {
                if ((!IsDataChanged) && ((ignoreFormLocker) || (!FormLocker.IsLocked)))
                {
                    IsDataChanged = true;
                    UpdateFormText();
                }
            }
            catch (ObjectDisposedException)
            {
                // Nothing to do
            }
        }

        private void SetDataChanged(object sender, EventArgs e)
        {
            SetDataChanged(ignoreFormLocker: false);
        }

        #endregion

        #region Set data as invalid

        /// <summary>
        /// Set data as invalid, so there is no need to show the "Discard changes?" dialog
        /// </summary>
        public void SetDataInvalid()
        {
            try
            {
                IsDataInvalid = true;
                UpdateFormText();
            }
            catch (ObjectDisposedException)
            {
                // Nothing to do
            }
        }

        #endregion
    }
}
