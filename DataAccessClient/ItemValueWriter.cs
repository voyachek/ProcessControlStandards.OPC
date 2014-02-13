#region using

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandards.OPC.Core;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
	class ItemValueWriter : IDisposable
	{
		[SecurityPermission(SecurityAction.LinkDemand)] 
		public ItemValueWriter(ICollection<object> values)
		{
            Values = Marshal.AllocCoTaskMem(NativeMethods.VariantSize * values.Count);
			variantsToClear = new List<IntPtr>(values.Count);

            var position = 0;
            var valuesPtrAsLong = Values.ToInt64();
            foreach (var value in values)
            {
                var variant = new IntPtr(valuesPtrAsLong + position);
	            variantsToClear.Add(variant);
	            Marshal.GetNativeVariantForObject(value, variant);
	            
				position += NativeMethods.VariantSize;
            }
		}

		~ItemValueWriter()
		{
			Dispose(false);
		}

		public IntPtr Values { get; private set; }

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{			
            if (Values != IntPtr.Zero)
                Marshal.FreeCoTaskMem(Values);
            Values = IntPtr.Zero;

            if (variantsToClear != null)
                foreach (var variant in variantsToClear)
                    NativeMethods.VariantClear(variant);
            variantsToClear = null;
		}

		private List<IntPtr> variantsToClear;
	}
}
