#region using

using AutoMapper;

using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
	/// Interaction logic for DAGroupProperties.xaml
	/// </summary>
	public partial class DAGroupProperties
	{
        public DAGroupProperties(ServerNode serverNode, DAGroupNode groupNode)
		{
			this.serverNode = serverNode;
            this.groupNode = groupNode;
			InitializeComponent();

            properties.Name = groupNode != null ? groupNode.Name : string.Empty;

			_propertyGrid.IsReadOnly = false;
			_propertyGrid.SelectedObject = properties;
            _acceptButton.Content = groupNode != null ? "OK" : "Create";

            if (groupNode != null)
            {
                groupNode.GetPropertiesAsync((task, args) =>
                {
                    if (args.Result != null)
                    {
                        var groupProperties = (GroupProperties)args.Result;
                        Mapper.Map(groupProperties, properties);
                        properties.PropertiesChanged();
                    }
                });                
            }
		}

        private void OnAcceptButton(object sender, System.Windows.RoutedEventArgs e)
        {
            if (groupNode == null)
            {
                serverNode.CreateDAGroupAsync(
                    Mapper.Map<Models.DAGroupProperties, GroupProperties>(properties),
                    (task, args) => Close());
            }
            else
            {
                groupNode.ChangePropertiesAsync(
                    Mapper.Map<Models.DAGroupProperties, GroupProperties>(properties),
                    (task, args) => Close());
            }
        }

        private readonly ServerNode serverNode;

        private readonly DAGroupNode groupNode;

        private readonly Models.DAGroupProperties properties = new Models.DAGroupProperties();
	}
}
