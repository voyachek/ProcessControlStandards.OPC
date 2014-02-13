#region using

using System.Collections.Generic;

using ProcessControlStandards.OPC.TestTool.Commands;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Models
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

		public override IList<ICommand> Commands
		{
			get { return CommandList; }
			protected set { }
		}

        private static readonly List<ICommand> CommandList = new List<ICommand>
		{
			new RefreshServersCommand()
		};
	}
}
