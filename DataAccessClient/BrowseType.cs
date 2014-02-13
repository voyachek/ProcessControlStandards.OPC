namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Namespace browse mode.
    /// </summary>
	public enum BrowseType
	{
        /// <summary>
        /// Reads folders.
        /// </summary>
		Branch = 1,

        /// <summary>
        /// Reads items.
        /// </summary>
		Leaf = 2,

        /// <summary>
        /// Reads all items in flat mode.
        /// </summary>
		Flat = 3,
	}
}
