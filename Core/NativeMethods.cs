#region using

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	static class NativeMethods
	{
		public const int VariantSize = 0x10;

		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("oleaut32.dll", PreserveSig = false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static extern void VariantClear(IntPtr variant);
	}
}
