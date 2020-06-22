using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Public Group State
    /// </summary>
    [ComVisible(true),
    Guid("39c13a51-011e-11d0-9675-0020afd8adb3"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCPublicGroupStateMgt
    {
        void GetState([Out, MarshalAs(UnmanagedType.Bool)] out bool pPublic);

        void MoveToPublic();
    }
}
