using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OPC.Common;

namespace OPC.Data
{
    /// <summary>
    /// OPC Item Read/DataChange Result
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcItemResult
    {
        /// <summary>
        /// Error
        /// </summary>
        public int Error { get; set; }
        /// <summary>
        /// OPC Item server handle, valid only if Error == <see cref="Common.HRESULTS.S_OK"/>
        /// </summary>
        public int HandleServer { get; set; }
        /// <summary>
        /// OPC Item canonical data type, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public VarEnum CanonicalDataType { get; set; }
        /// <summary>
        /// OPC Item access rights, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public OPCACCESSRIGHTS AccessRights { get; set; }
        /// <summary>
        /// OPC Item blob, valid only if Error == <see cref="HRESULTS.S_OK"/>
        /// </summary>
        public byte[] Blob { get; set; }

        internal string DebuggerDisplay
        {
            get
            {
                if (Error == HRESULTS.S_OK)
                    return String.Format("Handle:{0} DataType:{1}, AccessRights", HandleServer, Extensions.VarEnumToString(CanonicalDataType), AccessRights);
                else
                    return String.Format("Handle:{0} Error:{1}", HandleServer, Error);
            }
        }
    }
}
