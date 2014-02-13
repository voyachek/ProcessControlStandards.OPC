#region using

using System;
using System.Runtime.Serialization;

#endregion

namespace ProcessControlStandards.OPC.Core
{
    /// <summary>
    /// Exception for OPC Toolkit library.
    /// </summary>
	[Serializable]
	public class OPCException : Exception
	{
        /// <summary>
        /// Default constructor.
        /// </summary>
		public OPCException()
		{
		} 

        /// <summary>
        /// Constructor based on error description.
        /// </summary>
        /// <param name="message">Error description.</param>
		public OPCException(string message) : 
			base(message)
		{
		}

        /// <summary>
        /// Constructor based on error description and other exception.
        /// </summary>
        /// <param name="message">Error description.</param>
        /// <param name="innerException">Error that happened earlier.</param>
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
