#region using

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace ProcessControlStandards.OPC.Core
{
    /// <summary>
    /// Manages the definition of the IEnumString interface.
    /// </summary>
	[Guid("00000101-0000-0000-C000-000000000046"), ComImport, InterfaceType((short)1)]
	public interface IEnumString
	{
        /// <summary>
        /// Retrieves a specified number of items in the enumeration sequence.
        /// </summary>
        /// <param name="celt">The number of strings to return in rgelt.</param>
        /// <param name="rgelt">When this method returns, contains a reference to the enumerated strings.</param>
        /// <param name="celtFetched">When this method returns, contains a reference to the actual number of strings enumerated in rgelt.</param>
        /// <returns>S_OK if the celtFetched parameter equals the celt parameter; otherwise, S_FALSE.</returns>
        [PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Next(
			[In] uint celt,
            [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] rgelt,
			[Out] out uint celtFetched);

        /// <summary>
        /// Skips a specified number of items in the enumeration sequence.
        /// </summary>
        /// <param name="celt">The number of elements to skip in the enumeration.</param>
        /// <returns>S_OK if the number of elements skipped equals the celt parameter; otherwise, S_FALSE.</returns>
        [PreserveSig]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        int Skip([In] uint celt);

        /// <summary>
        /// Resets the enumeration sequence to the beginning.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Reset();

        /// <summary>
        /// Creates a new enumerator that contains the same enumeration state as the current one.
        /// </summary>
        /// <param name="enum">When this method returns, contains a reference to the newly created enumerator.</param>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Clone([Out, MarshalAs(UnmanagedType.Interface)] out IEnumString @enum);
	}
}
