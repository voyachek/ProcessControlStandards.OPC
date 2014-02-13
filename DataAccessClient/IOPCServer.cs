#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
	[Guid("39C13A4D-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCServer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddGroup(
			[In, MarshalAs(UnmanagedType.LPWStr)] string name, 
			[In] int active, 
			[In] int requestedUpdateRate, 
			[In] int clientGroup, 
			[In] ref int timeBias, 
			[In] ref float percentDeadband, 
			[In] uint lcid,
			[Out] out int serverGroup, 
			[Out] out int revisedUpdateRate, 
			[In] ref Guid iid,
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object group);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetErrorString(
			[In, MarshalAs(UnmanagedType.Error)] int error, 
			[In] uint locale, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string @string);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetGroupByName(
			[In, MarshalAs(UnmanagedType.LPWStr)] string name, 
			[In] ref Guid iid, 
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object group);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetStatus([Out] out IntPtr serverStatus);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveGroup([In] int serverGroup, [In] int force);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CreateGroupEnumerator(
			[In] EnumScope scope, 
			[In] ref Guid iid, 
			[Out, MarshalAs(UnmanagedType.IUnknown)] out object enumerator);

	}
}
