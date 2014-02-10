#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
    [Guid("39c13a70-011e-11d0-9675-0020afd8adb3"), ComImport, InterfaceType((short)1)]
    interface IOPCDataCallback
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnDataChange(
            [In] int transactionId,
            [In] int groupId,
            [In] int quality,
            [In] int error,
            [In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] clientIds,
            [In] IntPtr values,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] short[] qualities,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] long[] timeStamps,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] errors);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnReadComplete(
            [In] int transactionId,
            [In] int groupId,
            [In] int quality,
            [In] int error,
            [In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] clientIds,
            [In] IntPtr values,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] short[] qualities,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] long[] timeStamps,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] int[] errors);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnWriteComplete(
            [In] int transactionId,
            [In] int groupId,
            [In] int error,
            [In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] clientIds,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] int[] errors);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnCancelComplete(
            [In] int transactionId,
            [In] int groupId);
	}
}
