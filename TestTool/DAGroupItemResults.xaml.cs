#region using

using System.Collections.Generic;

using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
    /// Interaction logic for DAGroupItemResults.xaml
	/// </summary>
    public partial class DAGroupItemResults
	{
        public DAGroupItemResults(IList<Item> items, IList<ItemResult> results)
		{
			InitializeComponent();

            var itemResults = new DAGroupItem[items.Count];
            for (var i = 0; i < itemResults.Length; ++i)
            {
                itemResults[i] = new DAGroupItem
                {
                    Name = items[i].ItemId,
                    ServerId = results[i].ServerId,
                    CanonicalDataType = results[i].CanonicalDataType,
                    CanonicalDataSubType = results[i].CanonicalDataSubType,
                    AccessRights = results[i].AccessRights,
                    Error = results[i].Error,
                };
            }

            _itemPropertiesList.ItemsSource = itemResults;
		}
	}
}
