#region using

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
            Closing += OnClosing;
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

		private void OnClosing(object sender, EventArgs eventArgs)
		{
			ServersTree.Dispose();
		}

        private void DetailsViewLoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateDetailsViewDataContext(sender);
        }

	    private void DetailsViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	    {
	        UpdateDetailsViewDataContext(sender);
	    }

        private static void UpdateDetailsViewDataContext(object sender)
        {
            var frame = (Frame)sender;
            var content = frame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = frame.DataContext;
        }
	}
}
