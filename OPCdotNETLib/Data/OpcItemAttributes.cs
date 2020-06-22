using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OPC.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Managed side only structs
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class OpcItemAttributes
    {
        /// <summary>
        /// Access paths
        /// </summary>
        public string AccessPath;
        /// <summary>
        /// Item ID
        /// </summary>
        public string ItemID;
        /// <summary>
        /// Iten is active
        /// </summary>
        public bool Active;
        /// <summary>
        /// Client handle
        /// </summary>
        public int HandleClient;
        /// <summary>
        /// Server handle
        /// </summary>
        public int HandleServer;
        /// <summary>
        /// Access rights
        /// </summary>
        public OPCACCESSRIGHTS AccessRights;
        /// <summary>
        /// Requested data type
        /// </summary>
        public VarEnum RequestedDataType;
        /// <summary>
        /// Canonical data type
        /// </summary>
        public VarEnum CanonicalDataType;
        /// <summary>
        /// EU type
        /// </summary>
        public OPCEUTYPE EUType;
        /// <summary>
        /// EU info
        /// </summary>
        public object EUInfo;
        /// <summary>
        /// Blob
        /// </summary>
        public byte[] Blob;

        internal string DebuggerDisplay
        {
            get
            {
                StringBuilder sb = new StringBuilder("OpcItemAttributes: '", 512);
                sb.Append(ItemID);
                sb.Append("' ('");
                sb.Append(AccessPath);
                sb.AppendFormat("') hc=0x{0:x} hs=0x{1:x} act={2}", HandleClient, HandleServer, Active);
                sb.AppendFormat("\r\n\tacc={0} typr={1} typc={2}", AccessRights, RequestedDataType, CanonicalDataType);
                sb.AppendFormat("\r\n\teut={0} eui={1}", EUType, EUInfo);
                if (!(Blob == null))
                    sb.AppendFormat(" blob size={0}", Blob.Length);

                return sb.ToString();
            }
        }
    }
}
