#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
	[Guid("39C13A52-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCSyncIO
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Read(
			[In] DataSource source,
			[In] uint count, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds, 
			[Out] out IntPtr values, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out int[] errors);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Write(
			[In] uint count, 
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds, 
			[In] IntPtr values, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out int[] errors);
	}
}
