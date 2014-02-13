#region Using

using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class AddDAGroupCommand : Command<ServerNode>
    {
        public AddDAGroupCommand()
            : base(Resources.AddOPCDAGroup)
		{
		}

        protected override void Execute(ServerNode node)
		{
            new GroupProperties(node, null).ShowDialog();
		}

        protected override bool CanExecute(ServerNode node)
        {
            return node != null && !node.Connected;
        }
    }
}
