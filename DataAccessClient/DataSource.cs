namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Data read mode.
    /// </summary>
	public enum DataSource
	{
        /// <summary>
        /// Reads from cache.
        /// </summary>
		Cache = 1,

        /// <summary>
        /// Reads from device directly.
        /// </summary>
		Device = 2,
	}
}
