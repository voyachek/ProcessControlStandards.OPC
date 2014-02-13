using System;

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA group item value.
    /// </summary>
	public struct ItemValue
	{
        /// <summary>
        /// Client ID of item.
        /// </summary>
		public int ClientId;

        /// <summary>
        /// Change date of item value.
        /// </summary>
		public DateTime Timestamp;

        /// <summary>
        /// Item data quality.
        /// </summary>
		public int Quality;
		
        /// <summary>
        /// Item value.
        /// </summary>
		public object Value;

        /// <summary>
        /// Error code of reading item value.
        /// </summary>
		public int Error;
	}
}
