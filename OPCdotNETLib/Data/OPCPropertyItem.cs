using System;
using System.Diagnostics;
using OPC.Common;

namespace OPC.Data
{
    /// <summary>
    /// OPC Item Property Item, <see cref="OpcServer.LookupItemIDs(string, int[])"/>
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcPropertyItem 
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
        /// Item Property new ID, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public string NewItemID { get; internal set; }

        internal string DebuggerDisplay
        {
            get
            {
                if (Error == HRESULTS.S_OK)
                    return String.Format("ID:{0} newID:{1}", PropertyID, NewItemID);
                else
                    return String.Format("ID:{0} Error:{1}", PropertyID, Error);
            }
        }
    }
}
