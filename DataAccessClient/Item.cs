using System.Runtime.InteropServices;

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA group item properties.
    /// </summary>
	public struct Item
	{
        /// <summary>
        /// Item name.
        /// </summary>
		public string ItemId;

        /// <summary>
        /// Item client ID.
        /// </summary>
		public int ClientId;
		
        /// <summary>
        /// Requested value data type.
        /// </summary>
        public VarEnum RequestedDataType;

        /// <summary>
        /// Item read mode.
        /// </summary>
		public bool Active;

        /// <summary>
        /// Item access path.
        /// </summary>
		public string AccessPath;
	}
}
