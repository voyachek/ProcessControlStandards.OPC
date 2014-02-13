#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.Core
{
	[Guid("0002E000-0000-0000-C000-000000000046"), ComImport, InterfaceType((short)1)]
	interface IEnumGUID
	{
        [PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Next(
			[In] uint celt,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] rgelt,
			[Out] out uint celtFetched);

        [PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Skip([In] uint celt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Reset();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IEnumGUID @enum);
	}
}
