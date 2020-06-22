using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Shutdown callback interface
    /// </summary>
    [ComImport]
    [Guid("F31DFDE1-07B6-11d2-B2D8-0060083BA1FB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCShutdown
    {
        void ShutdownRequest([In, MarshalAs(UnmanagedType.LPWStr)] string szReason);
    }
}
