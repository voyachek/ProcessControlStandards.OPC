#region using

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ProcessControlStandards.OPC.Core;

#endregion

namespace ProcessControlStandards.OPC.DataAccessClient
{
	[Guid("39C13A4F-011E-11D0-9675-0020AFD8ADB3"), ComImport, InterfaceType((short)1)]
	interface IOPCBrowseServerAddressSpace
	{
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void QueryOrganization([Out] out NamespaceType nameSpaceType);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void ChangeBrowsePosition(
			[In] BrowseDirection browseDirection, 
			[In, MarshalAs(UnmanagedType.LPWStr)] string @string);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void BrowseOPCItemIDs(
			[In] BrowseType browseFilterType, 
			[In, MarshalAs(UnmanagedType.LPWStr)] string filterCriteria, 
			[In] short vtDataTypeFilter, 
			[In] int dwAccessRightsFilter,
            [Out, MarshalAs(UnmanagedType.Interface)] out IEnumString ppIEnumString);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void GetItemID(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemDataId, 
			[Out, MarshalAs(UnmanagedType.LPWStr)] out string itemId);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void BrowseAccessPaths(
			[In, MarshalAs(UnmanagedType.LPWStr)] string itemId, 
			[Out, MarshalAs(UnmanagedType.Interface)] out IEnumString enumString);
	}
}
