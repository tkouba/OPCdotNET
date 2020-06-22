using System;
using System.Runtime.InteropServices;

namespace OPC.Common.Interface
{
    /// <summary>
    /// OPC Server List Interface
    /// </summary>
    [ComVisible(true), ComImport,
    Guid("13486D50-4821-11D2-A494-3CB306C10000"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCServerList
    {
        void EnumClassesOfCategories(
            [In]                                            int cImplemented,	// WARNING ONLY 1!!
            [In]                                        ref Guid catidImpl,		// WARNING ONLY 1!!
            [In]                                            int cRequired,		// WARNING ONLY 1!!
            [In]                                        ref Guid catidReq,		// WARNING ONLY 1!!
            [Out, MarshalAs(UnmanagedType.IUnknown)]    out object ppUnk);

        void GetClassDetails(
            [In]                                        ref Guid clsid,
            [Out, MarshalAs(UnmanagedType.LPWStr)]      out string ppszProgID,
            [Out, MarshalAs(UnmanagedType.LPWStr)]      out string ppszUserType);

        void CLSIDFromProgID(
            [In, MarshalAs(UnmanagedType.LPWStr)]           string szProgId,
            [Out]                                       out Guid clsid);
    }
}
