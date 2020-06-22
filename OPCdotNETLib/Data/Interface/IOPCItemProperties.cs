using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Item Properties
    /// </summary>
    [ComImport]
    [Guid("39c13a72-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCItemProperties
    {
        void QueryAvailableProperties([In, MarshalAs(UnmanagedType.LPWStr)] string szItemID, [Out] out int dwCount,
            [Out] out IntPtr ppPropertyIDs, [Out] out IntPtr ppDescriptions, [Out] out IntPtr ppvtDataTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="szItemID"></param>
        /// <param name="dwCount"></param>
        /// <param name="pdwPropertyIDs"></param>
        /// <param name="ppvData"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int GetItemProperties([In, MarshalAs(UnmanagedType.LPWStr)] string szItemID, [In] int dwCount,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] pdwPropertyIDs, [Out] out IntPtr ppvData,
            [Out] out IntPtr ppErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="szItemID"></param>
        /// <param name="dwCount"></param>
        /// <param name="pdwPropertyIDs"></param>
        /// <param name="ppszNewItemIDs"></param>
        /// <param name="ppErrors"></param>
        /// <returns></returns>
        /// <remarks>
        /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
        /// </remarks>
        [PreserveSig]
        int LookupItemIDs([In, MarshalAs(UnmanagedType.LPWStr)]string szItemID, [In]int dwCount,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] int[] pdwPropertyIDs,
            [Out] out IntPtr ppszNewItemIDs, [Out]out IntPtr ppErrors);
    }
}
