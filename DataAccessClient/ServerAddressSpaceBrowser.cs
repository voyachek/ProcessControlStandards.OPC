#region using

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using ProcessControlStandards.OPC.Core;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Helps to read OPC DA Server namespace.
    /// </summary>
	public class ServerAddressSpaceBrowser
	{
        internal ServerAddressSpaceBrowser(IOPCServer server, int blockSize)
        {
            this.blockSize = blockSize;
			try
			{
				browseServerSpace = (IOPCBrowseServerAddressSpace) server;
			}
			catch (InvalidCastException)
			{
				throw new NotSupportedException();
			}
		}

        /// <summary>
        /// Queries namespace organization.
        /// </summary>
		public NamespaceType NamespaceType
		{
			get
			{
				NamespaceType namespaceType;
				browseServerSpace.QueryOrganization(out namespaceType);
				return namespaceType;					
			}
		}

        /// <summary>
        /// Change current browse position.
        /// </summary>
        /// <param name="direction">The direction to browse OPC Server namespace. See <see cref="BrowseDirection"/>.</param>
        /// <param name="filter">Mask of item/folder name.</param>
		public void ChangeBrowsePosition(BrowseDirection direction, string filter)
		{
			browseServerSpace.ChangeBrowsePosition(direction, filter);				
		}

        /// <summary>
        /// Retrieves enumerator of current branch.
        /// </summary>
        /// <param name="type">Namespace browse mode. See <see cref="BrowseType"/></param>
        /// <param name="filterCriteria">Mask of item/folder name.</param>
        /// <param name="dataTypeFilter">Filter by type.</param>
        /// <param name="accessRightsFilter">Filter by access rights.</param>
        /// <returns>Enumerator of current namespace level.</returns>
		public IEnumerable<string> GetItemIds(BrowseType type, string filterCriteria, VarEnum dataTypeFilter, int accessRightsFilter)
		{
            IEnumString enumerator;
			browseServerSpace.BrowseOPCItemIDs(
				type, 
				filterCriteria,
                (short)dataTypeFilter, 
				accessRightsFilter, 
				out enumerator);								

			try
			{
				enumerator.Reset();
			    var itemBlock = new Queue<string>(blockSize);
				while(true)
				{
				    if (itemBlock.Count == 0)
				    {
                        var itemIds = new string[blockSize];
                        uint fetched;
                        var res = enumerator.Next((uint)blockSize, itemIds, out fetched);
                        if (res > 1)
                            Marshal.ThrowExceptionForHR(res);
                        for(var i = 0; i < fetched; i++)
                            itemBlock.Enqueue(itemIds[i]);
                    }

                    if (itemBlock.Count == 0)
						break;

                    yield return itemBlock.Dequeue();					
				}            
			}
			finally
			{
				if(enumerator != null)
					Marshal.ReleaseComObject(enumerator);
			}
		}

        /// <summary>
        /// Gets the full item name of the specified name in the current branch.
        /// </summary>
        /// <param name="itemDataId">Item short name.</param>
        /// <returns>Item full name.</returns>
		public string GetItemId(string itemDataId)
		{
			itemDataId.ArgumentNotNullOrEmpty("itemDataId");

			string result;
			browseServerSpace.GetItemID(itemDataId, out result);				
			return result;
		}

        /// <summary>
        /// Retrieves enumerator of access path to specified item.
        /// </summary>
        /// <param name="itemId">Name of item.</param>
        /// <returns>Enumerator of access path to specified item.</returns>
		public IEnumerable<string> BrowseAccessPaths(string itemId)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");

			IEnumString enumerator;
			browseServerSpace.BrowseAccessPaths(itemId, out enumerator);

			try
			{
				enumerator.Reset();
                var pathsBlock = new Queue<string>(blockSize);
                while (true)
				{
                    if (pathsBlock.Count == 0)
				    {
                        var paths = new string[blockSize];
                        uint fetched;
                        var res = enumerator.Next((uint)blockSize, paths, out fetched);
                        if (res > 1)
                            Marshal.ThrowExceptionForHR(res);
                    }

                    if (pathsBlock.Count == 0)
                        break;

                    yield return pathsBlock.Dequeue();
				}
			}
			finally
			{
				if(enumerator != null)
					Marshal.ReleaseComObject(enumerator);
			}
		}

	    private readonly int blockSize;

		private readonly IOPCBrowseServerAddressSpace browseServerSpace;
	}
}
