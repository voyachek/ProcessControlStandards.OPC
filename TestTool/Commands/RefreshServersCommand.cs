#region using

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using ProcessControlStandards.OPC.Core;
using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Models;
using ProcessControlStandards.OPC.TestTool.Properties;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Commands
{
    public class RefreshServersCommand : Command<LocalHostNode>
	{
		public RefreshServersCommand() 
            : base(Resources.RefreshCommand)
		{
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += ServersRefreshDoWork;
			worker.RunWorkerCompleted += ServersRefreshWorkerCompleted;
		}

        protected override void Execute(LocalHostNode node)
		{
			node.Owner.Context.IsBusy = true;
			worker.RunWorkerAsync(node);
		}

        protected override bool CanExecute(LocalHostNode node)
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
