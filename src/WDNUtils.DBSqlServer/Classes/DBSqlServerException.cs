using System;
using System.Data.SqlClient;
using WDNUtils.DBSqlServer.Localization;

namespace WDNUtils.DBSqlServer
{
    /// <summary>
    /// SQL Server exception handling
    /// </summary>
    public static class DBSqlServerException
    {
        /// <summary>
        /// Convert a SqlException instance into a ApplicationException with an user friendly exception if possible
        /// </summary>
        /// <param name="ex">Exception to be converted</param>
        /// <param name="isConnecting">Indicates if the exception occurred when the database connection was being opened</param>
        public static void ConvertSqlServerException(this Exception ex, bool isConnecting)
        {
            if (!(ex is SqlException sqlException))
                return;

            switch (sqlException.Number)
            {
                case -2: // Timeout expired
                case 4014: // Fatal error reading network input stream
                case 7836: // Fatal error reading network input stream
                case 7885: // Network error
                case 9647: // Malformed message from network
                    throw new ApplicationException(string.Format(DBSqlServerLocalizedText.DBSqlServerException_NetworkError, Environment.NewLine, string.Format(@"Error {0} - {1}", sqlException.Number, sqlException.Message)), sqlException);

                case 2: // Error establishing connection - Could not open a connection to SQL Server
                case -1: // Error establishing connection - Server doesn't support requested protocol
                case 53: // Error establishing connection - Could not open a connection to SQL Server
                case 1418: // The server cannot be reached
                case 5828: // User connections limit exceeded
                case 8645: // Pool request timeout
                case 9754: // Error establishing connection - General failure
                case 10060: // Error establishing connection - No response
                case 10061: // Error establishing connection - Target actively refused
                case 11001: // Error establishing connection - Unknown host
                case 17187: // Error establishing connection - Not accepting new connections
                case 17188: // Error establishing connection - Shutting down
                case 17189: // Error establishing connection - Cannot spawn process thread
                case 17191: // Error establishing connection - Session terminated
                case 17197: // Error establishing connection - Login timeout due to heavy load
                case 17198: // Error establishing connection - Endpoint not found
                case 17803: // Error establishing connection - Memory allocation failure
                case 17809: // User connections limit exceeded
                case 17813: // Error establishing connection - Service stopped
                case 17829: // Error establishing connection - Network error
                case 17830: // Error establishing connection - Network timeout
                case 17832: // Error establishing connection - Invalid login packed
                case 17836: // Error establishing connection - Invalid packed length
                case 17889: // User connections limit exceeded
                case 18055: // Error establishing connection - Connection reset failed
                case 18312: // Error establishing connection - Service paused
                case 18401: // Error establishing connection - Server in script upgrade mode
                case 18451: // Error establishing connection - Only administrators can connect at this time
                case 18461: // Error establishing connection - Server in single user mode
                case 21670: // Error establishing connection - General failure
                case 26010: // Error establishing connection - Invalid certificate
                case 26014: // Error establishing connection - User certificate error
                case 26015: // Error establishing connection - User certificate error
                case 26058: // Error establishing connection - No TCP listener ports configured
                    throw new ApplicationException(string.Format(DBSqlServerLocalizedText.DBSqlServerException_DatabaseDown, Environment.NewLine, string.Format(@"Error {0} - {1}", sqlException.Number, sqlException.Message)), sqlException);

                default:
                    // Error codes for corrupted messages
                    if (((sqlException.Number >= 11238) && (sqlException.Number <= 11281)) ||
                        ((sqlException.Number >= 28040) && (sqlException.Number <= 28045)) ||
                        (sqlException.Number >= 28059) ||
                        ((sqlException.Number >= 28061) && (sqlException.Number <= 28068)) ||
                        // Error codes for handshake failure
                        ((sqlException.Number >= 28026) && (sqlException.Number <= 28039)) ||
                        (sqlException.Number >= 28070) ||
                        ((sqlException.Number >= 28079) && (sqlException.Number <= 28082)))
                        throw new ApplicationException(string.Format(DBSqlServerLocalizedText.DBSqlServerException_DatabaseDown, Environment.NewLine, string.Format(@"Error {0} - {1}", sqlException.Number, sqlException.Message)), sqlException);

                    break;
            }
        }
    }
}
