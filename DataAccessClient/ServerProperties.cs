#region using

using System;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA Server properties.
    /// </summary>
	public class ServerProperties
	{
        /// <summary>
        /// Time of start.
        /// </summary>
		public DateTime? StartTime { get; set; }
		
        /// <summary>
        /// Current time in OPC DA Server.
        /// </summary>
		public DateTime? CurrentTime { get; set; }

        /// <summary>
        /// Last time of update item values.
        /// </summary>
		public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// Current OPC DA Server state.
        /// </summary>
		public ServerState ServerState { get; set; }

        /// <summary>
        /// Number of OPC DA groups registered on server.
        /// </summary>
		public int GroupCount { get; set; }

        /// <summary>
        /// Current bandwidth.
        /// </summary>
		public int Bandwidth { get; set; }

        /// <summary>
        /// OPC DA Server major version number.
        /// </summary>
		public int MajorVersion { get; set; }

        /// <summary>
        /// OPC DA Server minor version number.
        /// </summary>
        public int MinorVersion { get; set; }

        /// <summary>
        /// OPC DA Server build version number.
        /// </summary>
		public int BuildNumber { get; set; }

        /// <summary>
        /// Description of OPC DA Server.
        /// </summary>
		public string VendorInfo { get; set; }
	}
}
