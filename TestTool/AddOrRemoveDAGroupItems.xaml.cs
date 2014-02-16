#region using

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
    /// Interaction logic for AddOrRemoveDAGroupItems.xaml
	/// </summary>
	public partial class AddOrRemoveDAGroupItems
	{
        public AddOrRemoveDAGroupItems(DAGroupNode groupNode)
		{
            this.groupNode = groupNode;
			InitializeComponent();

            groupNode.GetItemsFlatAsync((task, args) =>
            {
                var result = (string[])args.Result;
                items = new ObservableCollection<DAGroupItem>(result.Select(x =>new DAGroupItem
                {
                    Name = x,
                    Selected = groupNode.Items.Any(y => y.Name == x)
                }));
                _itemList.ItemsSource = items;
            });
		}

        private void OnAcceptButton(object sender, RoutedEventArgs e)
        {
            groupNode.SetItemsAsync(items, (task, args) =>
            {
                var result = (DAGroupNode.AddRemoveResult)args.Result;
                if (result.AddResult.Any(x => x.Error != 0) || result.RemoveResult.Any(x => x != 0))
                    new AddOrRemoveDAGroupItemResults(result).ShowDialog();                   
            });

            Close();
        }

        private void OnValidateButton(object sender, RoutedEventArgs e)
        {
            groupNode.ValidateItemsAsync(items.Where(x => x.Selected).Select(x => x.Name).ToList(), (task, args) =>
            {
                var result = (KeyValuePair<IList<Item>, IList<ItemResult>>)args.Result;
                new DAGroupItemResults(result.Key, result.Value).ShowDialog();
            });
        }

        private void ShowProperties(object sender, RequestNavigateEventArgs e)
        {
            new DAGroupItemProperties(
                groupNode.Server, 
                ((Hyperlink)e.Source).Tag.ToString()).ShowDialog();
        }

        private readonly DAGroupNode groupNode;

        private ObservableCollection<DAGroupItem> items = new ObservableCollection<DAGroupItem>();
	}
}
