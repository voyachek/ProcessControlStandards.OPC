#region using

using System.Collections.Generic;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class WriteCompleteEventArgs : CompleteEventArgs
	{
		public WriteCompleteEventArgs(int groupId, int transactionId, int error, KeyValuePair<int, int>[] results): 
			base(groupId, transactionId)
		{
			Error = error;
			Results = results;
		}

		public int Error { get; private set; }

		public KeyValuePair<int, int>[] Results { get; private set; }
	}
}
