#region using

using System.Collections.Generic;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Arguments for data writing events.
    /// </summary>
	public class WriteCompleteEventArgs : CompleteEventArgs
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="groupId">OPC DA group ID.</param>
        /// <param name="transactionId">Transaction ID of write operation.</param>
        /// <param name="error">Master error code.</param>
        /// <param name="results">Result of each item writing .</param>
		public WriteCompleteEventArgs(int groupId, int transactionId, int error, KeyValuePair<int, int>[] results): 
			base(groupId, transactionId)
		{
			Error = error;
			Results = results;
		}

        /// <summary>
        /// Master error code.
        /// </summary>
		public int Error { get; private set; }

        /// <summary>
        /// Result of each item writing .
        /// </summary>
		public KeyValuePair<int, int>[] Results { get; private set; }
	}
}
