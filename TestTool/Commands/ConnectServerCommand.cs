#region using

using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class ConnectServerCommand : Command<ServerNode>
	{
		public ConnectServerCommand() 
            : base(Resources.ConnectServer)
		{
		}

        protected override void Execute(ServerNode node)
		{
			node.Connect();
		}

        protected override bool CanExecute(ServerNode node)
		{
			return node != null && node.Connected;
		}
	}
}
