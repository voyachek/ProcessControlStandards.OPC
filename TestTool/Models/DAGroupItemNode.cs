using ProcessControlStandards.OPC.DataAccessClient;

namespace ProcessControlStandards.OPC.TestTool.Models
{
	public class DAGroupItemNode : Node
	{
        public DAGroupItemNode(Item item, ItemResult result)
        {
            Name = item.ItemId + (result.Error != 0 ? "(" + result.Error + ")" : "");
            Icon = "/Images/DAGroup.png";
	    }
	}
}
