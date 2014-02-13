#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
	[Guid("39C13A54-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCItemMgt
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddItems(
			[In] uint count,
			[In] IntPtr itemArray, 
			[Out] out IntPtr addResults,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ValidateItems(
			[In] uint count, 
			[In] IntPtr itemArray, 
			[In] int blobUpdate, 
			[Out] out IntPtr validationResults,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveItems(
			[In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetActiveState(
			[In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds,
			[In] int active,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetClientHandles(
			[In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] clientIds,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetDatatypes(
			[In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] short[] requestedDatatypes,
            [Out] out IntPtr errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CreateEnumerator(
			[In] ref Guid iid, 
			[MarshalAs(UnmanagedType.IUnknown)] out object @enum);
	}
}
