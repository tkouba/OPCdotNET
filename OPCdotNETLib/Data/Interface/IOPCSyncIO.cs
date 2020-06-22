using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Sync IO
    /// </summary>
    [ComImport]
    [Guid("39c13a52-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCSyncIO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwSource"></param>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="ppItemValues"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int Read([In, MarshalAs(UnmanagedType.U4)] OPCDATASOURCE dwSource, [In] int dwCount,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] phServer, [Out] out IntPtr ppItemValues,
            [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="pItemValues"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int Write([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] object[] pItemValues, [Out] out IntPtr ppErrors);

    }
}
