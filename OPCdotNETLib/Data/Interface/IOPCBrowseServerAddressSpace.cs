
/*
Notes:
	An interface declared with ComImport can expose HRESULTs to C#,
	this is done by [PreserveSig]

	midl attribute 'pointer_unique' is simulated by passing an array[1]

*/

using System;
using System.Runtime.InteropServices;


namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Server Address Space Browsing
    /// </summary>
    [ComImport]
    [Guid("39c13a4f-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCBrowseServerAddressSpace
    {
        void QueryOrganization([Out, MarshalAs(UnmanagedType.U4)] out OPCNAMESPACETYPE pNameSpaceType);

        void ChangeBrowsePosition([In, MarshalAs(UnmanagedType.U4)] OPCBROWSEDIRECTION dwBrowseDirection,
            [In, MarshalAs(UnmanagedType.LPWStr)] string szName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwBrowseFilterType"></param>
        /// <param name="szFilterCriteria"></param>
        /// <param name="vtDataTypeFilter"></param>
        /// <param name="dwAccessRightsFilter"></param>
        /// <param name="ppUnk"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int BrowseOPCItemIDs([In, MarshalAs(UnmanagedType.U4)] OPCBROWSETYPE dwBrowseFilterType,
            [In, MarshalAs(UnmanagedType.LPWStr)] string szFilterCriteria,
            [In, MarshalAs(UnmanagedType.U2)] short vtDataTypeFilter,
            [In, MarshalAs(UnmanagedType.U4)] OPCACCESSRIGHTS dwAccessRightsFilter,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

        void GetItemID([In, MarshalAs(UnmanagedType.LPWStr)] string szItemDataID,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string szItemID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="szItemID"></param>
        /// <param name="ppUnk"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int BrowseAccessPaths([In, MarshalAs(UnmanagedType.LPWStr)] string szItemID,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);
    }
}	// namespace OPC.Data.Interface
