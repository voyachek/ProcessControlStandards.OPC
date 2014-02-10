#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	[Guid("9DD0B56C-AD9E-43EE-8305-487F3188BF7A"), ComImport, InterfaceType((short)1)]
	interface IOPCServerList2
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void EnumClassesOfCategories(
			[In] uint implemented, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] Guid[] catidImpl, 
			[In] uint required, 
			[In] ref Guid catidReq, 
			[Out, MarshalAs(UnmanagedType.Interface)] out IOPCEnumGUID enumGuid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetClassDetails(
			[In] ref Guid clsid, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string progId, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string userType,
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string verIndProgId);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CLSIDFromProgID(
			[In, MarshalAs(UnmanagedType.LPWStr)] string progId, 
			[Out] out Guid clsid);
	}
}
