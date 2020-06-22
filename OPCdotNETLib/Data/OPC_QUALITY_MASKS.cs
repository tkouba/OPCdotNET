using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC Quality mask flags (QQSSSSLL)
    /// </summary>
    [Flags]
    public enum OPC_QUALITY_MASKS : short
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        LIMIT_MASK = 0x0003,
        STATUS_MASK = 0x00FC,
        MASTER_MASK = 0x00C0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
