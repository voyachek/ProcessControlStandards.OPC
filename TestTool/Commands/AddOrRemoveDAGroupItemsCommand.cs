#region using

using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class AddOrRemoveDAGroupItemsCommand : Command<DAGroupNode>
	{
        public AddOrRemoveDAGroupItemsCommand() 
            : base(Resources.AddDAGroupItems)
		{
		}

        protected override void Execute(DAGroupNode node)
		{
            new AddOrRemoveDAGroupItems(node).ShowDialog();
		}

        protected override bool CanExecute(DAGroupNode node)
		{
			return true;
		}
	}
}
