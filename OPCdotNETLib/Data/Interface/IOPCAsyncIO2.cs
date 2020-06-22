using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Async IO 2
    /// </summary>
    [ComVisible(true)]
    [ComImport]
    [Guid("39c13a71-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCAsyncIO2
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="dwTransactionID"></param>
        /// <param name="pdwCancelID"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int Read([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [In] int dwTransactionID, [Out] out int pdwCancelID, [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="pItemValues"></param>
        /// <param name="dwTransactionID"></param>
        /// <param name="pdwCancelID"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int Write([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] object[] pItemValues, [In] int dwTransactionID,
            [Out] out int pdwCancelID, [Out] out IntPtr ppErrors);

        void Refresh2([In, MarshalAs(UnmanagedType.U4)] OPCDATASOURCE dwSource, [In] int dwTransactionID,
            [Out] out int pdwCancelID);

        void Cancel2([In] int dwCancelID);

        void SetEnable([In, MarshalAs(UnmanagedType.Bool)] bool bEnable);

        void GetEnable([Out, MarshalAs(UnmanagedType.Bool)] out bool pbEnable);

    }
}
