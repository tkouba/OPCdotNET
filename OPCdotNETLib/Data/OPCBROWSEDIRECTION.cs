using System;

namespace OPC.Data
{
    /// <summary>
    /// OPC Browse direction
    /// </summary>
    public enum OPCBROWSEDIRECTION
    {
        /// <summary>
        /// Direction up, use <see cref="String.Empty"/> to parent.
        /// </summary>
        OPC_BROWSE_UP = 1,
        /// <summary>
        /// Direction down
        /// </summary>
        OPC_BROWSE_DOWN = 2,
        /// <summary>
        /// Direction to, use <see cref="String.Empty"/> to root.
        /// </summary>
        OPC_BROWSE_TO = 3
    }
}
