using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Server
    /// </summary>
    /// <remarks>
    /// An interface declared with ComImport can expose HRESULTs to C#, this is done by [PreserveSig]
    /// </remarks>
    [ComImport]
    [Guid("39c13a4d-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCServer
    {
        void AddGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In, MarshalAs(UnmanagedType.Bool)] bool bActive,
            [In] int dwRequestedUpdateRate, [In] int hClientGroup,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] int[] pTimeBias,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] float[] pPercentDeadband,
            [In] int dwLCID, [Out] out int phServerGroup, [Out] out int pRevisedUpdateRate,
            [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

        void GetErrorString([In] int dwError, [In] int dwLocale,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppString);

        void GetGroupByName([In, MarshalAs(UnmanagedType.LPWStr)] string szName, [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

        void GetStatus([Out, MarshalAs(UnmanagedType.LPStruct)]out SERVERSTATUS ppServerStatus);

        void RemoveGroup([In] int hServerGroup, [In, MarshalAs(UnmanagedType.Bool)] bool bForce);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dwScope"></param>
        /// <param name="riid"></param>
        /// <param name="ppUnk"></param>
        /// <returns>may return S_FALSE</returns>
        [PreserveSig]
        int CreateGroupEnumerator([In] OPCENUMSCOPE dwScope, [In] ref Guid riid,
            [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

    }
}
