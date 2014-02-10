#region using

using System;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class ServerProperties
	{
		public DateTime? StartTime { get; set; }
		
		public DateTime? CurrentTime { get; set; }

		public DateTime? LastUpdateTime { get; set; }

		public ServerState ServerState { get; set; }

		public int GroupCount { get; set; }

		public int Bandwidth { get; set; }

		public int MajorVersion { get; set; }

		public int MinorVersion { get; set; }

		public int BuildNumber { get; set; }

		public string VendorInfo { get; set; }
	}
}
