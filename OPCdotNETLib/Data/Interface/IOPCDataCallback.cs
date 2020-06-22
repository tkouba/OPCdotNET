using System;
using System.Runtime.InteropServices;


namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Data Async Callback
    /// </summary>
    [ComVisible(true)]
    [ComImport]
    [Guid("39c13a70-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCDataCallback
    {
        void OnDataChange([In] int dwTransid, [In] int hGroup, [In] int hrMasterquality,
            [In] int hrMastererror, [In] int dwCount, [In] IntPtr phClientItems, [In] IntPtr pvValues,
            [In] IntPtr pwQualities, [In] IntPtr pftTimeStamps, [In] IntPtr ppErrors);

        void OnReadComplete([In] int dwTransid, [In] int hGroup, [In] int hrMasterquality,
            [In] int hrMastererror, [In] int dwCount, [In] IntPtr phClientItems,
            [In] IntPtr pvValues, [In] IntPtr pwQualities, [In] IntPtr pftTimeStamps,
            [In] IntPtr ppErrors);

        void OnWriteComplete([In] int dwTransid, [In] int hGroup, [In] int hrMastererr,
            [In] int dwCount, [In] IntPtr pClienthandles, [In] IntPtr ppErrors);

        void OnCancelComplete([In] int dwTransid, [In] int hGroup);
    }
}
