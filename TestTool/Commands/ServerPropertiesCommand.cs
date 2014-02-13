#region using

using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class ServerPropertiesCommand : Command<ServerNode>
	{
		public ServerPropertiesCommand() 
            : base(Resources.Properties)
		{
		}

        protected override void Execute(ServerNode node)
		{
            new ServerProperties(node).ShowDialog();
		}

        protected override bool CanExecute(ServerNode node)
		{
			return true;
		}
	}
}
