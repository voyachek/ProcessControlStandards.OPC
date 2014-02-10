#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

using ProcessControlStandarts.OPC.Core;
using ProcessControlStandarts.OPC.DataAccessClient;
using ProcessControlStandarts.OPC.TestTool.Commands;

#endregion

namespace ProcessControlStandarts.OPC.TestTool.Models
{
	public class ServerNode : Node
	{
		public ServerNode(ServersTree owner, ServerDescription description)
		{
			Owner = owner;
			Id = description.Id;
			Name = description.ProgramId;
			ServerName = description.Name;
			IsExpanded = true;
			Icon = "/Images/ServerOff.png";
		}

		public override IList<Command> Commands
		{
			get { return CommandList; }
			protected set { }
		}

		public ServersTree Owner { get; private set; }

		public string ServerName { get; private set; }

		public Guid Id { get; private set; }

		public bool Connected 
		{
			get { return serverThread != null; }
		}

		public void Connect()
		{
			if(serverThread != null)
				return;

			Owner.Context.IsBusy = true;
			serverThread = new WorkerThread("Server " + Name);

			serverThread.Post(new WorkerThread.Task
			{
				Do = (task, args) =>
				{
					server = new DAServer(Id);
				},

				Completed = (task, args) =>
				{
					Owner.Context.IsBusy = false;

					if (args.Error != null)
					{
						Owner.Context.Log.TraceData(TraceEventType.Error, 0, args.Error);

						Disconnect();
					}
					else
					{
						Icon = "/Images/ServerOn.png";
					}

					CommandList.ForEach(command => command.RiseCanExecuteChanged());
				}
			});
		}

		public void Disconnect()
		{
			if(serverThread == null)
				return;

			Owner.Context.IsBusy = true;

			serverThread.Post(new WorkerThread.Task
			{
				Do = (task, args) =>
				{
					if (server != null)
					{
						server.Dispose();
						server = null;
					}
				},

				Completed = (task, args) =>
				{
					serverThread.Dispose();
					serverThread = null;

					Icon = "/Images/ServerOff.png";
					Owner.Context.IsBusy = false;
					CommandList.ForEach(command => command.RiseCanExecuteChanged());

					if(args.Error != null)
						Owner.Context.Log.TraceData(TraceEventType.Warning, 0, args.Error);
				}
			});
		}

		public bool GetActivePropertiesAsync(Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
		{
			if (serverThread == null)
				return false;

			serverThread.Post(new WorkerThread.Task
			{
				Do = (task, args) =>
				{
					args.Result = server.GetProperties();
				},

				Completed = completed,
			});

			return true;
		}

		public override void Dispose()
		{
			base.Dispose();

			Disconnect();
		}

		private DAServer server;

		private WorkerThread serverThread;

		private static readonly List<Command> CommandList = new List<Command>
		{
			new ConnectServerCommand(),
			new DisconnectServerCommand(),
			new ServerPropertiesCommand(),
		};
	}
}
