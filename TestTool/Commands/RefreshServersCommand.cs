#region using

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using ProcessControlStandarts.OPC.Core;
using ProcessControlStandarts.OPC.DataAccessClient;
using ProcessControlStandarts.OPC.TestTool.Models;
using ProcessControlStandarts.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Commands
{
	public class RefreshServersCommand : Command
	{
		public RefreshServersCommand() : base(Resources.RefreshCommand)
		{
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += ServersRefreshDoWork;
			worker.RunWorkerCompleted += ServersRefreshWorkerCompleted;
		}

		public override void Execute(object parameter)
		{
			var node = (LocalHostNode) parameter;

			node.Owner.Context.IsBusy = true;
			worker.RunWorkerAsync(node);
		}

		public override bool CanExecute(object parameter)
		{
			return !worker.IsBusy;
		}

		private void ServersRefreshDoWork(object sender, DoWorkEventArgs e)
		{
			var result = new KeyValuePair<LocalHostNode, List<ServerDescription>>(
				(LocalHostNode)e.Argument, new List<ServerDescription>(10));
			e.Result = result;

			using (var @enum = new ServerBrowser())
				result.Value.AddRange(@enum
					.GetEnumerator(DAServer.Version10, DAServer.Version20)
					.TakeWhile(server => !worker.CancellationPending));
		}

		private static void ServersRefreshWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			var result = (KeyValuePair<LocalHostNode, List<ServerDescription>>) e.Result;

			result.Key.Owner.Context.IsBusy = false;
			if (e.Error != null)
				result.Key.Owner.Context.Log.TraceData(TraceEventType.Error, 0, e.Error);
			else
			{
				var servers = result.Value;
				foreach (var info in servers
					.Where(info => result.Key.Children.All(x => x.Name != info.ProgramId)))
					result.Key.Children.Add(new ServerNode(result.Key.Owner, info));

				var serversToRemove = result.Key.Children.Where(
					x => servers.All(y => y.ProgramId != x.Name));
				foreach (var item in serversToRemove)
				{
					item.Dispose();
					result.Key.Children.Remove(item);
				}
			}
		}

		private readonly BackgroundWorker worker = new BackgroundWorker();
	}
}
