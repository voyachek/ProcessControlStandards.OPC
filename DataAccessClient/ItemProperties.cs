#region using

using System;
using System.Security.Permissions;

using ProcessControlStandards.OPC.Core;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
    /// <summary>
    /// Access to OPC DA group item properties.
    /// </summary>
	public class ItemProperties
	{
		internal ItemProperties(IOPCServer server)
		{
			try
			{
				itemProperties = (IOPCItemProperties) server;
			}
			catch (InvalidCastException)
			{
				throw new NotSupportedException();
			}
		}

        /// <summary>
        /// Retrieves list of item property descriptions.
        /// </summary>
        /// <param name="itemId">Name of item.</param>
        /// <returns>List of item property descriptions.</returns>
        [SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemProperty[] QueryAvailableProperties(string itemId)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");
            
            uint size;
            IntPtr idsPtr, descriptionsPtr, typesPtr;
            itemProperties.QueryAvailableProperties(
                itemId, out size, out idsPtr, out descriptionsPtr, out typesPtr);

            return ItemPropertyResultReader.ReadItemProperties(
                size, idsPtr, descriptionsPtr, typesPtr);
		}

        /// <summary>
        /// Retrieves list of item properties.
        /// </summary>
        /// <param name="itemId">Name of item.</param>
        /// <param name="propertyIds">ID of properties.</param>
        /// <returns>List of item properties.</returns>
		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemPropertyValue[] GetItemProperties(string itemId, int[] propertyIds)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");
			propertyIds.ArgumentNotNull("propertyIds");
			if(propertyIds.Length == 0)
				return new ItemPropertyValue[0];

            IntPtr dataPtr, errorsPtr;
			itemProperties.GetItemProperties(
                itemId, (uint)propertyIds.Length, propertyIds, out dataPtr, out errorsPtr);

			return ItemPropertyResultReader.ReadItemPropertyValues(
			    propertyIds.Length, dataPtr, errorsPtr);
		}

        /// <summary>
        /// Retrieves item property names.
        /// </summary>
        /// <param name="itemId">Name of item.</param>
        /// <param name="propertyIds">ID of properties.</param>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.LinkDemand)] 
        public ItemPropertyId[] LookupItemIds(string itemId, int[] propertyIds)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");
			propertyIds.ArgumentNotNull("propertyIds");
			if(propertyIds.Length == 0)
				return new ItemPropertyId[0];

            IntPtr dataPtr, errorsPtr;
            itemProperties.LookupItemIDs(
                itemId, (uint)propertyIds.Length, propertyIds, out dataPtr, out errorsPtr);

            return ItemPropertyResultReader.ReadItemPropertyIds(
                propertyIds.Length, dataPtr, errorsPtr);
		}

		private readonly IOPCItemProperties itemProperties;
	}
}
