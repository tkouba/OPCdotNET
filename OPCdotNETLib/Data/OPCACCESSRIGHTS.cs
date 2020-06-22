using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC items access rights
    /// </summary>
    [Flags]
    public enum OPCACCESSRIGHTS
    {
        /// <summary>
        /// Unknown
        /// </summary>
        OPC_UNKNOWN = 0,
        /// <summary>
        /// Readable
        /// </summary>
        OPC_READABLE = 1,
        /// <summary>
        /// Writeable
        /// </summary>
        OPC_WRITEABLE = 2
    }
}
