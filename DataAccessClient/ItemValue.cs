using System;

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public struct ItemValue
	{
		public int ClientId;

		public DateTime Timestamp;

		public int Quality;
		
		public object Value;

		public int Error;
	}
}
