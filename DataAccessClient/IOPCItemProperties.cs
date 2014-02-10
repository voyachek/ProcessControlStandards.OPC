#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	[Guid("39C13A72-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCItemProperties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void QueryAvailableProperties(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[Out] out uint count, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] out int[] propertyIds, 
			[Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] out string[] descriptions, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] out VarEnum[] dataTypes);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItemProperties(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[In] uint count, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] propertyIds,
			[Out] out IntPtr data, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] out int[] errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void LookupItemIDs(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[In] uint count, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[]  propertyIds, 
			[Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 1)] out string[] newItemIds, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] out int[] errors);
	}
}
