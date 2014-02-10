#region using

using ProcessControlStandarts.OPC.TestTool.Models;
using ProcessControlStandarts.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Commands
{
	public class DisconnectServerCommand : Command
	{
		public DisconnectServerCommand() : base(Resources.DisconnectServer)
		{
		}

		public override void Execute(object parameter)
		{
			var node = (ServerNode) parameter;

			node.Disconnect();
		}

		public override bool CanExecute(object parameter)
		{
			var node = (ServerNode) parameter;
			if (node == null)
				return false;

			return node.Connected;
		}
	}
}
