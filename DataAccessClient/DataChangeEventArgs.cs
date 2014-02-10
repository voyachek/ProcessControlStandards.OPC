
namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class DataChangeEventArgs : CompleteEventArgs
	{
		public DataChangeEventArgs(int groupId, int transactionId, int quality, int error, ItemValue[] values) : 
			base(groupId, transactionId)
		{
			Quality = quality;
			Error = error;
			Values = values;
		}

		public int Quality { get; private set; }

		public int Error { get; private set; }

		public ItemValue[] Values { get; private set; }
	}
}
