using System;

namespace OPC.Data
{
    /// <summary>
    /// Selection of the read source
    /// </summary>
    public enum OPCDATASOURCE
    {
        /// <summary>
        /// Read data from OPC server cache
        /// </summary>
        OPC_DS_CACHE = 1,
        /// <summary>
        /// Read data from device
        /// </summary>
        OPC_DS_DEVICE = 2
    }
}	
