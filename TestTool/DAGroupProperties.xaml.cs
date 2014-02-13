#region using

using System;
using System.Diagnostics;

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

            if (groupNode != null)
                UpdateProperties();
			
			Closed += OnClosed;
		}

		private void UpdateProperties()
		{
			serverNode.GetDAGroupPropertiesAsync(groupNode, (task, args) =>
			{
				if(args.Error != null)
					serverNode.Owner.Context.Log.TraceData(TraceEventType.Error, 0, args.Error);
				else
				{
					var groupProperties = (DataAccessClient.GroupProperties)args.Result;
                    properties.Name = groupProperties.Name;
                    properties.ClientId = groupProperties.ClientId;
                    properties.ServerId = groupProperties.ServerId;
                    properties.Active = groupProperties.Active;
                    properties.UpdateRate = groupProperties.UpdateRate;
                    properties.TimeBias = groupProperties.TimeBias;
                    properties.PercentDeadband = groupProperties.PercentDeadband;
                    _propertyGrid.Update();
				}
			});
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
		}

        private readonly ServerNode serverNode;

        private readonly DAGroupNode groupNode;

        private readonly Models.DAGroupProperties properties = new Models.DAGroupProperties();
	}
}
