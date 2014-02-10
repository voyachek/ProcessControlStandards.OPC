#region using

using System.Collections.Generic;

using ProcessControlStandarts.OPC.TestTool.Commands;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Models
{
	public class LocalHostNode : Node
	{
		public LocalHostNode(ServersTree owner)
		{
			Owner = owner;
			Name = "localhost";
			IsExpanded = true;
			Icon = "/Images/Localhost.png";
		}

		public ServersTree Owner { get; private set; }

		public override IList<Command> Commands
		{
			get { return CommandList; }
			protected set { }
		}

		private static readonly List<Command> CommandList = new List<Command>
		{
			new RefreshServersCommand()
		};
	}
}
