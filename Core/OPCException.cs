#region using

using System;
using System.Runtime.Serialization;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	[Serializable]
	public class OPCException : Exception
	{
		public OPCException()
		{
		} 

		public OPCException(string message) : 
			base(message)
		{
		}

		public OPCException(string message, Exception innerException) : 
			base(message, innerException)
		{
		}

		private OPCException(SerializationInfo info, StreamingContext context) : 
			base(info, context)
		{ 
		}
	}
}
