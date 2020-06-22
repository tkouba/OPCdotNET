using System;
using System.Runtime.InteropServices;

namespace OPC.Data
{
    
    /// <summary>
    /// OPC Server Status 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Unicode)]
    public class SERVERSTATUS
    {
        /// <summary>
        /// OPC Server start time
        /// </summary>
        public long ftStartTime;
        /// <summary>
        /// OPC Server current time
        /// </summary>
        public long ftCurrentTime;
        /// <summary>
        /// OPC Server last update time
        /// </summary>
        public long ftLastUpdateTime;

        /// <summary>
        /// OPC Server <see cref="OPCSERVERSTATE"/>
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public OPCSERVERSTATE eServerState;

        /// <summary>
        /// OPC Server group count
        /// </summary>
        public int dwGroupCount;
        /// <summary>
        /// OPC Server bandwidth (server specific value)
        /// </summary>
        public int dwBandWidth;
        /// <summary>
        /// OPC Server major version
        /// </summary>
        public short wMajorVersion;
        /// <summary>
        /// OPC Server minor version
        /// </summary>
        public short wMinorVersion;
        /// <summary>
        /// OPC Server build number
        /// </summary>
        public short wBuildNumber;
        /// <summary>
        /// Reserved
        /// </summary>
        public short wReserved;

        /// <summary>
        /// OPC Server vendor info
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string szVendorInfo;
    };
}
