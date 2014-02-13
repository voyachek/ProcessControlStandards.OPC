#region using

using System;
using System.ComponentModel;

using ProcessControlStandards.OPC.DataAccessClient;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Models
{
	public class ServerProperties : INotifyPropertyChanged
	{
		[Category("Basic")]
		public Guid Id { get; set; }

		[Category("Basic")]
		public string ProgramId { get; set; }
		
		[Category("Basic")]
		public string ServerName { get; set; }

		[Category("Operative")]
		//[DisplayName("")]
		//[Description("This property uses a TextBox as the default editor.")]
		public DateTime? StartTime
		{
			get { return startTime; }
			set
			{
				startTime = value;

				OnPropertyChanged("StartTime");
			}
		}

		[Category("Operative")]
		public DateTime? CurrentTime
		{
			get { return currentTime; }
			set
			{
				currentTime = value;

				OnPropertyChanged("CurrentTime");
			}
		}

		[Category("Operative")]
		public DateTime? LastUpdateTime
		{
			get { return lastUpdateTime; }
			set
			{			
				lastUpdateTime = value;

				OnPropertyChanged("LastUpdateTime");
			}
		}

		[Category("Operative")]
		public ServerState? ServerState
		{
			get { return serverState; }
			set
			{				
				serverState = value;

				OnPropertyChanged("ServerState");
			}
		}

		[Category("Operative")]
		public int? Groups
		{
			get { return groups; }
			set
			{
				groups = value;

				OnPropertyChanged("Groups");
			}
		}

		[Category("Operative")]
		public int? Bandwidth
		{
			get { return bandwidth; }
			set
			{
				bandwidth = value;

				OnPropertyChanged("Bandwidth");
			}
		}

		[Category("Operative")]
		public string Version
		{
			get { return version; }
			set
			{		
				version = value;

				OnPropertyChanged("Version");
			}
		}

		[Category("Operative")]
		public string VendorInfo
		{
			get { return vendorInfo; }
			set
			{
				vendorInfo = value;

				OnPropertyChanged("VendorInfo");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null) 
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private int? bandwidth;

		private int? groups;

		private DateTime? currentTime;

		private DateTime? lastUpdateTime;

		private ServerState? serverState;

		private DateTime? startTime;

		private string version;

		private string vendorInfo;
	}
}
