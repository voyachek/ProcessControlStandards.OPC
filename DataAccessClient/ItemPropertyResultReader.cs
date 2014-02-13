using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using ProcessControlStandards.OPC.Core;

namespace ProcessControlStandards.OPC.DataAccessClient
{
    class ItemPropertyResultReader
    {
        public static ItemProperty[] ReadItemProperties(uint size, IntPtr idsPtr, IntPtr descriptionsPtr, IntPtr typesPtr)
        {
            var result = new ItemProperty[size];
            try
            {
                for (var i = 0; i < size; i++)
                {
                    result[i].Id = Marshal.ReadInt32(idsPtr, i * sizeof(int));
                    var descriptionPtr = Marshal.ReadIntPtr(descriptionsPtr, i * IntPtr.Size);
                    if (descriptionPtr != IntPtr.Zero)
                    {
                        result[i].Description = Marshal.PtrToStringUni(descriptionPtr);
                        Marshal.FreeCoTaskMem(descriptionPtr);
                    }
                    result[i].Type = (VarEnum)Marshal.ReadInt16(typesPtr, i * sizeof(short));
                }

                return result;
            }
            finally
            {
                if (idsPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(idsPtr);
                if (descriptionsPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(descriptionsPtr);
                if (typesPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(typesPtr);
            }
        }

        public static ItemPropertyValue[] ReadItemPropertyValues(int size, IntPtr dataPtr, IntPtr errorsPtr)
        {
            try
            {
                var results = new ItemPropertyValue[size];
                var dataPtrAsLong = dataPtr.ToInt64();
                for (var i = 0; i < size; i++)
                {
                    var valuePtr = new IntPtr(dataPtrAsLong + i * NativeMethods.VariantSize);
                    if (valuePtr != IntPtr.Zero)
                    {
                        results[i].Value = Marshal.GetObjectForNativeVariant(valuePtr);
                        NativeMethods.VariantClear(valuePtr);
                    }
                    results[i].Error = Marshal.ReadInt32(errorsPtr, i * sizeof(int));
                }

                return results;
            }
            finally
            {
                if (dataPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(dataPtr);
                if (errorsPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(errorsPtr);
            }            
        }

        [SecurityPermission(SecurityAction.LinkDemand)] 
        public static ItemPropertyId[] ReadItemPropertyIds(int size, IntPtr dataPtr, IntPtr errorsPtr)
        {
            try
            {
                var results = new ItemPropertyId[size];
                for (var i = 0; i < size; i++)
                {
                    var idPtr = Marshal.ReadIntPtr(dataPtr, i * IntPtr.Size);
                    results[i].Id = Marshal.PtrToStringUni(idPtr);
                    results[i].Error = Marshal.ReadInt32(errorsPtr, i * sizeof(int));

                    Marshal.FreeCoTaskMem(idPtr);
                }

                return results;
            }
            finally
            {
                if (dataPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(dataPtr);
                if (errorsPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(errorsPtr);
            }        
        }
    }
}
