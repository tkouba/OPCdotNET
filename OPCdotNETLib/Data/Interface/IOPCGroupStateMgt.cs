using System;
using System.Runtime.InteropServices;

namespace OPC.Data.Interface
{
    /// <summary>
    /// OPC Group State 
    /// </summary>
	[Guid("39c13a50-011e-11d0-9675-0020afd8adb3")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IOPCGroupStateMgt
    {
        void GetState([Out] out int pUpdateRate, [Out, MarshalAs(UnmanagedType.Bool)] out bool pActive,
            [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppName, [Out] out int pTimeBias,
            [Out] out float pPercentDeadband, [Out] out int pLCID, [Out] out int phClientGroup,
            [Out] out int phServerGroup);

        void SetState([In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] int[] pRequestedUpdateRate,
            [Out] out int pRevisedUpdateRate,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Bool, SizeConst = 1)] bool[] pActive,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] int[] pTimeBias,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] float[] pPercentDeadband,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] int[] pLCID,
            [In, MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] int[] phClientGroup);

        void SetName([In, MarshalAs(UnmanagedType.LPWStr)] string szName);

        void CloneGroup([In, MarshalAs(UnmanagedType.LPWStr)] string szName,
            [In] ref Guid riID, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppUnk);

    }
}
