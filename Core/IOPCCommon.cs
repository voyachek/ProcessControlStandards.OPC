#region using

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.Core
{
	[Guid("F31DFDE2-07B6-11D2-B2D8-0060083BA1FB"), ComImport, InterfaceType((short)1)]
	public interface IOPCCommon
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetLocaleID([In] int lcid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetLocaleID([Out] out int lcid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void QueryAvailableLocaleIDs(
			[Out] out uint count, 
			[Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out int[] lcid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetErrorString(
			[In] int error, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string @string);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetClientName([In] string name);
	}
}
