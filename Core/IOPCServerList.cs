#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	[Guid("13486D50-4821-11D2-A494-3CB306C10000"), ComImport, InterfaceType((short)1)]
	interface IOPCServerList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void EnumClassesOfCategories(
			[In] uint implemented, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] catidImpl, 
			[In] uint required, 
			[In] ref Guid catidReq, 
			[Out, MarshalAs(UnmanagedType.Interface)] out IEnumGUID enumGuid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetClassDetails(
			[In] ref Guid clsid, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string progId, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string userType);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CLSIDFromProgID(
			[In, MarshalAs(UnmanagedType.LPWStr)] string progId, 
			[Out] out Guid clsid);
	}
}
