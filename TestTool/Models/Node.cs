using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using ProcessControlStandards.OPC.TestTool.Commands;

namespace ProcessControlStandards.OPC.TestTool.Models
{
	public class Node : INotifyPropertyChanged
	{
		public Node()
		{
			Children = new ObservableCollection<Node>();
            DetailsView = new NodeDetailsView();
		}

		public bool IsExpanded { get; set; }

		public bool IsSelected { get; set; }

		public string Name { get; protected set; }

        public NodeDetailsView DetailsView { get; protected set; }

	    public string Icon
		{
			get { return icon; }
			protected set
			{
				icon = value;
				NotifyPropertyChanged("Icon");
			}
		}

		public ObservableCollection<Node> Children { get; private set; }

        public virtual IList<ICommand> Commands { get; protected set; }

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
