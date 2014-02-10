#region using

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandarts.OPC.Core;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
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

		public ItemProperty[] QueryAvailableProperties(string itemId)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");

			uint size;
			int[] ids;
			string[] descriptions;
			VarEnum[] types;
			itemProperties.QueryAvailableProperties(
				itemId, out size, out ids, out descriptions, out types);

			var result = new ItemProperty[size];
			for(var i = 0; i < size; i++)
			{
				result[i].Id = ids[i];
				result[i].Description = descriptions[i];
				result[i].Type = types[i];
			}

			return result;
		}

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemPropertyValue[] GetItemProperties(string itemId, int[] propertyIds)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");
			propertyIds.ArgumentNotNull("propertyIds");
			if(propertyIds.Length == 0)
				return new ItemPropertyValue[0];

			var data = new IntPtr[propertyIds.Length];
			var dataPtr = IntPtr.Zero;
			try
			{
				int[] errors;
				itemProperties.GetItemProperties(
					itemId, (uint)propertyIds.Length, propertyIds, out dataPtr, out errors);

				var results = new ItemPropertyValue[propertyIds.Length];

				var dataPtrAsLong = dataPtr.ToInt64();
				for(var i = 0; i < propertyIds.Length; i++)
				{
					data[i] = new IntPtr(dataPtrAsLong + i * NativeMethods.VariantSize);

					results[i].Value = Marshal.GetObjectForNativeVariant(data[i]);
					results[i].Error = errors[i];
				}

				return results;
			}
			finally
			{
				foreach (var ptr in data)
					if(ptr != IntPtr.Zero)
						NativeMethods.VariantClear(ptr);
				if(dataPtr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(dataPtr);
			}
		}

		public ItemPropertyId[] LookupItemIds(string itemId, int[] propertyIds)
		{
			itemId.ArgumentNotNullOrEmpty("itemId");
			propertyIds.ArgumentNotNull("propertyIds");
			if(propertyIds.Length == 0)
				return new ItemPropertyId[0];

			string[] newItemIds;
			int[] errors;
			itemProperties.LookupItemIDs(
				itemId, (uint)propertyIds.Length, propertyIds, out newItemIds, out errors);

			var results = new ItemPropertyId[propertyIds.Length];

			for(var i = 0; i < propertyIds.Length; i++)
			{
				results[i].Id = newItemIds[i];
				results[i].Error = errors[i];
			}

			return results;
		}

		private readonly IOPCItemProperties itemProperties;
	}
}
