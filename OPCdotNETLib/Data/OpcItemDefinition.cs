using System;
using System.Runtime.InteropServices;

namespace OPC.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    public class OpcItemDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccessPath { get; set; }
        /// <summary>
        /// OPC Item ID
        /// </summary>
        public string ItemID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// OPC Item client handle
        /// </summary>
        public int HandleClient { get; set; }
        /// <summary>
        /// OPC Item blob
        /// </summary>
        public byte[] Blob { get; set; }
        /// <summary>
        /// OPC Item requested data type or <see cref="VarEnum.VT_EMPTY"/>
        /// </summary>
        public VarEnum RequestedDataType { get; set; }

        /// <summary>
        /// OPC Item Definition
        /// </summary>
        public OpcItemDefinition()
        {
            AccessPath = String.Empty;
            Blob = null;
        }

        /// <summary>
        /// OPC Item Definition
        /// </summary>
        /// <param name="itemID">Item ID</param>
        /// <param name="active"></param>
        /// <param name="handleClient">client handle</param>
        /// <param name="requestedDataType">requested data type or <see cref="VarEnum.VT_EMPTY"/></param>
        public OpcItemDefinition(string itemID, bool active, int handleClient, VarEnum requestedDataType) : this()
        {
            ItemID = itemID;
            Active = active;
            HandleClient = handleClient;
            RequestedDataType = requestedDataType;
        }
    };
}
