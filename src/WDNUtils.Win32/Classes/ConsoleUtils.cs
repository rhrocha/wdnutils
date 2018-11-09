using log4net;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using WDNUtils.Common;
using WDNUtils.Win32.Localization;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Utilities for Windows prompt command console
    /// </summary>
    public static class ConsoleUtils
    {
        #region Logger

        private static CachedProperty<ILog> _log = new CachedProperty<ILog>(() => LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType));
        private static ILog Log => _log.Value;

        #endregion

        #region Bring console window to front

        /// <summary>
        /// Bring console window to front
        /// </summary>
        /// <param name="ignoreException">If true, any exception will be logged instead of being rethrown (default is false)</param>
        public static void BringToFront(bool ignoreException = false)
        {
            try
            {
                var consoleWindowHandle = NativeMethods.GetConsoleWindow();

                if (consoleWindowHandle == IntPtr.Zero)
                    throw new ApplicationException(Win32LocalizedText.ConsoleUtils_BringToFront_GetConsoleWindow);

                if (!NativeMethods.SetForegroundWindow(consoleWindowHandle))
                    throw new ApplicationException(Win32LocalizedText.ConsoleUtils_BringToFront_SetForegroundWindow);
            }
            catch (Exception ex)
            {
                if (!ignoreException)
                    throw;

                Log.Error(Win32LocalizedText.ConsoleUtils_BringToFront_LogError, ex);
            }
        }

        #endregion

        #region Remove close button and menu from console window

        /// <summary>
        /// Remove close button and menu from console window
        /// </summary>
        /// <param name="ignoreException">If true, any exception will be logged instead of being rethrown (default is false)</param>
        public static void DisableClosing(bool ignoreException = false)
        {
            try
            {
                var consoleWindowHandle = NativeMethods.GetConsoleWindow();

                if (consoleWindowHandle == IntPtr.Zero)
                    throw new ApplicationException(Win32LocalizedText.ConsoleUtils_DisableClosing_GetConsoleWindow);

                var systemMenu = NativeMethods.GetSystemMenu(consoleWindowHandle, false);

                if (systemMenu == IntPtr.Zero)
                    throw new ApplicationException(Win32LocalizedText.ConsoleUtils_DisableClosing_GetSystemMenu);

                if (!NativeMethods.DeleteMenu(systemMenu, (uint)NativeMethods.DeleteMenuPositions.SC_CLOSE, (uint)NativeMethods.DeleteMenuFlags.BYCOMMAND))
                    throw new ApplicationException(Win32LocalizedText.ConsoleUtils_DisableClosing_DeleteMenu);
            }
            catch (Exception ex)
            {
                if (!ignoreException)
                    throw;

                Log.Error(Win32LocalizedText.ConsoleUtils_DisableClosing_LogError, ex);
            }
        }

        #endregion

        #region WinAPI references

        /// <summary>
        /// WinAPI references
        /// </summary>
        internal static class NativeMethods
        {
            public enum DeleteMenuPositions : uint
            {
                SC_CLOSE = 0xF060,
                SC_CONTEXTHELP = 0xF180,
                SC_DEFAULT = 0xF160,
                SC_HOTKEY = 0xF150,
                SC_HSCROLL = 0xF080,
                SCF_ISSECURE = 0x00000001,
                SC_KEYMENU = 0xF100,
                SC_MAXIMIZE = 0xF030,
                SC_MINIMIZE = 0xF020,
                SC_MONITORPOWER = 0xF170,
                SC_MOUSEMENU = 0xF090,
                SC_MOVE = 0xF010,
                SC_NEXTWINDOW = 0xF040,
                SC_PREVWINDOW = 0xF050,
                SC_RESTORE = 0xF120,
                SC_SCREENSAVE = 0xF140,
                SC_SIZE = 0xF000,
                SC_TASKLIST = 0xF130,
                SC_VSCROLL = 0xF070
            }

            public enum DeleteMenuFlags : uint
            {
                BYCOMMAND = 0x0000,
                BYPOSITION = 0x0400
            }

            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern bool DeleteMenu(IntPtr hMenu, uint nPosition, uint wFlags);

            [DllImport("user32.dll")]
            public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        }

        #endregion
    }
}
