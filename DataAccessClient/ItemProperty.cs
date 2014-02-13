#region using

using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA group item property description.
    /// </summary>
	public struct ItemProperty
	{
        /// <summary>
        /// Property ID.
        /// </summary>
		public int Id;

        /// <summary>
        /// Property description.
        /// </summary>
		public string Description;

        /// <summary>
        /// Property data type.
        /// </summary>
		public VarEnum Type;
	}
}
