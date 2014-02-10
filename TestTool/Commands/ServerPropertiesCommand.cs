#region using

using ProcessControlStandarts.OPC.TestTool.Models;
using ProcessControlStandarts.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Commands
{
	public class ServerPropertiesCommand : Command
	{
		public ServerPropertiesCommand() : base(Resources.Properties)
		{
		}

		public override void Execute(object parameter)
		{
			var node = (ServerNode) parameter;

			var propertiesDialog = new ServerProperties(node);
			propertiesDialog.ShowDialog();
		}

		public override bool CanExecute(object parameter)
		{
			return true;
		}
	}
}
