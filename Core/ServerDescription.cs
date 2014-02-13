#region using

using System;

#endregion

namespace ProcessControlStandards.OPC.Core
{
    /// <summary>
    /// Information about installed OPC Server.
    /// </summary>
	public class ServerDescription
	{
        /// <summary>
        /// Constructor based on information about installed OPC Server.
        /// </summary>
        /// <param name="id">UUID of OPC Server.</param>
        /// <param name="programId">Program ID of OPC Server.</param>
        /// <param name="versionIndependentProgramId">Independent Program ID of OPC Server from Version .</param>
        /// <param name="name">Name of OPC Server.</param>
		public ServerDescription(Guid id, string programId, string versionIndependentProgramId, string name)
		{
			Id = id;
			ProgramId = programId;
			VersionIndependentProgramId = versionIndependentProgramId;
			Name = name;
		}

        /// <summary>
        /// Constructor based of error that occurred in the process of obtaining information.
        /// </summary>
        /// <param name="id">UUID of OPC Server.</param>
        /// <param name="error">Error that occurred in the process of obtaining information.</param>
        public ServerDescription(Guid id, Exception error)
        {
            Id = id;
            Error = error;
        }

        /// <summary>
        /// UUID of OPC Server.
        /// </summary>
		public Guid Id { get; private set; }

        /// <summary>
        /// Program ID of OPC Server.
        /// </summary>
		public string ProgramId { get; private set; }
		
        /// <summary>
        /// Independent Program ID of OPC Server from Version.
        /// </summary>
		public string VersionIndependentProgramId { get; private set; }

        /// <summary>
        /// Name of OPC Server.
        /// </summary>
		public string Name { get; private set; }

        /// <summary>
        /// Error that occurred in the process of obtaining information.
        /// </summary>
	    public Exception Error { get; private set; }
	}
}
