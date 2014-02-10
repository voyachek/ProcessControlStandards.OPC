#region using

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandarts.OPC.Core;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	class ItemValueReader : IDisposable
	{
		public ItemValueReader(int[] clientIds, IntPtr values, short[] qualities, long[] timeStamps, int[] errors)
		{
			this.values = values;
			variantsToClear = new List<IntPtr>(clientIds.Length);
            Values = new ItemValue[qualities.Length];

            var position = 0;
			var dataPtrAsLong = values.ToInt64();
            for (var i = 0; i < clientIds.Length; i++)
            {
	            var variant = new IntPtr(dataPtrAsLong + position);
				variantsToClear.Add(variant);
				position += NativeMethods.VariantSize;

	            Values[i] = new ItemValue
	            {
		            ClientId = clientIds[i],
		            Timestamp = DateTime.FromFileTimeUtc(timeStamps[i]),
		            Quality = qualities[i],
		            Value = Marshal.GetObjectForNativeVariant(variant),
		            Error = errors[i],
	            };
            }
		}

		~ItemValueReader()
		{
			Dispose(false);
		}

		public ItemValue[] Values { get; private set; }

		[SecurityPermission(SecurityAction.LinkDemand)] 
		public static ItemValue[] Read(IntPtr dataPtr, int[] errors)
        {
	        try
	        {
		        var position = 0;
		        var dataPtrAsLong = dataPtr.ToInt64();
		        var result = new ItemValue[errors.Length];
		        for (var i = 0; i < errors.Length; i++)
		        {
			        // ReSharper disable UseObjectOrCollectionInitializer
			        result[i] = new ItemValue();
			        // ReSharper restore UseObjectOrCollectionInitializer

			        // uint hClient;
			        result[i].ClientId = Marshal.ReadInt32(dataPtr, position);
			        position += sizeof (int);

			        // FILETIME ftTimeStamp;
			        long time = Marshal.ReadInt64(dataPtr, position);
			        result[i].Timestamp = DateTime.FromFileTimeUtc(time);
			        position += sizeof (long);

			        // ushort wQuality;
			        result[i].Quality =
				        Marshal.ReadInt16(dataPtr, position);
			        position += sizeof (short);

			        // ushort wReserved;
			        position += sizeof (short);

			        // VARIANT vDataValue;
			        var variant = new IntPtr(dataPtrAsLong + position);
			        result[i].Value = Marshal.GetObjectForNativeVariant(variant);
					NativeMethods.VariantClear(variant);
			        position += NativeMethods.VariantSize;

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
            if (values != IntPtr.Zero)
                Marshal.FreeCoTaskMem(values);
            values = IntPtr.Zero;

            if (variantsToClear != null)
                foreach (var variant in variantsToClear)
                    NativeMethods.VariantClear(variant);
            variantsToClear = null;
		}

		private IntPtr values;

		private List<IntPtr> variantsToClear;
	}
}
