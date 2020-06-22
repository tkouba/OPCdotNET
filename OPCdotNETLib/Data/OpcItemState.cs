using System;
using System.Diagnostics;
using System.Text;
using OPC.Common;

namespace OPC.Data
{
    /// <summary>
    /// OPC Item read/change state
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcItemState
    {
        /// <summary>
        /// OPC Item Handle Client, always valid for callbacks
        /// </summary>
        public int HandleClient;
        /// <summary>
        /// Error
        /// </summary>
        public int Error { get; set; }
        /// <summary>
        /// OPC Item data value, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public object DataValue { get; set; }
        /// <summary>
        /// OPC Item timestamp, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// OPC Item quality, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public short Quality { get; set; }

        internal string DebuggerDisplay
        {
            get
            {
                if (Error == HRESULTS.S_OK)
                    return String.Format("HandleClient:{0} Error:{1}", HandleClient, Error);
                else
                return String.Format("HandleClient:{0} Value:{1} '{2}' {3}", HandleClient, DataValue, 
                    DateTime.FromFileTime(TimeStamp), Extensions.QualityToString(Quality));
            }
        }
    }
}
