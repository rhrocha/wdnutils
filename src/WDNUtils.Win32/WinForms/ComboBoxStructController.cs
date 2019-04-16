using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Controller for a ComboBox with a struct values
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    public class ComboBoxStructController<T> where T : struct
    {
        #region Properties

        /// <summary>
        /// Combo box that is controlled by this class
        /// </summary>
        protected ComboBox ComboBox { get; private set; }

        /// <summary>
        /// List of command text elements
        /// </summary>
        private IEnumerable<Tuple<T?, string>> CommandTextList { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of ComboBoxStructController
        /// </summary>
        /// <param name="comboBox">Combo box that will be controlled by this class</param>
        /// <param name="commandText">List of command text elements</param>
        protected ComboBoxStructController(ComboBox comboBox, params string[] commandText)
        {
            ComboBox = comboBox;

            ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox.DropDownHeight = ComboBox.ItemHeight * 10;
            ComboBox.IntegralHeight = false;
            ComboBox.FormattingEnabled = true;

            ComboBox.DisplayMember = nameof(Tuple<T?, string>.Item2);
            ComboBox.ValueMember = nameof(Tuple<T?, string>.Item2);
            ComboBox.DataSource = new List<Tuple<T?, string>>();

            CommandTextList = (commandText?.Select(item => new Tuple<T?, string>(null, item))?.ToList())
                 ?? Enumerable.Empty<Tuple<T?, string>>();
        }

        #endregion

        #region Get selection

        /// <summary>
        /// Indicates if there is a selected item in the combo box
        /// </summary>
        /// <returns>True if there is a selected item in the combo box</returns>
        public bool HasSelection()
        {
            return (ComboBox.SelectedIndex >= 0);
        }

        /// <summary>
        /// Get current selected item (returns null if the selected item is a command text element, or if there is no selected item)
        /// </summary>
        /// <returns>Current selected item</returns>
        public T? GetSelectedItem()
        {
            return (ComboBox.SelectedItem as Tuple<T?, string>)?.Item1;
        }

        /// <summary>
        /// Get current command text (returns null if the selected item is a data element, or if there is no selected item)
        /// </summary>
        /// <returns>Current selected command text</returns>
        public string GetSelectedCommandText()
        {
            var item = ComboBox.SelectedItem as Tuple<T?, string>;

            return (item?.Item1 is null) ? item?.Item2 : null;
        }

        #endregion

        #region Set selection

        /// <summary>
        /// Clear selection
        /// </summary>
        public void ClearSelection()
        {
            ComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// Set selected data item
        /// </summary>
        /// <param name="predicate">Predicate to select the desired data item</param>
        /// <param name="clearSelectionIfNotFound">Indicates if the selection should be cleared if the desired data item is not found</param>
        /// <returns>True if the desired data item is selected, false if the data item was not found</returns>
        protected bool SetSelectedItem(Func<T, bool> predicate, bool clearSelectionIfNotFound = true)
        {
            for (int index = 0; index < ComboBox.Items.Count; index++)
            {
                var item = ComboBox.Items[index] as Tuple<T?, string>;

                if ((!(item?.Item1 is null)) && (predicate(item.Item1.Value)))
                {
                    ComboBox.SelectedIndex = index;
                    return true;
                }
            }

            if (clearSelectionIfNotFound)
            {
                ComboBox.SelectedIndex = -1;
            }

            return false;
        }

        /// <summary>
        /// Set selected command text
        /// </summary>
        /// <param name="predicate">Predicate to select the desired command text</param>
        /// <param name="clearSelectionIfNotFound">Indicates if the selection should be cleared if the desired command text is not found</param>
        /// <returns>True if the desired command text is selected, false if the command text was not found</returns>
        protected bool SetSelectedCommandText(Func<string, bool> predicate, bool clearSelectionIfNotFound = true)
        {
            for (int index = 0; index < ComboBox.Items.Count; index++)
            {
                var item = ComboBox.Items[index] as Tuple<T?, string>;

                if ((item?.Item1 is null) && (predicate(item?.Item2)))
                {
                    ComboBox.SelectedIndex = index;
                    return true;
                }
            }

            if (clearSelectionIfNotFound)
            {
                ComboBox.SelectedIndex = -1;
            }

            return false;
        }

        /// <summary>
        /// Set selected command text
        /// </summary>
        /// <param name="commandText">Command text to be select</param>
        /// <param name="clearSelectionIfNotFound">Indicates if the selection should be cleared if the command text is not found</param>
        /// <returns>True if the command text is selected, false if the command text was not found</returns>
        protected bool SetSelectedCommandText(string commandText, bool clearSelectionIfNotFound = true)
        {
            return SetSelectedCommandText(
                predicate: item => string.Equals(item, commandText, StringComparison.Ordinal),
                clearSelectionIfNotFound: clearSelectionIfNotFound);
        }

        /// <summary>
        /// Set the first data item element as current selected item, if there is no current selected item
        /// </summary>
        /// <returns>True if a data item was selected, false if there are no data items to be selected, or null if the selection was not changed</returns>
        public bool? SetSelectedItemFirstByDefault()
        {
            if (ComboBox.SelectedIndex < 0)
            {
                return SetSelectedItem(predicate: item => true, clearSelectionIfNotFound: false);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the first command text as current selected item, if there is no current selected item
        /// </summary>
        /// <returns>True if a command text was selected, false if there are no command texts to be selected, or null if the selection was not changed</returns>
        public bool? SetSelectedCommandTextFirstByDefault()
        {
            if (ComboBox.SelectedIndex < 0)
            {
                return SetSelectedCommandText(predicate: item => true, clearSelectionIfNotFound: false);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set a command text as current selected item, if there is no current selected item
        /// </summary>
        /// <param name="commandText">Command text to be select</param>
        /// <returns>True if a command text was selected, false if the command text was not found, or null if the selection was not changed</returns>
        public bool? SetSelectedCommandTextByDefault(string commandText)
        {
            if (ComboBox.SelectedIndex < 0)
            {
                return SetSelectedCommandText(commandText: commandText, clearSelectionIfNotFound: false);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Set data source

        /// <summary>
        /// Remove all data items from the combo box list (command text items are kept)
        /// </summary>
        public void Clear()
        {
            SetDataList(
                dataList: Enumerable.Empty<T>(),
                getText: item => item.ToString(),
                equalityComparer: null);
        }

        /// <summary>
        /// Updates the date items in the combo box list (command text items are kept)
        /// </summary>
        /// <param name="dataList">New list of data items</param>
        /// <param name="getText">Function to generate the text description for each data item, to be displayed in the combo box</param>
        /// <param name="equalityComparer">Comparer used to keep the current selected item (may be null)</param>
        /// <returns>True if the selection was kept (or if there was no selection), false if the previously selected element was not found</returns>
        public bool SetDataList(IEnumerable<T> dataList, Func<T, string> getText, Func<T, T, bool> equalityComparer)
        {
            var hasSelection = HasSelection();
            var selectedItem = (!hasSelection) ? null : GetSelectedItem();
            var selectedCommandText = (!hasSelection) ? null : GetSelectedCommandText();

            ComboBox.DataSource = CommandTextList.Concat(
                dataList.Select(item => new Tuple<T?, string>(item, getText(item)))).ToList();

            if (!hasSelection)
            {
                return true;
            }
            else if (!(selectedCommandText is null))
            {
                return SetSelectedCommandText(selectedCommandText);
            }
            else if ((!(selectedItem is null)) && (!(equalityComparer is null)))
            {
                return SetSelectedItem(item => equalityComparer(item, selectedItem.Value));
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Get the parent form

        /// <summary>
        /// Retrieves the form that the control is on
        /// </summary>
        /// <returns>The <see cref="Form"/> that the control is on</returns>
        public Form FindForm()
        {
            return ComboBox?.FindForm();
        }

        #endregion
    }
}
