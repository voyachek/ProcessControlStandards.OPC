#region using

using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class DAGroupPropertiesCommand : Command<DAGroupNode>
	{
        public DAGroupPropertiesCommand() 
            : base(Resources.Properties)
		{
		}

        protected override void Execute(DAGroupNode node)
		{
            new DAGroupProperties(node.Server, node).ShowDialog();
		}

        protected override bool CanExecute(DAGroupNode node)
		{
			return true;
		}
	}
}
