
namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// OPC DA Server state.
    /// </summary>
	public enum ServerState
	{
        /// <summary>
        /// Server in running mode.
        /// </summary>
		Running = 1,

        /// <summary>
        /// Server in failed mode.
        /// </summary>
		Failed = 2,
        
        /// <summary>
        /// Server not configured.
        /// </summary>
		NoConfig = 3,

        /// <summary>
        /// Server suspended.
        /// </summary>
		Suspended = 4,

        /// <summary>
        /// Server in test mode.
        /// </summary>
		Test = 5
	}
}
