#region using

using System;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class CompleteEventArgs : EventArgs
	{
		public CompleteEventArgs(int groupId, int transactionId)
		{
			GroupId = groupId;
			TransactionId = transactionId;
		}

		public int GroupId { get; private set; }

		public int TransactionId { get; private set; }
	}
}
