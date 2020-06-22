using System;
using System.Runtime.InteropServices;

namespace OPC.Data
{
    /// <summary>
    /// Internal OPC Item Definition
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    internal class InternalOPCITEMDEF
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szAccessPath;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string szItemID;

        [MarshalAs(UnmanagedType.Bool)]
        public bool bActive;

        public int hClient;
        public int dwBlobSize;
        public IntPtr pBlob;

        public short vtRequestedDataType;

        public short wReserved;
    };
}
