
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

using ProcessControlStandards.OPC.DataAccessClient;
using ProcessControlStandards.OPC.TestTool.Commands;

namespace ProcessControlStandards.OPC.TestTool.Models
{
	public class DAGroupNode : Node
	{
        public DAGroupNode(ServerNode server, Group group)
        {
            Server = server;
            Group = group;
            Items = new ObservableCollection<DAGroupItem>();

            Name = group.Name;
            Icon = "/Images/DAGroup.png";
            DetailsView = new NodeDetailsView
            {
                Page = new Uri("DAGroupItems.xaml", UriKind.Relative),
                Data = Items,
            };

            Group.DataChange += OnDataChange;
            Group.ReadComplete += OnDataChange;
	    }

	    public override IList<ICommand> Commands
        {
            get { return CommandList; }
            protected set { }
        }

	    public Group Group { get; private set; }

	    public ServerNode Server { get; private set; }

        public ObservableCollection<DAGroupItem> Items { get; private set; }

	    private static readonly List<ICommand> CommandList = new List<ICommand>
		{
            new DAGroupPropertiesCommand(),
            new AddOrRemoveDAGroupItemsCommand(),
		};

        public bool GetPropertiesAsync(Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            return Server.DoAsync((task, args) =>
            {
                args.Result = Group.GetProperties();
            },
            completed);
        }

        public bool ChangePropertiesAsync(GroupProperties properties, Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            return Server.DoAsync((task, args) => Group.SetProperties(properties),
            completed);
        }

        public bool GetItemsFlatAsync(Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            return Server.DoAsync((task, args) =>
            {
                var browser = Server.QueryAddressSpaceBrowser();

                args.Result = browser.GetItemIds(
                    BrowseType.Flat, string.Empty, VarEnum.VT_EMPTY, 0).ToArray();
            },
            completed);
        }

        public bool ValidateItemsAsync(IList<string> itemIds, Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            return Server.DoAsync((task, args) =>
            {
                var items = itemIds.Select(x => new Item
                {
                    ItemId = x,
                    RequestedDataType = VarEnum.VT_EMPTY,
                    Active = true,
                    ClientId = Interlocked.Increment(ref daItemClientId),

                }).ToArray();

                var result = Group.ValidateItems(items);

                args.Result = new KeyValuePair<IList<Item>, IList<ItemResult>>(items, result);
            },
            completed);
        }

        public bool SetItemsAsync(IList<DAGroupItem> itemIds, Action<WorkerThread.Task, RunWorkerCompletedEventArgs> completed)
        {
            var itemsToAdd = new List<DAGroupItem>(itemIds.Count);
            var itemsToRemove = new List<DAGroupItem>(itemIds.Count);
            itemsToAdd.AddRange(itemIds
                .Where(x => x.Selected)
                .Where(itemId => Items.All(x => x.Name != itemId.Name)));
            itemsToRemove.AddRange(itemIds
                .Where(item => !item.Selected)
                .Select(item => Items.FirstOrDefault(x => x.Name == item.Name))
                .Where(itemToRemove => itemToRemove != null));

            return Server.DoAsync((task, args) =>
            {
                var items = itemsToAdd.Select(x => new Item
                {
                    ItemId = x.Name,
                    RequestedDataType = VarEnum.VT_EMPTY,
                    Active = true,
                    ClientId = Interlocked.Increment(ref daItemClientId),

                }).ToArray();

                var addResult = Group.AddItems(items);
                var removeResult = Group.RemoveItems(itemsToRemove.Select(x => x.ServerId).ToArray());

                args.Result = new AddRemoveResult
                {
                    ToAdd = items,
                    AddResult = addResult,
                    ToRemove = itemsToRemove,
                    RemoveResult = removeResult,
                };
            },
            (task, args) =>
            {
                var result = (AddRemoveResult)args.Result;
                for (var i = 0; i < result.ToRemove.Count; ++i)
                    if (result.RemoveResult[i] == 0)
                        Items.Remove(result.ToRemove[i]);

                for (var i = 0; i < result.ToAdd.Length; ++i)
                    if (result.AddResult[i].Error == 0)
                        Items.Add(new DAGroupItem
                        {
                            Name = result.ToAdd[i].ItemId,
                            ClientId = result.ToAdd[i].ClientId,
                            ServerId = result.AddResult[i].ServerId,
                            CanonicalDataType = result.AddResult[i].CanonicalDataType,
                            CanonicalDataSubType = result.AddResult[i].CanonicalDataSubType,
                            AccessRights = result.AddResult[i].AccessRights,
                        });

                if (completed != null)
                    completed(task, args);
            });
        }

	    public override void Dispose()
	    {
	        base.Dispose();

            Group.Dispose();
	    }

	    private void OnDataChange(object sender, DataChangeEventArgs dataChangeEventArgs)
        {
            Server.DoAsync((task, args) => {}, (task, args) =>
            {
                foreach (var itemValue in dataChangeEventArgs.Values)
                {
                    var item = Items.FirstOrDefault(x => x.ClientId == itemValue.ClientId);
                    if (item != null)
                    {
                        item.Error = itemValue.Error;
                        if (itemValue.Error == 0)
                        {
                            item.Value = itemValue.Value;
                            item.Quality = itemValue.Quality;
                            item.Timestamp = itemValue.Timestamp;
                        }
                        item.Refreshed();
                    }
                }
            });
        }

	    public struct AddRemoveResult
	    {
            public Item[] ToAdd;

            public ItemResult[] AddResult;

            public List<DAGroupItem> ToRemove;

	        public int[] RemoveResult;
	    }

	    private static int daItemClientId = 1;
	}
}
