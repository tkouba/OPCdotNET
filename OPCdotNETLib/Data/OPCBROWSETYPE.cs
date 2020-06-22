using System;

namespace OPC.Data
{
    /// <summary>
    /// Browse mode selection
    /// </summary>
    public enum OPCBROWSETYPE
    {
        /// <summary>
        /// Browse items that have children.
        /// </summary>
        OPC_BRANCH = 1,
        /// <summary>
        /// Browse items that don't have children.
        /// </summary>
        OPC_LEAF = 2,
        /// <summary>
        /// Browse everything at and below this level including all children of children. Same as adress space is <see cref="OPCNAMESPACETYPE.OPC_NS_FLAT"/>.
        /// </summary>
        OPC_FLAT = 3
    }
}
