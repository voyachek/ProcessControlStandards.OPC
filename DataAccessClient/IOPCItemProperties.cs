#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
	[Guid("39C13A72-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCItemProperties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void QueryAvailableProperties(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[Out] out uint count, 
			[Out] out IntPtr propertyIds,
            [Out] out IntPtr descriptions,
            [Out] out IntPtr dataTypes);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItemProperties(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[In] uint count, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] propertyIds,
			[Out] out IntPtr data,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void LookupItemIDs(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[In] uint count, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[]  propertyIds,
            [Out] out IntPtr data,
            [Out] out IntPtr errors);
	}
}
