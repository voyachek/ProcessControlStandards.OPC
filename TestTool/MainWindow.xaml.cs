#region using

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

using ProcessControlStandards.OPC.TestTool.Commands;
using ProcessControlStandards.OPC.TestTool.Models;

#endregion

namespace ProcessControlStandards.OPC.TestTool
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : IRunContext
	{
		public MainWindow()
		{
			InitializeComponent();

			Log = new TraceSource("Main");
			ServersTree = new ServersTree(this);
			DataContext = ServersTree;

			Loaded += OnLoaded;
			Closed += OnClosed;
		}

		public ServersTree ServersTree { get; private set; }

		public bool IsBusy 
		{
			get { return _busyIndicator.IsBusy; }
			set { _busyIndicator.IsBusy = value; } 
		}

		public TraceSource Log { get; private set; }

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var node = (LocalHostNode) ServersTree.Children.First(x => x is LocalHostNode);
			node.Commands.First(x => x is RefreshServersCommand).Execute(node);
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
			ServersTree.Dispose();
		}
	}
}
