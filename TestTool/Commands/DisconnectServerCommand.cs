#region using

using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class DisconnectServerCommand : Command<ServerNode>
	{
		public DisconnectServerCommand() 
            : base(Resources.DisconnectServer)
		{
		}

        protected override void Execute(ServerNode node)
		{
			node.Disconnect();
		}

        protected override bool CanExecute(ServerNode node)
        {
            return node != null && node.Connected;
        }
	}
}
