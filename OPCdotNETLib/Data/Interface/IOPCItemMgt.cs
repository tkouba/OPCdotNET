using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Item Mgmt
    /// </summary>
    [ComVisible(true), ComImport,
    Guid("39c13a54-011e-11d0-9675-0020afd8adb3"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCItemMgt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="pItemArray"></param>
        /// <param name="ppAddResults"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int AddItems([In] int dwCount, [In] IntPtr pItemArray, [Out] out IntPtr ppAddResults, [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="pItemArray"></param>
        /// <param name="bBlobUpdate"></param>
        /// <param name="ppValidationResults"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int ValidateItems([In] int dwCount, [In] IntPtr pItemArray, [In, MarshalAs(UnmanagedType.Bool)] bool bBlobUpdate,
            [Out] out IntPtr ppValidationResults, [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int RemoveItems([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="bActive"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int SetActiveState([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [In, MarshalAs(UnmanagedType.Bool)] bool bActive, [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="phClient"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int SetClientHandles([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phClient, [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwCount"></param>
        /// <param name="phServer"></param>
        /// <param name="pRequestedDatatypes"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int SetDataTypes([In] int dwCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] int[] phServer,
            [In] IntPtr pRequestedDatatypes, [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="riID"></param>
        /// <param name="ppUnk"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int CreateEnumerator([In] ref Guid riID, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

    }
}
