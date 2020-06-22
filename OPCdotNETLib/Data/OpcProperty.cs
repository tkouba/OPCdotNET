using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OPC.Common;

namespace OPC.Data
{
    /// <summary>
    /// OPC Item Property Definition, <see cref="OpcServer.QueryAvailableProperties(string)"/>
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcProperty                    
    {
        /// <summary>
        /// Item Property ID
        /// </summary>
        public int PropertyID { get; internal set; }
        /// <summary>
        /// Item Property Description
        /// </summary>
        public string Description { get; internal set; }
        /// <summary>
        /// Item Property Data Type
        /// </summary>
        public VarEnum DataType { get; internal set; }

        internal string DebuggerDisplay
        {
            get { return String.Format("ID:{0} '{1}' T:{2}", PropertyID, Description, Extensions.VarEnumToString(DataType)); }
        }
    }
}
