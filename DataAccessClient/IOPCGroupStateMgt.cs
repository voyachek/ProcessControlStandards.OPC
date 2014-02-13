#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
	[Guid("39C13A50-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCGroupStateMgt
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetState(
			[Out] out int updateRate, 
			[Out] out int active, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string name, 
			[Out] out int timeBias, 
			[Out] out float percentDeadband, 
			[Out] out int locale, 
			[Out] out int clientId, 
			[Out] out int serverId);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetState(
			[In] ref int requestedUpdateRate, 
			[Out] out int revisedUpdateRate, 
			[In] ref int active, 
			[In] ref int timeBias, 
			[In] ref float percentDeadband, 
			[In] ref int locale, 
			[In] ref int clientId);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetName([In, MarshalAs(UnmanagedType.LPWStr)] string name);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CloneGroup(
			[In, MarshalAs(UnmanagedType.LPWStr)] string name, 
			[In] ref Guid interfaceId, 
			[MarshalAs(UnmanagedType.IUnknown)] out object group);
	}
}
