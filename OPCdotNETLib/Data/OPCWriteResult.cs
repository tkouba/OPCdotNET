using System;
using System.Diagnostics;

namespace OPC.Data
{
    /// <summary>
    /// OPC Write Result
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcWriteResult
    {
        /// <summary>
        /// Handle Client
        /// </summary>
        public int HandleClient { get; set; }
        /// <summary>
        /// Error
        /// </summary>
        public int Error { get; set; }

        internal string DebuggerDisplay
        {
            get { return String.Format("Handle:{0} Error:{1}", HandleClient, Error); }
        }
    }
}
