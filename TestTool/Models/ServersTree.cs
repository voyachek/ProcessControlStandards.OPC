#region using

using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace ProcessControlStandards.OPC.TestTool.Models
{
	public class ServersTree
	{
		public IRunContext Context { get; set; }

		public ServersTree(IRunContext runContext)
		{
			Context = runContext;
			Children = new ObservableCollection<Node>
			{
				new LocalHostNode(this),
			};
		}

		public ObservableCollection<Node> Children { get; private set; }

		public void Dispose()
		{
			DisposeChildren(Children);
		}

		private static void DisposeChildren(IEnumerable<Node> nodes)
		{
			foreach (var node in nodes)
			{
				DisposeChildren(node.Children);
				node.Dispose();
			}
		}
	}
}
