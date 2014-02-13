
namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Arguments for data change events.
    /// </summary>
	public class DataChangeEventArgs : CompleteEventArgs
	{
        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="groupId">ID of OPC DA group.</param>
        /// <param name="transactionId">ID of completed transaction.</param>
        /// <param name="quality">Master quality.</param>
        /// <param name="error">Master error code.</param>
        /// <param name="values">OPC DA item values.</param>
		public DataChangeEventArgs(int groupId, int transactionId, int quality, int error, ItemValue[] values) : 
			base(groupId, transactionId)
		{
			Quality = quality;
			Error = error;
			Values = values;
		}

        /// <summary>
        /// Master quality.
        /// </summary>
		public int Quality { get; private set; }

        /// <summary>
        /// Master error code.
        /// </summary>
		public int Error { get; private set; }

        /// <summary>
        /// OPC DA item values.
        /// </summary>
		public ItemValue[] Values { get; private set; }
	}
}
