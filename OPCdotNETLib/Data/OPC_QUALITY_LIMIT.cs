using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC Quality Limit flags (------LL)
    /// </summary>
    [Flags]
    public enum OPC_QUALITY_LIMIT
    {
        /// <summary>
        /// OPC Quality limit OK
        /// </summary>
        LIMIT_OK = 0x0000,
        /// <summary>
        /// OPC Quality limit LOW
        /// </summary>
        LIMIT_LOW = 0x0001,
        /// <summary>
        /// OPC Quality limit HIGH
        /// </summary>
        LIMIT_HIGH = 0x0002,
        /// <summary>
        /// OPC Quality limit CONST
        /// </summary>
        LIMIT_CONST = 0x0003
    }
}
