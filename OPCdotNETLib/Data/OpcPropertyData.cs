
using System;
using System.Diagnostics;
using OPC.Common;

namespace OPC.Data
{
    /// <summary>
    /// OPC Item Property Value, <see cref="OpcServer.GetItemProperties(string, int[])"/>
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcPropertyData
    {
        /// <summary>
        /// Item Property ID
        /// </summary>
        public int PropertyID { get; internal set; }
        /// <summary>
        /// Error reading value
        /// </summary>
        public int Error { get; internal set; }
        /// <summary>
        /// Item Property Value, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public object Data { get; internal set; }

        internal string DebuggerDisplay
        {
            get
            {
                if (Error == HRESULTS.S_OK)
                    return String.Format("ID:{0} Data:{1}", PropertyID, Data);
                else
                    return String.Format("ID:{0} Error:{1}", PropertyID, Error);
            }
        }
    }
}
