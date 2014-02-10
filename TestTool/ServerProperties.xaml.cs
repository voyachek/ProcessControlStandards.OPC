#region using

using System;
using System.Diagnostics;
using System.Windows.Threading;

using ProcessControlStandarts.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandarts.OPC.TestTool
{
	/// <summary>
	/// Interaction logic for ServerProperties.xaml
	/// </summary>
	public partial class ServerProperties
	{
		private readonly ServerNode node;

		public ServerProperties(ServerNode node)
		{
			this.node = node;
			InitializeComponent();

			properties.Id = node.Id;
			properties.ProgramId = node.Name;
			properties.ServerName = node.ServerName;

			_propertyGrid.IsReadOnly = true;
			_propertyGrid.SelectedObject = properties;

			updatePropertiesTimer.Interval = new TimeSpan(0, 0, 0, 1);
			updatePropertiesTimer.Tick += UpdateProperties;
			updatePropertiesTimer.Start();
			
			Closed += OnClosed;
		}

		private void UpdateProperties(object sender, EventArgs eventArgs)
		{
			node.GetActivePropertiesAsync((task, args) =>
			{
				if(args.Error != null)
					node.Owner.Context.Log.TraceData(TraceEventType.Error, 0, args.Error);
				else
				{
					var serverProperties =
						(DataAccessClient.ServerProperties)args.Result;
					properties.StartTime = serverProperties.StartTime;
					properties.CurrentTime = serverProperties.CurrentTime;
					properties.LastUpdateTime = serverProperties.LastUpdateTime;
					properties.ServerState = serverProperties.ServerState;
					properties.Groups = serverProperties.GroupCount;
					properties.Bandwidth = serverProperties.Bandwidth;
					properties.Version = string.Format("{0}.{1}.{2}",
						serverProperties.MajorVersion, serverProperties.MinorVersion, serverProperties.BuildNumber);
					properties.VendorInfo = serverProperties.VendorInfo;
				}
			});
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
			updatePropertiesTimer.Stop();
		}

		private readonly Models.ServerProperties properties = new Models.ServerProperties();

		private readonly DispatcherTimer updatePropertiesTimer = new DispatcherTimer(); 
	}
}
