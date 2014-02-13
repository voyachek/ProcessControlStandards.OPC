#region using

using System;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Arguments for completion events.
    /// </summary>
	public class CompleteEventArgs : EventArgs
	{
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="groupId">ID of OPC DA group.</param>
        /// <param name="transactionId">ID of completed transaction.</param>
		public CompleteEventArgs(int groupId, int transactionId)
		{
			GroupId = groupId;
			TransactionId = transactionId;
		}

        /// <summary>
        /// ID of OPC group.
        /// </summary>
		public int GroupId { get; private set; }

        /// <summary>
        /// ID of completed transaction.
        /// </summary>
		public int TransactionId { get; private set; }
	}
}
