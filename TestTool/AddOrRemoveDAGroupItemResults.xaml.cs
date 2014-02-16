#region using

using System.Collections.Generic;

using ProcessControlStandards.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
    /// Interaction logic for AddOrRemoveDAGroupItemResults.xaml
	/// </summary>
    public partial class AddOrRemoveDAGroupItemResults
	{
        public AddOrRemoveDAGroupItemResults(DAGroupNode.AddRemoveResult result)
		{
			InitializeComponent();

            var addedItems = new List<DAGroupItem>(result.ToAdd.Length);
            for (var i = 0; i < result.AddResult.Length; i++)
            {
                if (result.AddResult[i].Error != 0)
                {
                    addedItems.Add(new DAGroupItem
                    {
                        Name = result.ToAdd[i].ItemId,
                        Error = result.AddResult[i].Error,
                    });
                }
            }
            _itemToAddList.ItemsSource = addedItems;

            var removedItems = new List<DAGroupItem>(result.ToRemove.Count);
            for (var i = 0; i < result.RemoveResult.Length; i++)
            {
                if (result.RemoveResult[i] != 0)
                {
                    result.ToRemove[i].Error = result.RemoveResult[i];
                    removedItems.Add(result.ToRemove[i]);
                }
            }
            _itemToRemoveList.ItemsSource = removedItems;
        }
	}
}
