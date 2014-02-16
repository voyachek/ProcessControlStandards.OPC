#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using ProcessControlStandards.OPC.Core;
using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Commands;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Models
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

        public override IList<ICommand> Commands
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

		    DoAsync((task, args) =>
		    {
		        server = new DAServer(Id);
		    },
            (task, args) =>
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
            });

            /*
		    serverThread.Post(new WorkerThread.Task
		    {
                Do = (task, args) =>
                {
                    var ip = server.GetItemProperties();

                    var a = server.GetAddressSpaceBrowser();
                    var @enum = a.GetItemIds(BrowseType.Flat, string.Empty, 0, 0).ToList();
                    var iid = a.GetItemId(@enum[5]);

                    var ipi = ip.QueryAvailableProperties(@enum[5]);
                    var ipv = ip.GetItemProperties(@enum[5], new[] { ipi[0].Id, ipi[2].Id });

                    var g = server.AddGroup(1, "test", true, 1000, 0);
                    var res = g.AddItems(new[]
                    {
                        new Item
                        {
                            Active = true,
                            ClientId = 1,
                            ItemId = iid,
                            RequestedDataType = (VarEnum)Convert.ToInt32(ipv[0].Value)
                        },
                        new Item
                        {
                            Active = true,
                            ClientId = 1,
                            ItemId = iid,
                            RequestedDataType = (VarEnum)Convert.ToInt32(ipv[0].Value)
                        }
                    });

                    var val = g.SyncReadItems(DataSource.Device, new[] { res[0].ServerId, res[1].ServerId });
                    g.Dispose();
                },
		    });
             * */
        }

		public void Disconnect()
		{
			if(serverThread == null)
				return;

			Owner.Context.IsBusy = true;

		    DoAsync((task, args) =>
		    {
                if (server != null)
                {
                    server.Dispose();
                    server = null;
                }
		    },
            (task, args) =>
            {
                serverThread.Dispose();
                serverThread = null;

                Children.Clear();

                Icon = "/Images/ServerOff.png";
                Owner.Context.IsBusy = false;
                CommandList.ForEach(command => command.RiseCanExecuteChanged());

                if (args.Error != null)
                    Owner.Context.Log.TraceData(TraceEventType.Warning, 0, args.Error);
            });
		}

		public bool GetActivePropertiesAsync(Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
		{
            return DoAsync((task, args) =>
            {
                args.Result = server.GetProperties();
            },
            completed);
		}

        public bool CreateDAGroupAsync(GroupProperties properties, Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            return DoAsync((task, args) =>
            {
                args.Result = server.AddGroup(
                    Interlocked.Increment(ref daGroupClientId),
                    properties.Name,
                    properties.Active,
                    properties.UpdateRate,
                    properties.PercentDeadband);
            },
            (task, args) =>
            {
                Children.Add(new DAGroupNode(this, (Group)args.Result));

                completed(task, args);
            });
        }

        public bool GetDAItemPropertiesAsync(string itemId, Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            return DoAsync((task, args) =>
            {
                var browser = server.GetItemProperties();
                var definitions = browser.QueryAvailableProperties(itemId);
                var values = browser.GetItemProperties(itemId, definitions.Select(x => x.Id).ToArray());
                args.Result = new KeyValuePair<ItemProperty[], ItemPropertyValue[]>(definitions, values);
            },
            completed);
        }

        public bool DoAsync(Action<WorkerThread.Task, DoWorkEventArgs> action, Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
	    {
            if (serverThread == null)
                return false;

            serverThread.Post(new WorkerThread.Task
            {
                Do = action,
                Completed = (task, args) =>
                {
                    Owner.Context.Log.TraceData(TraceEventType.Error, 0, args.Error);
                    if (completed != null)
                        completed(task, args);
                }
            });

            return true;	        
	    }

	    public ServerAddressSpaceBrowser QueryAddressSpaceBrowser()
	    {
	        return server.GetAddressSpaceBrowser();
	    }

        public ItemProperties QueryPropertyBrowser()
        {
            return server.GetItemProperties();
        }

	    public override void Dispose()
		{
			base.Dispose();

			Disconnect();
		}

		private DAServer server;

		private WorkerThread serverThread;

	    private static int daGroupClientId = 1;

        private static readonly List<ICommand> CommandList = new List<ICommand>
		{
			new ConnectServerCommand(),
            new AddDAGroupCommand(),
			new DisconnectServerCommand(),
			new ServerPropertiesCommand(),
		};
	}
}
