using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Server public groups
    /// </summary>
    [ComImport]
    [Guid("39c13a4e-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCServerPublicGroups
    {
        void GetPublicGroupByName([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riID,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

        void RemovePublicGroup([In] int hServerGroup, [In, MarshalAs(UnmanagedType.Bool)] bool bForce);

    }
}
