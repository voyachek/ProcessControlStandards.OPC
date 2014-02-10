#region using

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	class ItemResultReader : IDisposable
	{
		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemResultReader(ICollection<Item> items)
		{
            Items = Marshal.AllocCoTaskMem(ItemSize * items.Count);
            stringsToClear = new List<IntPtr>(items.Count * 2);

            var position = 0;
            foreach (var item in items)
            {
				// string szAccessPath;
	            var accessPath = Marshal.StringToCoTaskMemUni(item.AccessPath);
				stringsToClear.Add(accessPath);
				Marshal.WriteIntPtr(Items, position, accessPath);
				position += IntPtr.Size;

				// string szItemID;
	            var itemId = Marshal.StringToCoTaskMemUni(item.ItemId);
				stringsToClear.Add(itemId);
				Marshal.WriteIntPtr(Items, position, itemId);
				position += IntPtr.Size;

				// int bActive;
				Marshal.WriteInt32(Items, position, item.Active ? 1 : 0);
				position += sizeof(int);

				// uint hClient;
				Marshal.WriteInt32(Items, position, item.ClientId);
				position += sizeof(int);

				// uint dwBlobSize;
				Marshal.WriteInt32(Items, position, 0);
				position += sizeof(int);

				// IntPtr pBlob;
				Marshal.WriteIntPtr(Items, position, IntPtr.Zero);
				position += IntPtr.Size;

				// ushort vtRequestedDataType;
				Marshal.WriteInt16(Items, position, (short)item.Type);
				position += sizeof(short);

				// ushort wReserved;
				Marshal.WriteInt16(Items, position, 0);
				position += sizeof(short);
            }
		}

		~ItemResultReader()
		{
			Dispose(false);
		}

		public IntPtr Items { get; private set; }

		[SecurityPermission(SecurityAction.LinkDemand)] 
        public ItemResult[] Read(IntPtr dataPtr, int[] errors)
        {
	        try
	        {
				var position = 0;
				var result = new ItemResult[errors.Length];
				for (var i = 0; i < errors.Length; i++)
				{
					// ReSharper disable UseObjectOrCollectionInitializer
					result[i] = new ItemResult();
					// ReSharper restore UseObjectOrCollectionInitializer

					// uint hServer;
					result[i].ServerId = Marshal.ReadInt32(dataPtr, position);
					position += sizeof(int);

					// ushort vtCanonicalDataType;
					result[i].CanonicalDataType = Marshal.ReadInt16(dataPtr, position);
					position += sizeof(short);

					// ushort wReserved;
					position += sizeof(short);

					// uint dwAccessRights;
					result[i].AccessRights = Marshal.ReadInt32(dataPtr, position);
					position += sizeof(int);

					// uint dwBlobSize;
					position += sizeof(int);

					// IntPtr pBlob;
					var blob = Marshal.ReadIntPtr(dataPtr, position);
					position += IntPtr.Size;
					if(blob != IntPtr.Zero)
						Marshal.FreeCoTaskMem(blob);

					result[i].Error = errors[i];
				}

				return result;
	        }
	        finally
	        {
				Marshal.FreeCoTaskMem(dataPtr);
	        }
        }

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{			
            if (Items != IntPtr.Zero)
                Marshal.FreeCoTaskMem(Items);
            Items = IntPtr.Zero;

            if (stringsToClear != null)
                foreach (var @string in stringsToClear)
                    Marshal.FreeCoTaskMem(@string);
            stringsToClear = null;
		}

		private List<IntPtr> stringsToClear;

		private static readonly int ItemSize = 
			IntPtr.Size + 
			IntPtr.Size + 
			sizeof(int) + 
			sizeof(int) + 
			sizeof(int) + 
			IntPtr.Size + 
			sizeof(short) + 
			sizeof(short);
	}
}
