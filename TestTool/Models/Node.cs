using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using ProcessControlStandarts.OPC.TestTool.Commands;

namespace ProcessControlStandarts.OPC.TestTool.Models
{
	public class Node : INotifyPropertyChanged
	{
		public Node()
		{
			Children = new ObservableCollection<Node>();
		}

		public bool IsExpanded { get; set; }

		public bool IsSelected { get; set; }

		public string Name { get; set; }

		public string Icon
		{
			get { return icon; }
			set
			{
				icon = value;
				NotifyPropertyChanged("Icon");
			}
		}

		public ObservableCollection<Node> Children { get; private set; }

		public virtual IList<Command> Commands { get; protected set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

		public virtual void Dispose()
		{			
		}

		private string icon;
	}
}
