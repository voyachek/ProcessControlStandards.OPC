using System.Runtime.InteropServices;

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Result of add/validate OPC DA group item.
    /// </summary>
	public struct ItemResult
	{
        /// <summary>
        /// Server ID of item.
        /// </summary>
		public int ServerId;

        /// <summary>
        /// Item value data type.
        /// </summary>
		public VarEnum CanonicalDataType;

        /// <summary>
        /// Item value data sub type (if type is array).
        /// </summary>
        public VarEnum CanonicalDataSubType;

        /// <summary>
        /// Item security.
        /// </summary>
		public int AccessRights;
		
        /// <summary>
        /// Error code of adding/validating OPC DA group item.
        /// </summary>
		public int Error;

        /// <summary>
        /// Item BLOB.
        /// </summary>
        public byte[] Blob { get; set; }
	}
}
