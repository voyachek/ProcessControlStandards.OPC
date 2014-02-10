#region using

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandarts.OPC.DataAccessClient
{
    [Guid("39c13a71-011e-11d0-9675-0020afd8adb3"), ComImport, InterfaceType((short)1)]
    interface IOPCAsyncIO2
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Read(
            [In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds,
            [In] int transactionId,
            [Out] out int cancelId,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out int[] errors);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Write(
            [In] uint count,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] serverIds,
            [In] IntPtr values,
            [In] int transactionId,
            [Out] out int cancelId,
            [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] out int[] errors);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Refresh2(
            [In] DataSource source,
            [In] int transactionId,
            [Out] out int cancelId);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void Cancel2([In] int cancelId);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetEnable([In] int enable);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetEnable([Out] out int enable);
    }
}
