using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace WDNUtils.Win32
{
    /// <summary>
    /// Creates <see cref="System.Threading.Mutex"/> instances, giving full control
    /// security rights to system (global) named mutexes so all users can use it.
    /// </summary>
    public static class MutexFactory
    {
        #region System (global) named mutex

        /// <summary>
        /// Creates a new or open an existing system (global) named mutex (shared by all sessions
        /// in a terminal server), giving full control security rights so all users can use it.
        /// </summary>
        /// <param name="name">Mutex name</param>
        /// <param name="createdNew">Indicates if a new mutex was created, or if it already exists</param>
        /// <param name="ignoreSecurityError">Indicates that the UnauthorizedAccessException
        /// should be ignored if the current user does not have permission to open an existing mutex.</param>
        /// <returns>The new created mutex, or null if <paramref name="ignoreSecurityError"/> is true
        /// and the mutex already exists but could not be opened due to lack of permissions</returns>
        public static Mutex CreateGlobal(string name, out bool createdNew, bool ignoreSecurityError = false)
        {
            // Setup a security context with full control access for 'Everyone' user

            var mutexSecurity = new MutexSecurity();

            mutexSecurity.AddAccessRule(
                new MutexAccessRule(
                    identity: new SecurityIdentifier(sidType: WellKnownSidType.WorldSid, domainSid: null),
                    eventRights: MutexRights.FullControl,
                    type: AccessControlType.Allow));

            try
            {
                return new Mutex(
                    initiallyOwned: true,
                    name: $@"Global\{name}",
                    createdNew: out createdNew,
                    mutexSecurity: mutexSecurity);
            }
            catch (UnauthorizedAccessException)
            {
                // The system (global) named mutex already exists, but current process cannot access it

                if (!ignoreSecurityError)
                    throw;

                createdNew = false;
                return null;
            }
        }

        #endregion

        #region Local named mutex

        /// <summary>
        /// Creates a new or open an existing local named mutex (affects only current session in a terminal server)
        /// </summary>
        /// <param name="name">Mutex name</param>
        /// <param name="createdNew">Indicates if a new mutex was created, or if it already exists</param>
        /// <returns>The new created mutex</returns>
        public static Mutex CreateLocal(string name, out bool createdNew)
        {
            return new Mutex(
                initiallyOwned: true,
                name: $@"Local\{name}",
                createdNew: out createdNew);
        }

        #endregion
    }
}
