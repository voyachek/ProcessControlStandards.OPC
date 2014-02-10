#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	[Guid("55C382C8-21C7-4E88-96C1-BECFB1E3F483"), ComImport, InterfaceType((short)1)]
	interface IOPCEnumGUID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Next(
			[In] uint celt, 
			[Out] out Guid gelt, 
			[Out] out uint celtFetched);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Skip([In] uint celt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Reset();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IOPCEnumGUID @enum);
	}
}
