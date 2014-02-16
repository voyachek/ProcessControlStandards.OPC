#region using

using System.Collections.Generic;

using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
	/// Interaction logic for DAGroupProperties.xaml
	/// </summary>
	public partial class DAGroupItemProperties
	{
        public DAGroupItemProperties(ServerNode serverNode, string itemId)
		{
			InitializeComponent();

            serverNode.GetDAItemPropertiesAsync(itemId, (task, args) =>
            {
                var result = (KeyValuePair<ItemProperty[], ItemPropertyValue[]>) args.Result;
                var properties = new List<DAGroupItemProperty>(result.Key.Length);
                for (var i = 0; i < result.Key.Length; ++i)
                    properties.Add(new DAGroupItemProperty
                    {
                        Id = result.Key[i].Id,
                        Description = result.Key[i].Description,
                        Type = result.Key[i].Type,
                        SubType = result.Key[i].SubType,
                        Value = result.Value[i].Value,
                        Error = result.Value[i].Error,
                    });
                _itemPropertiesList.ItemsSource = properties;
            });                
		}
	}
}
