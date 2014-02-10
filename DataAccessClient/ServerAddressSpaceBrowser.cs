#region using

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ProcessControlStandarts.OPC.Core;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	public class ServerAddressSpaceBrowser
	{
		internal ServerAddressSpaceBrowser(IOPCServer server)
		{
			try
			{
				browseServerSpace = (IOPCBrowseServerAddressSpace) server;
			}
			catch (InvalidCastException)
			{
				throw new NotSupportedException();
			}
		}

		public NamespaceType NamespaceType
		{
			get
			{
				NamespaceType namespaceType;
				browseServerSpace.QueryOrganization(out namespaceType);
				return namespaceType;					
			}
		}

		public void ChangeBrowsePosition(BrowseDirection direction, string filter)
		{
			browseServerSpace.ChangeBrowsePosition(direction, filter);				
		}

		public IEnumerable<string> GetItemIds(BrowseType type, string filterCriteria, short dataTypeFilter, int accessRightsFilter)
		{
			IEnumString enumerator;
			browseServerSpace.BrowseOPCItemIDs(
				type, 
				filterCriteria, 
				dataTypeFilter, 
				accessRightsFilter, 
				out enumerator);								

			try
			{
				enumerator.Reset();
				while(true)
				{
					string itemId;
					uint fetched;
					enumerator.RemoteNext(1, out itemId, out fetched);
					if(fetched == 0)
						break;

					yield return itemId;					
				}            
			}
			finally
			{
				if(enumerator != null)
					Marshal.ReleaseComObject(enumerator);
			}
		}

		public string GetItemId(string itemDataId)
		{
			itemDataId.ArgumentNotNullOrEmpty("itemDataId");

			string result;
			browseServerSpace.GetItemID(itemDataId, out result);				
			return result;
		}

		public IEnumerable<string> BrowseAccessPaths(string itemId)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");

			IEnumString enumerator;
			browseServerSpace.BrowseAccessPaths(itemId, out enumerator);

			try
			{
				enumerator.Reset();
				while(true)
				{
					string path;
					uint fetched;
					enumerator.RemoteNext(1, out path, out fetched);
					if(fetched == 0)
						break;

					yield return path;
				}
			}
			finally
			{
				if(enumerator != null)
					Marshal.ReleaseComObject(enumerator);
			}
		}

		private readonly IOPCBrowseServerAddressSpace browseServerSpace;
	}
}
