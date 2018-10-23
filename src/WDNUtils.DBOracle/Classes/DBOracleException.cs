using Oracle.ManagedDataAccess.Client;
using System;
using WDNUtils.DBOracle.Localization;

namespace WDNUtils.DBOracle
{
    /// <summary>
    /// Oracle exception handling
    /// </summary>
    public static class DBOracleException
    {
        /// <summary>
        /// Convert a OracleException instance into a ApplicationException with an user friendly exception if possible
        /// </summary>
        /// <param name="ex">Exception to be converted</param>
        public static void ConvertOracleException(this Exception ex)
        {
            if (!(ex is OracleException oracleException))
                return;

            switch (oracleException.Number)
            {
                case -1000: // TCP Connection timeout
                case -6403: // TCP Transport error
                    throw new ApplicationException(string.Format(DBOracleLocalizedText.DBOracleException_NetworkError, Environment.NewLine, oracleException.Message), oracleException);

                case 12150: // TNS: Unable to send data
                case 12151: // TNS: Received bad packet type from network layer
                case 12152: // TNS: Unable to send break message
                case 12153: // TNS: Not connected
                case 12155: // TNS: Received bad datatype in NSWMARKER packet
                case 12157: // TNS: Internal network communication error
                case 12161: // TNS: Internal error: partial data received
                case 12162: // TNS: Net service name is incorrectly specified
                case 12170: // TNS: Connect timeout occurred
                case 12203: // TNS: Unable to connect to destination
                case 12224: // TNS: No listener
                case 12225: // TNS: Destination host unreachable
                case 12230: // TNS: Severe Network error occurred in making this connection
                case 12231: // TNS: No connection possible to destination
                case 12525: // TNS: Listener has not received client's request in time allowed
                case 12537: // TNS: Connection closed
                case 12543: // TNS: Destination host unreachable
                case 12569: // TNS: Packet checksum failure
                case 12570: // TNS: Packet reader failure
                case 12571: // TNS: Packet writer failure
                case 12585: // TNS: Data truncation
                case 12592: // TNS: Bad packet
                case 12606: // TNS: Application timeout occurred
                case 12607: // TNS: Connect timeout occurred
                case 12608: // TNS: Send timeout occurred
                case 12609: // TNS: Receive timeout occurred
                case 12636: // Packet send failed
                case 12637: // Packet receive failed
                case 16509: // Request timed out
                case 16619: // Health check timed out
                case 16625: // Cannot reach the database
                case 16635: // Network connection failed during transmission
                case 16665: // Timeout waiting for the result from a database
                case 17629: // Cannot connect to the remote database server
                    throw new ApplicationException(string.Format(DBOracleLocalizedText.DBOracleException_NetworkError, Environment.NewLine, string.Format(@"ORA-{0:00000} - {1}", oracleException.Number, oracleException.Message)), oracleException);

                default:
                    if ((oracleException.Number >= 12000) && (oracleException.Number < 13000))
                        throw new ApplicationException(string.Format(DBOracleLocalizedText.DBOracleException_DatabaseDown, Environment.NewLine, string.Format(@"ORA-{0:00000} - {1}", oracleException.Number, oracleException.Message)), oracleException);
                    break;
            }
        }
    }
}
