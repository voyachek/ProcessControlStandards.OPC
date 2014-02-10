#region using

using System;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	public class ServerDescription
	{
		public ServerDescription(Guid id, string programId, string versionIndependedProgramId, string name)
		{
			Id = id;
			ProgramId = programId;
			VersionIndependedProgramId = versionIndependedProgramId;
			Name = name;
		}

		public Guid Id { get; private set; }

		public string ProgramId { get; private set; }
		
		public string VersionIndependedProgramId { get; private set; }

		public string Name { get; private set; }
	}
}
