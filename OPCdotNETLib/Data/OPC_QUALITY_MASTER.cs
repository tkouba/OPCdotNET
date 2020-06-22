using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC Quality master flags
    /// </summary>
    [Flags]
    public enum OPC_QUALITY_MASTER : short
    {
        /// <summary>
        /// Bad [Non-Specific]
        /// </summary>
        QUALITY_BAD = 0x0000,
        /// <summary>
        /// Uncertain [Non-Specific]
        /// </summary>
        QUALITY_UNCERTAIN = 0x0040,
        /// <summary>
        /// Non standard error
        /// </summary>
        ERROR_QUALITY_VALUE = 0x0080,
        /// <summary>
        /// Good [Non-Specific]
        /// </summary>
        QUALITY_GOOD = 0x00C0,
    }
}
