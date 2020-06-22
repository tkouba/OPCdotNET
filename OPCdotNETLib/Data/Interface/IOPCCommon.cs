using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Common interface
    /// </summary>
    [ComImport]
    [Guid("F31DFDE2-07B6-11d2-B2D8-0060083BA1FB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCCommon
    {
        void SetLocaleID([In] int dwLcid);

        void GetLocaleID([Out] out int pdwLcid);

        [PreserveSig]
        int QueryAvailableLocaleIDs([Out] out int pdwCount, [Out] out IntPtr pdwLcid);

        void GetErrorString([In] int dwError, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppString);

        void SetClientName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);

    }
}
