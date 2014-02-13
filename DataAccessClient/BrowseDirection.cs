namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// The direction to browse OPC Server namespace.
    /// </summary>
	public enum BrowseDirection
	{
        /// <summary>
        /// Default direction.
        /// </summary>
        None = 0,

        /// <summary>
        /// Level down.
        /// </summary>
        Down = 2,

        /// <summary>
        /// Move to specified name.
        /// </summary>
		To = 3,

        /// <summary>
        /// Level up.
        /// </summary>
        Up = 1,
	}
}
